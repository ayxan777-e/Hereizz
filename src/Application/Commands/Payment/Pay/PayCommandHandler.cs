using Application.Commands.Payments.Pay;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System.Text.Json;

public class PayCommandHandler : IRequestHandler<PayCommand, BaseResponse<int>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICurrentUserService _currentUserService;

    public PayCommandHandler(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        ICurrentUserService currentUserService)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse<int>> Handle(PayCommand request, CancellationToken ct)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, ct);

        if (payment is null)
            return BaseResponse<int>.Fail("Payment not found", new(), ErrorType.NotFound);

        if (payment.UserId != _currentUserService.UserId)
            return BaseResponse<int>.Fail("Forbidden", new(), ErrorType.Forbidden);

        if (payment.Status == PaymentStatus.Paid)
            return BaseResponse<int>.Fail("Already paid", new(), ErrorType.BadRequest);

        var items = JsonSerializer.Deserialize<List<TempOrderItem>>(payment.OrderData);

        var order = new Order
        {
            UserId = payment.UserId,
            Status = OrderStatus.Confirmed,
            Items = items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                ProductTitle = x.Title,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                ShippingOptionId = x.ShippingOptionId,
                ShippingOptionName = x.ShippingOptionName,
                ShippingCost = x.ShippingCost,
                CustomsFee = x.CustomsFee,
                WarehouseFee = x.WarehouseFee,
                LocalDeliveryFee = x.LocalDeliveryFee,
                FinalPrice = x.FinalPrice,
                TransportType = x.TransportType,
                EstimatedMinDays = x.EstimatedMinDays,
                EstimatedMaxDays = x.EstimatedMaxDays
            }).ToList()
        };

        order.TotalPrice = order.Items.Sum(x => x.FinalPrice * x.Quantity);

        await _orderRepository.AddAsync(order, ct);

        payment.Status = PaymentStatus.Paid;
        payment.PaidAt = DateTime.UtcNow;
        payment.TransactionId = Guid.NewGuid().ToString();

        var cart = await _cartRepository.GetByUserIdWithItemsAsync(payment.UserId, ct);
        if (cart != null)
        {
            await _cartRepository.ClearCartAsync(cart.Id, ct);
        }

        await _paymentRepository.SaveChangesAsync(ct);

        return BaseResponse<int>.Ok(order.Id, "Order created");
    }
}
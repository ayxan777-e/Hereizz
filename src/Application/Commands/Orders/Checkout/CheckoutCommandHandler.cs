using Application.Commands.Orders.Checkout;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System.Text.Json;

public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, BaseResponse<int>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICurrentUserService _currentUserService;

    public CheckoutCommandHandler(
        ICartRepository cartRepository,
        IPaymentRepository paymentRepository,
        ICurrentUserService currentUserService)
    {
        _cartRepository = cartRepository;
        _paymentRepository = paymentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse<int>> Handle(CheckoutCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId;

        var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId, ct);

        if (cart is null || !cart.Items.Any())
        {
            return BaseResponse<int>.Fail(
                "Cart is empty",
                new List<string> { "No items in cart" },
                ErrorType.BadRequest);
        }

        var orderData = cart.Items.Select(x => new
        {
            x.ProductId,
            x.Product.Title,
            x.Quantity,
            x.UnitPrice,
            x.ShippingOptionId,
            x.ShippingOptionName,
            x.ShippingCost,
            x.CustomsFee,
            x.WarehouseFee,
            x.LocalDeliveryFee,
            x.FinalPrice,
            TransportType = x.TransportType.ToString(),
            x.EstimatedMinDays,
            x.EstimatedMaxDays
        });

        var payment = new Payment
        {
            UserId = userId,
            Amount = cart.Items.Sum(x => x.FinalPrice * x.Quantity),
            Method = PaymentMethod.Card,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            OrderData = JsonSerializer.Serialize(orderData)
        };

        await _paymentRepository.AddAsync(payment, ct);
        await _paymentRepository.SaveChangesAsync(ct);

        return BaseResponse<int>.Ok(payment.Id, "Payment created");
    }
}
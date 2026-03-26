using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Commands.Orders.Checkout;

public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, BaseResponse<int>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckoutCommandHandler(
        ICartRepository cartRepository,
        IOrderRepository orderRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<int>> Handle(CheckoutCommand request, CancellationToken ct)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            return BaseResponse<int>.Fail(
                "Unauthorized",
                new List<string> { "User context is missing." },
                ErrorType.Unauthorized);
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return BaseResponse<int>.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
        }

        var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId, ct);

        if (cart is null || cart.Items is null || !cart.Items.Any())
        {
            return BaseResponse<int>.Fail(
                "Checkout failed",
                new List<string> { "Cart is empty." },
                ErrorType.BadRequest);
        }

        var order = new Order
        {
            UserId = userId,
            Status = OrderStatus.Pending,
            Items = cart.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                ProductTitle = x.Product.Title,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                ShippingOptionId = x.ShippingOptionId,
                ShippingOptionName = x.ShippingOptionName,
                ShippingCost = x.ShippingCost,
                CustomsFee = x.CustomsFee,
                WarehouseFee = x.WarehouseFee,
                LocalDeliveryFee = x.LocalDeliveryFee,
                FinalPrice = x.FinalPrice,
                TransportType = x.TransportType.ToString(),
                EstimatedMinDays = x.EstimatedMinDays,
                EstimatedMaxDays = x.EstimatedMaxDays
            }).ToList()
        };

        order.TotalPrice = order.Items.Sum(x => x.FinalPrice * x.Quantity);

        await _orderRepository.AddAsync(order, ct);
        await _cartRepository.ClearCartAsync(cart.Id, ct);
        await _orderRepository.SaveChangesAsync(ct);

        return BaseResponse<int>.Ok(order.Id, "Checkout completed successfully");
    }
}
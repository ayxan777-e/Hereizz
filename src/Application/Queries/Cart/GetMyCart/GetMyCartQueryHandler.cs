using Application.DTOs.Cart;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Queries.Cart.GetMyCart;

public class GetMyCartQueryHandler : IRequestHandler<GetMyCartQuery, BaseResponse<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetMyCartQueryHandler(
        ICartRepository cartRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartRepository = cartRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<CartDto>> Handle(GetMyCartQuery request, CancellationToken ct)
    {
        if (!UserContextHelper.TryGetUserId<CartDto>(_httpContextAccessor, out var userId, out var errorResponse))
        {
            return errorResponse!;
        }

        var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId, ct);

        if (cart is null)
        {
            var emptyCart = new CartDto
            {
                UserId = userId,
                Items = new List<CartItemDto>(),
                TotalItemCount = 0,
                TotalPrice = 0
            };

            return BaseResponse<CartDto>.Ok(emptyCart, "Cart fetched successfully");
        }

        var dto = new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Select(x => new CartItemDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductTitle = x.Product.Title,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                ShippingCost = x.ShippingCost,
                CustomsFee = x.CustomsFee,
                WarehouseFee = x.WarehouseFee,
                LocalDeliveryFee = x.LocalDeliveryFee,
                FinalPrice = x.FinalPrice,
                ShippingOptionName = x.ShippingOptionName,
                TransportType = x.TransportType.ToString(),
                EstimatedMinDays = x.EstimatedMinDays,
                EstimatedMaxDays = x.EstimatedMaxDays
            }).ToList()
        };

        dto.TotalItemCount = dto.Items.Sum(x => x.Quantity);
        dto.TotalPrice = dto.Items.Sum(x => x.FinalPrice * x.Quantity);

        return BaseResponse<CartDto>.Ok(dto, "Cart fetched successfully");
    }
}

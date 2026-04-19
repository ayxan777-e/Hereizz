using Application.Helpers;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Cart.AddItem;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, BaseResponse>
{
    private readonly ICartService _cartService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddItemToCartCommandHandler(
        ICartService cartService,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartService = cartService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse> Handle(AddItemToCartCommand request, CancellationToken ct)
    {
        if (!UserContextHelper.TryGetUserId(_httpContextAccessor, out var userId, out var errorResponse))
        {
            return errorResponse!;
        }

        return await _cartService.AddItemAsync(userId, request.ProductId, request.ShippingOptionId, request.Quantity, ct);
    }
}

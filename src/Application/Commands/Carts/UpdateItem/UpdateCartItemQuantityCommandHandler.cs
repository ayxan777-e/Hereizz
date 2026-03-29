using Application.Interfaces.Repositories;
using Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Commands.Cart.UpdateItemQuantity;

public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, BaseResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateCartItemQuantityCommandHandler(
        ICartRepository cartRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartRepository = cartRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse> Handle(UpdateCartItemQuantityCommand request, CancellationToken ct)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            return BaseResponse.Fail(
                "Unauthorized",
                new List<string> { "User context is missing." },
                ErrorType.Unauthorized);
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return BaseResponse.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
        }

        var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId, ct);

        if (cart is null)
        {
            return BaseResponse.Fail(
                "Cart not found",
                new List<string> { "Cart was not found for this user." },
                ErrorType.NotFound);
        }

        var cartItem = cart.Items.FirstOrDefault(x => x.Id == request.ItemId);

        if (cartItem is null)
        {
            return BaseResponse.Fail(
                "Cart item not found",
                new List<string> { "The requested cart item does not exist in your cart." },
                ErrorType.NotFound);
        }

        cartItem.Quantity = request.Quantity;

        await _cartRepository.SaveChangesAsync(ct);

        return BaseResponse.Ok("Cart item quantity updated successfully.");
    }
}
using Application.Helpers;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Cart.RemoveItem;

public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, BaseResponse>
{
    private readonly ICartService _cartService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoveCartItemCommandHandler(
        ICartService cartService,
        IHttpContextAccessor httpContextAccessor)
    {
        _cartService = cartService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse> Handle(RemoveCartItemCommand request, CancellationToken ct)
    {
        if (!UserContextHelper.TryGetUserId(_httpContextAccessor, out var userId, out var errorResponse))
        {
            return errorResponse!;
        }

        return await _cartService.RemoveItemAsync(userId, request.ItemId, ct);
    }
}

using Application.Shared.Responses;

namespace Application.Interfaces.Services;

public interface ICartService
{
    Task<BaseResponse> AddItemAsync(string userId, int productId, int shippingOptionId, int quantity, CancellationToken ct);
    Task<BaseResponse> RemoveItemAsync(string userId, int itemId, CancellationToken ct);
}

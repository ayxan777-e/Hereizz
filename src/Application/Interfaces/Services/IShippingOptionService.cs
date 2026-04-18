using Application.Shared.Responses;
using Application.DTOs.ShippingOption;

namespace Application.Interfaces.Services;

public interface IShippingOptionService
{
    Task<BaseResponse<List<ShippingOptionListItemResponse>>> GetAllAsync(bool adminView, CancellationToken ct);
    Task<BaseResponse<ShippingOptionDetailResponse>> GetByIdAsync(int id, bool adminView, CancellationToken ct);

    Task<BaseResponse<int>> CreateAsync(CreateShippingOptionRequest request, CancellationToken ct);
    Task<BaseResponse> UpdateAsync(int id, UpdateShippingOptionRequest request, CancellationToken ct);
    Task<BaseResponse> DeactivateAsync(int id, CancellationToken ct);
    Task<BaseResponse> ActivateAsync(int id, CancellationToken ct);
}
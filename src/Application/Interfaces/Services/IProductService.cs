using Application.DTOs.Product;
using Application.Shared.Responses;

namespace Application.Interfaces.Services;

public interface IProductService
{
    Task<BaseResponse<List<ProductListItemResponse>>> GetAllAsync(bool adminView, CancellationToken ct);
    Task<BaseResponse<ProductDetailResponse>> GetByIdAsync(int id, bool adminView, CancellationToken ct);

    Task<BaseResponse<int>> ImportAsync(ImportProductRequest request, CancellationToken ct);
    Task<BaseResponse> UpdateAsync(int id, UpdateProductRequest request, CancellationToken ct);
    Task<BaseResponse> DeactivateAsync(int id, CancellationToken ct);
    Task<BaseResponse> ActivateAsync(int id, CancellationToken ct);
}
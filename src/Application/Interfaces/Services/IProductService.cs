using Application.DTOs.Product;

namespace Application.Interfaces.Services;

public interface IProductService
{
    Task<List<ProductListItemResponse>> GetAllAsync(CancellationToken ct);
    Task<ProductDetailResponse?> GetByIdAsync(int id, CancellationToken ct);
}
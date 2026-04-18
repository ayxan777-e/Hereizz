using Application.DTOs.Product;
using Domain.Enums;

namespace Application.Abstracts.Services;

public interface IProductProviderService
{
    Task<ProductProviderDataResponse> GetProductAsync(
        Marketplace marketplace,
        string externalProductId,
        CancellationToken ct = default);
}
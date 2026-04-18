using Application.Abstracts.Services;
using Application.DTOs.Product;
using Domain.Enums;

namespace Infrastructure.Services;

public class MockProductProviderService : IProductProviderService
{
    public Task<ProductProviderDataResponse> GetProductAsync(
        Marketplace marketplace,
        string externalProductId,
        CancellationToken ct = default)
    {
        var result = new ProductProviderDataResponse
        {
            Title = $"Imported product {externalProductId}",
            Description = "Mock provider-dan gətirilmiş məhsul",
            Price = 199.99m,
            Currency = Currency.USD,
            OriginCountry = Country.USA,
            WeightKg = 1.2m,
            Category = "Electronics",
            ImageUrl = "https://example.com/product.jpg",
            AffiliateUrl = $"https://example.com/products/{externalProductId}"
        };

        return Task.FromResult(result);
    }
}
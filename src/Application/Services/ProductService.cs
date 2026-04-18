using Application.Abstracts.Services;
using Application.DTOs.Product;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductProviderService _productProviderService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductRepository productRepository,
        IProductProviderService productProviderService,
        IMapper mapper,
        ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _productProviderService = productProviderService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<List<ProductListItemResponse>>> GetAllAsync(bool adminView, CancellationToken ct)
    {
        _logger.LogInformation("Getting product list. AdminView={AdminView}", adminView);

        var products = adminView
            ? await _productRepository.GetAllAsync(ct)
            : await _productRepository.GetAllActiveAsync(ct);

        var response = _mapper.Map<List<ProductListItemResponse>>(products);

        _logger.LogInformation("Product list loaded. Count={Count}", response.Count);

        return BaseResponse<List<ProductListItemResponse>>.Ok(response, "Product list uğurla gətirildi.");
    }

    public async Task<BaseResponse<ProductDetailResponse>> GetByIdAsync(int id, bool adminView, CancellationToken ct)
    {
        _logger.LogInformation("Getting product detail. ProductId={ProductId}, AdminView={AdminView}", id, adminView);

        var product = adminView
            ? await _productRepository.GetByIdAsync(id, ct)
            : await _productRepository.GetActiveByIdAsync(id, ct);

        if (product is null)
        {
            _logger.LogWarning("Product not found. ProductId={ProductId}, AdminView={AdminView}", id, adminView);

            return BaseResponse<ProductDetailResponse>.Fail(
                "Product tapılmadı.",
                new List<string> { $"Id-si {id} olan uyğun product mövcud deyil." },
                ErrorType.NotFound);
        }

        var response = _mapper.Map<ProductDetailResponse>(product);

        _logger.LogInformation("Product detail loaded. ProductId={ProductId}", id);

        return BaseResponse<ProductDetailResponse>.Ok(response, "Product detail uğurla gətirildi.");
    }

    public async Task<BaseResponse<int>> ImportAsync(ImportProductRequest request, CancellationToken ct)
    {
        _logger.LogInformation(
            "Importing product. Marketplace={Marketplace}, ExternalProductId={ExternalProductId}",
            request.Marketplace,
            request.ExternalProductId);

        var exists = await _productRepository.ExistsByMarketplaceAndExternalProductIdAsync(
            request.Marketplace,
            request.ExternalProductId,
            ct);

        if (exists)
        {
            _logger.LogWarning(
                "Duplicate product import blocked. Marketplace={Marketplace}, ExternalProductId={ExternalProductId}",
                request.Marketplace,
                request.ExternalProductId);

            return BaseResponse<int>.Fail(
                "Bu marketplace və external product id ilə məhsul artıq mövcuddur.",
                new List<string> { "Duplicate product import attempt." },
                ErrorType.Conflict);
        }

        var providerData = await _productProviderService.GetProductAsync(
            request.Marketplace,
            request.ExternalProductId,
            ct);

        var product = new Domain.Entities.Product
        {
            ExternalProductId = request.ExternalProductId,
            Marketplace = request.Marketplace,
            Title = providerData.Title,
            Description = providerData.Description,
            Price = providerData.Price,
            Currency = providerData.Currency,
            OriginCountry = providerData.OriginCountry,
            WeightKg = providerData.WeightKg,
            Category = providerData.Category,
            ImageUrl = providerData.ImageUrl,
            AffiliateUrl = providerData.AffiliateUrl,
            IsActive = true
        };

        await _productRepository.AddAsync(product, ct);
        await _productRepository.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Product imported successfully. ProductId={ProductId}, Marketplace={Marketplace}, ExternalProductId={ExternalProductId}",
            product.Id,
            product.Marketplace,
            product.ExternalProductId);

        return BaseResponse<int>.Ok(product.Id, "Product uğurla import edildi.");
    }

    public async Task<BaseResponse> UpdateAsync(int id, UpdateProductRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Updating product. ProductId={ProductId}", id);

        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null)
        {
            _logger.LogWarning("Product not found for update. ProductId={ProductId}", id);

            return BaseResponse.Fail(
                "Product tapılmadı.",
                new List<string> { $"Id-si {id} olan product mövcud deyil." },
                ErrorType.NotFound);
        }

        var duplicateAffiliateUrl = await _productRepository.ExistsByAffiliateUrlAsync(
            request.AffiliateUrl,
            id,
            ct);

        if (duplicateAffiliateUrl)
        {
            _logger.LogWarning(
                "Duplicate affiliate url blocked. ProductId={ProductId}, AffiliateUrl={AffiliateUrl}",
                id,
                request.AffiliateUrl);

            return BaseResponse.Fail(
                "Bu affiliate url başqa məhsul tərəfindən istifadə olunur.",
                new List<string> { "AffiliateUrl must be unique." },
                ErrorType.Conflict);
        }

        product.Title = request.Title;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Currency = request.Currency;
        product.OriginCountry = request.OriginCountry;
        product.WeightKg = request.WeightKg;
        product.Category = request.Category;
        product.ImageUrl = request.ImageUrl;
        product.AffiliateUrl = request.AffiliateUrl;
        product.IsActive = request.IsActive;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(ct);

        _logger.LogInformation("Product updated successfully. ProductId={ProductId}", id);

        return BaseResponse.Ok("Product uğurla yeniləndi.");
    }

    public async Task<BaseResponse> DeactivateAsync(int id, CancellationToken ct)
    {
        _logger.LogInformation("Deactivating product. ProductId={ProductId}", id);

        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null)
        {
            _logger.LogWarning("Product not found for deactivate. ProductId={ProductId}", id);

            return BaseResponse.Fail(
                "Product tapılmadı.",
                new List<string> { $"Id-si {id} olan product mövcud deyil." },
                ErrorType.NotFound);
        }

        if (!product.IsActive)
        {
            _logger.LogWarning("Product already inactive. ProductId={ProductId}", id);

            return BaseResponse.Fail(
                "Product artıq deaktivdir.",
                new List<string> { "Product is already inactive." },
                ErrorType.BusinessRule);
        }

        product.IsActive = false;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(ct);

        _logger.LogInformation("Product deactivated successfully. ProductId={ProductId}", id);

        return BaseResponse.Ok("Product deaktiv edildi.");
    }

    public async Task<BaseResponse> ActivateAsync(int id, CancellationToken ct)
    {
        _logger.LogInformation("Activating product. ProductId={ProductId}", id);

        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null)
        {
            _logger.LogWarning("Product not found for activate. ProductId={ProductId}", id);

            return BaseResponse.Fail(
                "Product tapılmadı.",
                new List<string> { $"Id-si {id} olan product mövcud deyil." },
                ErrorType.NotFound);
        }

        if (product.IsActive)
        {
            _logger.LogWarning("Product already active. ProductId={ProductId}", id);

            return BaseResponse.Fail(
                "Product artıq aktivdir.",
                new List<string> { "Product is already active." },
                ErrorType.BusinessRule);
        }

        product.IsActive = true;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(ct);

        _logger.LogInformation("Product activated successfully. ProductId={ProductId}", id);

        return BaseResponse.Ok("Product aktiv edildi.");
    }
}
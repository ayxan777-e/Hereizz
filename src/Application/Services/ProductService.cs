using Application.DTOs.Product;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ProductListItemResponse>> GetAllAsync(CancellationToken ct)
    {
        _logger.LogInformation("Getting product list");

        var products = await _productRepository.GetAllAsync(ct);

        _logger.LogInformation("Product list loaded. Count={Count}", products.Count);

        return _mapper.Map<List<ProductListItemResponse>>(products);
    }

    public async Task<ProductDetailResponse?> GetByIdAsync(int id, CancellationToken ct)
    {
        _logger.LogInformation("Getting product details. ProductId={ProductId}", id);

        var product = await _productRepository.GetByIdAsync(id, ct);

        if (product == null)
        {
            _logger.LogWarning("Product not found. ProductId={ProductId}", id);
            return null;
        }

        _logger.LogInformation("Product found. ProductId={ProductId}", id);

        return _mapper.Map<ProductDetailResponse>(product);
    }
}

using Application.DTOs.Product;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductListItemResponse>> GetAllAsync(CancellationToken ct)
    {
        var products = await _productRepository.GetAllAsync(ct);

        return _mapper.Map<List<ProductListItemResponse>>(products);
    }

    public async Task<ProductDetailResponse?> GetByIdAsync(int id, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);

        if (product == null)
            return null;

        return _mapper.Map<ProductDetailResponse>(product);
    }
}
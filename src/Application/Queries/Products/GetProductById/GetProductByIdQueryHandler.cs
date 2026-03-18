using Application.DTOs.Product;
using Application.Interfaces.Services;
using MediatR;

namespace Application.Queries.Products.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductDetailResponse?>
{
    private readonly IProductService _productService;

    public GetProductByIdQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDetailResponse?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.Id, cancellationToken);

        return product;
    }
}
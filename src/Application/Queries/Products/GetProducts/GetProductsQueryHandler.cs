using Application.DTOs.Product;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Products.GetProducts;

public class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, BaseResponse<List<ProductListItemResponse>>>
{
    private readonly IProductService _productService;

    public GetProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<BaseResponse<List<ProductListItemResponse>>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await _productService.GetAllAsync(request.AdminView, cancellationToken);
    }
}
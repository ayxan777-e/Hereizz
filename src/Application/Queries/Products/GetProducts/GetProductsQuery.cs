using Application.DTOs.Product;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Products.GetProducts;

public class GetProductsQuery : IRequest<BaseResponse<List<ProductListItemResponse>>>
{
    public bool AdminView { get; set; }

    public GetProductsQuery(bool adminView = false)
    {
        AdminView = adminView;
    }
}
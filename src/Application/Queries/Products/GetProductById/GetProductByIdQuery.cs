using Application.DTOs.Product;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Products.GetProductById;

public class GetProductByIdQuery : IRequest<BaseResponse<ProductDetailResponse>>
{
    public int Id { get; set; }
    public bool AdminView { get; set; }

    public GetProductByIdQuery(int id, bool adminView = false)
    {
        Id = id;
        AdminView = adminView;
    }
}
using Application.DTOs.Product;
using MediatR;

namespace Application.Queries.Products.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDetailResponse?>
{
    public int Id { get; set; }

    public GetProductByIdQuery(int id)
    {
        Id = id;
    }
}
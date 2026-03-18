using Application.DTOs.Product;
using MediatR;

namespace Application.Queries.Products.GetProducts;

public class GetProductsQuery : IRequest<List<ProductListItemResponse>>
{
}
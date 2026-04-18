using Application.DTOs.Product;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Product.UpdateProduct;

public class UpdateProductCommand : IRequest<BaseResponse>
{
    public int Id { get; set; }
    public UpdateProductRequest Request { get; set; }

    public UpdateProductCommand(int id, UpdateProductRequest request)
    {
        Id = id;
        Request = request;
    }
}
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Product.DeleteProduct;

public class DeleteProductCommand : IRequest<BaseResponse>
{
    public int Id { get; set; }

    public DeleteProductCommand(int id)
    {
        Id = id;
    }
}
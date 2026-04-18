using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Product.ActivateProduct;

public class ActivateProductCommand : IRequest<BaseResponse>
{
    public int Id { get; set; }

    public ActivateProductCommand(int id)
    {
        Id = id;
    }
}
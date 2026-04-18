using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.ShippingOption.DeleteShippingOption;

public class DeleteShippingOptionCommand : IRequest<BaseResponse>
{
    public int Id { get; set; }

    public DeleteShippingOptionCommand(int id)
    {
        Id = id;
    }
}
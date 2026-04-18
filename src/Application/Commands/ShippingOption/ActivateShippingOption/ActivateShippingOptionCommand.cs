using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.ShippingOption.ActivateShippingOption;

public class ActivateShippingOptionCommand : IRequest<BaseResponse>
{
    public int Id { get; set; }

    public ActivateShippingOptionCommand(int id)
    {
        Id = id;
    }
}
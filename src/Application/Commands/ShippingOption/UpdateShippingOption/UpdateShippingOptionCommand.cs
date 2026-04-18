using Application.DTOs.ShippingOption;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.ShippingOption.UpdateShippingOption;

public class UpdateShippingOptionCommand : IRequest<BaseResponse>
{
    public int Id { get; set; }
    public UpdateShippingOptionRequest Request { get; set; }

    public UpdateShippingOptionCommand(int id, UpdateShippingOptionRequest request)
    {
        Id = id;
        Request = request;
    }
}
using Application.DTOs.ShippingOption;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.ShippingOption.CreateShippingOption;

public class CreateShippingOptionCommand : IRequest<BaseResponse<int>>
{
    public CreateShippingOptionRequest Request { get; set; }

    public CreateShippingOptionCommand(CreateShippingOptionRequest request)
    {
        Request = request;
    }
}
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.ShippingOption.ActivateShippingOption;

public class ActivateShippingOptionCommandHandler
    : IRequestHandler<ActivateShippingOptionCommand, BaseResponse>
{
    private readonly IShippingOptionService _service;

    public ActivateShippingOptionCommandHandler(IShippingOptionService service)
    {
        _service = service;
    }

    public async Task<BaseResponse> Handle(
        ActivateShippingOptionCommand request,
        CancellationToken cancellationToken)
    {
        return await _service.ActivateAsync(request.Id, cancellationToken);
    }
}
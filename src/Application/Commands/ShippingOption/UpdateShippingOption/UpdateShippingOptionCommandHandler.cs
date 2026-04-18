using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.ShippingOption.UpdateShippingOption;

public class UpdateShippingOptionCommandHandler
    : IRequestHandler<UpdateShippingOptionCommand, BaseResponse>
{
    private readonly IShippingOptionService _service;

    public UpdateShippingOptionCommandHandler(IShippingOptionService service)
    {
        _service = service;
    }

    public async Task<BaseResponse> Handle(
        UpdateShippingOptionCommand request,
        CancellationToken cancellationToken)
    {
        return await _service.UpdateAsync(
            request.Id,
            request.Request,
            cancellationToken);
    }
}
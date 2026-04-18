using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.ShippingOption.CreateShippingOption;

public class CreateShippingOptionCommandHandler
    : IRequestHandler<CreateShippingOptionCommand, BaseResponse<int>>
{
    private readonly IShippingOptionService _service;

    public CreateShippingOptionCommandHandler(IShippingOptionService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<int>> Handle(
        CreateShippingOptionCommand request,
        CancellationToken cancellationToken)
    {
        return await _service.CreateAsync(request.Request, cancellationToken);
    }
}
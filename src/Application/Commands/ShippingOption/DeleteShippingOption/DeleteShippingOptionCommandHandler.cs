using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.ShippingOption.DeleteShippingOption;

public class DeleteShippingOptionCommandHandler
    : IRequestHandler<DeleteShippingOptionCommand, BaseResponse>
{
    private readonly IShippingOptionService _service;

    public DeleteShippingOptionCommandHandler(IShippingOptionService service)
    {
        _service = service;
    }

    public async Task<BaseResponse> Handle(
        DeleteShippingOptionCommand request,
        CancellationToken cancellationToken)
    {
        return await _service.DeactivateAsync(request.Id, cancellationToken);
    }
}
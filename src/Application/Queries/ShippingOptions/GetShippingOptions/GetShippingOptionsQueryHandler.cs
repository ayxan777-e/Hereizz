using Application.DTOs.ShippingOption;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.ShippingOptions.GetShippingOptions;

public class GetShippingOptionsQueryHandler
    : IRequestHandler<GetShippingOptionsQuery, BaseResponse<List<ShippingOptionListItemResponse>>>
{
    private readonly IShippingOptionService _shippingOptionService;

    public GetShippingOptionsQueryHandler(IShippingOptionService shippingOptionService)
    {
        _shippingOptionService = shippingOptionService;
    }

    public async Task<BaseResponse<List<ShippingOptionListItemResponse>>> Handle(
        GetShippingOptionsQuery request,
        CancellationToken cancellationToken)
    {
        return await _shippingOptionService.GetAllAsync(request.AdminView, cancellationToken);
    }
}
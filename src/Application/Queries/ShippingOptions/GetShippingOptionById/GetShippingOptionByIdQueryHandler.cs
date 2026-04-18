using Application.DTOs.ShippingOption;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.ShippingOptions.GetShippingOptionById;

public class GetShippingOptionByIdQueryHandler
    : IRequestHandler<GetShippingOptionByIdQuery, BaseResponse<ShippingOptionDetailResponse>>
{
    private readonly IShippingOptionService _shippingOptionService;

    public GetShippingOptionByIdQueryHandler(IShippingOptionService shippingOptionService)
    {
        _shippingOptionService = shippingOptionService;
    }

    public async Task<BaseResponse<ShippingOptionDetailResponse>> Handle(
        GetShippingOptionByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _shippingOptionService.GetByIdAsync(
            request.Id,
            request.AdminView,
            cancellationToken);
    }
}
using Application.DTOs.ShippingOption;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.ShippingOptions.GetShippingOptions;

public class GetShippingOptionsQuery : IRequest<BaseResponse<List<ShippingOptionListItemResponse>>>
{
    public bool AdminView { get; set; }

    public GetShippingOptionsQuery(bool adminView = false)
    {
        AdminView = adminView;
    }
}
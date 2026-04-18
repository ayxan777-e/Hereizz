using Application.DTOs.ShippingOption;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.ShippingOptions.GetShippingOptionById;

public class GetShippingOptionByIdQuery : IRequest<BaseResponse<ShippingOptionDetailResponse>>
{
    public int Id { get; set; }
    public bool AdminView { get; set; }

    public GetShippingOptionByIdQuery(int id, bool adminView = false)
    {
        Id = id;
        AdminView = adminView;
    }
}
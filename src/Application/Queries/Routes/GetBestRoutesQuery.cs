using Application.DTOs.Calculation;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Routes;

public class GetBestRoutesQuery : IRequest<BaseResponse<RouteSelectionResponse>>
{
    public int ProductId { get; set; }

    public GetBestRoutesQuery(int productId)
    {
        ProductId = productId;
    }
}
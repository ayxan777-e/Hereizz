using Application.DTOs.Calculation;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Routes;

public class GetBestRoutesQueryHandler
    : IRequestHandler<GetBestRoutesQuery, BaseResponse<RouteSelectionResponse>>
{
    private readonly IRouteSelectionService _routeSelectionService;

    public GetBestRoutesQueryHandler(IRouteSelectionService routeSelectionService)
    {
        _routeSelectionService = routeSelectionService;
    }

    public async Task<BaseResponse<RouteSelectionResponse>> Handle(
        GetBestRoutesQuery request,
        CancellationToken cancellationToken)
    {
        return await _routeSelectionService.SelectBestRoutesAsync(
            request.ProductId,
            cancellationToken);
    }

}
using Application.DTOs.Calculation;
using Application.Shared.Responses;

namespace Application.Interfaces.Services;

public interface IRouteSelectionService
{
    Task<BaseResponse<RouteSelectionResponse>> SelectBestRoutesAsync(int productId, CancellationToken ct);
}
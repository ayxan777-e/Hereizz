using Application.DTOs.Calculation;

namespace Application.Interfaces.Services;

public interface IRouteSelectionService
{
    Task<RouteSelectionResponse> SelectBestRoutesAsync(int productId, CancellationToken ct);
}
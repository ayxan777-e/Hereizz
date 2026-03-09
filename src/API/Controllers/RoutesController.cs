using Application.DTOs.Calculation;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController : ControllerBase
{
    private readonly IRouteSelectionService _routeSelectionService;

    public RoutesController(IRouteSelectionService routeSelectionService)
    {
        _routeSelectionService = routeSelectionService;
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<RouteSelectionResponse>> GetBestRoutes(int productId, CancellationToken ct)
    {
        var result = await _routeSelectionService.SelectBestRoutesAsync(productId, ct);
        return Ok(result);
    }
}
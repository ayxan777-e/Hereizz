using Application.DTOs.Calculation;
using Application.Queries.Routes;
using Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoutesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<BaseResponse<RouteSelectionResponse>>> GetBestRoutes(int productId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBestRoutesQuery(productId), ct);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
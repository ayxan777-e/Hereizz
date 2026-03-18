using Application.DTOs.Calculation;
using Application.Queries.Calculation.CalculatePrice;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculationController : ControllerBase
{
    private readonly IMediator _mediator;

    public CalculationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<List<PriceCalculationResponse>>> Calculate(
        int productId,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CalculatePriceQuery(productId), ct);

        return Ok(result);
    }
}
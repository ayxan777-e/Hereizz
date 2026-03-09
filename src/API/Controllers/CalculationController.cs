using Application.DTOs.Calculation;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculationController : ControllerBase
{
    private readonly IPriceCalculatorService _priceCalculatorService;

    public CalculationController(IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<List<PriceCalculationResponse>>> Calculate(
        int productId,
        CancellationToken ct)
    {
        var result = await _priceCalculatorService.CalculateAsync(productId, ct);

        return Ok(result);
    }
}
using Application.DTOs.Calculation;
using Application.Interfaces.Services;

namespace Application.Services;

public class RouteSelectionService : IRouteSelectionService
{
    private readonly IPriceCalculatorService _priceCalculatorService;

    public RouteSelectionService(IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }

    public async Task<RouteSelectionResponse> SelectBestRoutesAsync(int productId, CancellationToken ct)
    {
        var calculations = await _priceCalculatorService.CalculateAsync(productId, ct);

        if (calculations == null || calculations.Count == 0)
        {
            return new RouteSelectionResponse();
        }

        var cheapest = calculations
            .OrderBy(x => x.FinalPrice)
            .FirstOrDefault();

        var fastest = calculations
            .OrderBy(x => x.EstimatedMinDays)
            .FirstOrDefault();

        PriceCalculationResponse? balanced = null;

        if (cheapest != null && fastest != null)
        {
            balanced = calculations
                .OrderBy(x =>
                    Math.Abs(x.FinalPrice - cheapest.FinalPrice) +
                    Math.Abs(x.EstimatedMinDays - fastest.EstimatedMinDays))
                .FirstOrDefault();
        }

        return new RouteSelectionResponse
        {
            Cheapest = cheapest,
            Fastest = fastest,
            Balanced = balanced
        };
    }
}
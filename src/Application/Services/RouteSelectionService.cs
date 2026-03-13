using Application.DTOs.Calculation;
using Application.Interfaces.Services;
using Application.Shared.Responses;

namespace Application.Services;

public class RouteSelectionService : IRouteSelectionService
{
    private readonly IPriceCalculatorService _priceCalculatorService;

    public RouteSelectionService(IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }

    public async Task<BaseResponse<RouteSelectionResponse>> SelectBestRoutesAsync(int productId, CancellationToken ct)
    {
        var calculations = await _priceCalculatorService.CalculateAsync(productId, ct);

        if (calculations.Count == 0)
        {
            return BaseResponse<RouteSelectionResponse>.Fail(
                    "No routes found",
                    new List<string> { $"Shipping options not available for  {productId} Id " }
                );
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

        var response = new RouteSelectionResponse
        {
            Cheapest = cheapest,
            Fastest = fastest,
            Balanced = balanced
        };

        return BaseResponse<RouteSelectionResponse>.Ok(response, "Routes calculated successfully");
    }
}
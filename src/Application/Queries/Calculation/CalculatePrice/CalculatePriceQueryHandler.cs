using Application.DTOs.Calculation;
using Application.Interfaces.Services;
using MediatR;

namespace Application.Queries.Calculation.CalculatePrice;

public class CalculatePriceQueryHandler
    : IRequestHandler<CalculatePriceQuery, List<PriceCalculationResponse>>
{
    private readonly IPriceCalculatorService _priceCalculatorService;

    public CalculatePriceQueryHandler(IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }

    public async Task<List<PriceCalculationResponse>> Handle(
        CalculatePriceQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _priceCalculatorService.CalculateAsync(request.ProductId, cancellationToken);

        return result;
    }
}
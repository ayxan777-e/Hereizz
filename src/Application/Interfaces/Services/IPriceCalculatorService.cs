using Application.DTOs.Calculation;

namespace Application.Interfaces.Services;

public interface IPriceCalculatorService
{
    Task<List<PriceCalculationResponse>> CalculateAsync(int productId, CancellationToken ct);
}
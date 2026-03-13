using Application.DTOs.Calculation;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IPriceCalculatorService
{
    Task<List<PriceCalculationResponse>> CalculateAsync(int productId, CancellationToken ct);
    Task<PriceCalculationResponse?> CalculateForOptionAsync(Product product, ShippingOption shippingOption);
}
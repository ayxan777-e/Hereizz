using Application.DTOs.Calculation;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private readonly IProductRepository _productRepository;
    private readonly IShippingOptionRepository _shippingRepository;

    public PriceCalculatorService(
        IProductRepository productRepository,
        IShippingOptionRepository shippingRepository)
    {
        _productRepository = productRepository;
        _shippingRepository = shippingRepository;
    }

    public async Task<List<PriceCalculationResponse>> CalculateAsync(int productId, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);

        if (product == null)
            return new List<PriceCalculationResponse>();

        var shippingOptions = await _shippingRepository
                                                      .GetByOriginCountryAsync(product.OriginCountry, ct);

        var results = new List<PriceCalculationResponse>();

        foreach (var option in shippingOptions)
        {

            var shippingCost = (product.WeightKg * option.PricePerKg) + option.FixedFee;

            var customsFee = product.Price * 0.05m; // 5% customs example

            var warehouseFee = 2;

            var localDeliveryFee = 3;

            var finalPrice =
                product.Price +
                shippingCost +
                customsFee +
                warehouseFee +
                localDeliveryFee;

            results.Add(new PriceCalculationResponse
            {
                ProductId = product.Id,
                ProductTitle = product.Title,
                ProductPrice = product.Price,
                ShippingCost = shippingCost,
                CustomsFee = customsFee,
                WarehouseFee = warehouseFee,
                LocalDeliveryFee = localDeliveryFee,
                FinalPrice = finalPrice,
                TransportType = option.TransportType,
                EstimatedMinDays = option.EstimatedMinDays,
                EstimatedMaxDays = option.EstimatedMaxDays
            });
        }

        return results;
    }
}
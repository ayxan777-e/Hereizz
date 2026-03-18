using Application.DTOs.Calculation;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private readonly IProductRepository _productRepository;
    private readonly IShippingOptionRepository _shippingRepository;
    private readonly IFeeCalculator _feeCalculator;

    public PriceCalculatorService(
        IProductRepository productRepository,
        IShippingOptionRepository shippingRepository,
        IFeeCalculator feeCalculator)
    {
        _productRepository = productRepository;
        _shippingRepository = shippingRepository;
        _feeCalculator = feeCalculator;
    }

    public async Task<List<PriceCalculationResponse>> CalculateAsync(int productId, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);

        if (product is null)
            throw new KeyNotFoundException("Product not found");

        var shippingOptions = await _shippingRepository.GetByOriginCountryAsync(product.OriginCountry, ct);

        var results = new List<PriceCalculationResponse>();

        foreach (var option in shippingOptions)
        {
            var shippingCost = (product.WeightKg * option.PricePerKg) + option.FixedFee;

            var fees = _feeCalculator.CalculateFees(product, option);
            var totalFees = fees.Sum(x => x.Amount);

            var customsFee = fees.FirstOrDefault(x => x.Name == "Customs")?.Amount ?? 0m;
            var warehouseFee = fees.FirstOrDefault(x => x.Name == "Warehouse")?.Amount ?? 0m;
            var localDeliveryFee = fees.FirstOrDefault(x => x.Name == "LocalDelivery")?.Amount ?? 0m;

            var finalPrice = product.Price + shippingCost + totalFees;

            results.Add(new PriceCalculationResponse
            {
                ProductTitle = product.Title,
                ShippingOptionName = option.Name,
                ProductPrice = product.Price,
                ShippingCost = shippingCost,
                CustomsFee = customsFee,
                WarehouseFee = warehouseFee,
                LocalDeliveryFee = localDeliveryFee,
                FinalPrice = finalPrice,
                EstimatedMinDays = option.EstimatedMinDays,
                EstimatedMaxDays = option.EstimatedMaxDays,
                Fees = fees
            });
        }

        return results;
    }

    public Task<PriceCalculationResponse?> CalculateForOptionAsync(Product product, ShippingOption shippingOption)
    {
        if (product is null)
            return Task.FromResult<PriceCalculationResponse?>(null);

        if (shippingOption is null)
            return Task.FromResult<PriceCalculationResponse?>(null);

        if (!shippingOption.IsActive)
            return Task.FromResult<PriceCalculationResponse?>(null);

        if (shippingOption.OriginCountry != product.OriginCountry)
            return Task.FromResult<PriceCalculationResponse?>(null);

        var shippingCost = (product.WeightKg * shippingOption.PricePerKg) + shippingOption.FixedFee;

        var fees = _feeCalculator.CalculateFees(product, shippingOption);

        var customsFee = fees.FirstOrDefault(x => x.Name == "Customs")?.Amount ?? 0m;
        var warehouseFee = fees.FirstOrDefault(x => x.Name == "Warehouse")?.Amount ?? 0m;
        var localDeliveryFee = fees.FirstOrDefault(x => x.Name == "LocalDelivery")?.Amount ?? 0m;

        var totalFees = fees.Sum(x => x.Amount);

        var finalPrice = product.Price + shippingCost + totalFees;

        var response = new PriceCalculationResponse
        {
            ProductTitle = product.Title,
            ShippingOptionName = shippingOption.Name,
            ProductPrice = product.Price,
            ShippingCost = shippingCost,
            CustomsFee = customsFee,
            WarehouseFee = warehouseFee,
            LocalDeliveryFee = localDeliveryFee,
            FinalPrice = finalPrice,
            TransportType = shippingOption.TransportType,
            EstimatedMinDays = shippingOption.EstimatedMinDays,
            EstimatedMaxDays = shippingOption.EstimatedMaxDays,
            Fees = fees
        };

        return Task.FromResult<PriceCalculationResponse?>(response);
    }
}
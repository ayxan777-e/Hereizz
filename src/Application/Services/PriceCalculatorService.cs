using Application.DTOs.Calculation;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private const string CustomsFeeName = "Customs";
    private const string WarehouseFeeName = "Warehouse";
    private const string LocalDeliveryFeeName = "LocalDelivery";

    private readonly IProductRepository _productRepository;
    private readonly IShippingOptionRepository _shippingRepository;
    private readonly IFeeCalculator _feeCalculator;
    private readonly ICurrencyService _currencyService;

    public PriceCalculatorService(
        IProductRepository productRepository,
        IShippingOptionRepository shippingRepository,
        IFeeCalculator feeCalculator,
        ICurrencyService currencyService)
    {
        _productRepository = productRepository;
        _shippingRepository = shippingRepository;
        _feeCalculator = feeCalculator;
        _currencyService = currencyService;
    }

    public async Task<List<PriceCalculationResponse>> CalculateAsync(
        int productId,
        CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct)
            ?? throw new KeyNotFoundException("Product not found");

        var shippingOptions = await _shippingRepository
            .GetByOriginCountryAsync(product.OriginCountry, ct);

        return shippingOptions
            .Where(option => IsEligibleForCalculation(product, option))
            .Select(option => BuildPriceResponse(product, option))
            .ToList();
    }

    public Task<PriceCalculationResponse?> CalculateForOptionAsync(
        Product product,
        ShippingOption shippingOption)
    {
        if (!IsEligibleForCalculation(product, shippingOption))
            return Task.FromResult<PriceCalculationResponse?>(null);

        var response = BuildPriceResponse(product, shippingOption);

        return Task.FromResult<PriceCalculationResponse?>(response);
    }

    private static bool IsEligibleForCalculation(Product product, ShippingOption option)
    {
        if (product is null || option is null)
            return false;

        return option.IsActive &&
               option.OriginCountry == product.OriginCountry;
    }

    private PriceCalculationResponse BuildPriceResponse(Product product, ShippingOption option)
    {
        var productPriceAzn = _currencyService.ConvertToAzn(
            product.Price,
            product.Currency.ToString());

        var shippingCost = CalculateShippingCost(product, option);

        var fees = _feeCalculator.CalculateFees(product, option);
        var totalFees = fees.Sum(x => x.Amount);

        var customsFee = GetFeeAmount(fees, CustomsFeeName);
        var warehouseFee = GetFeeAmount(fees, WarehouseFeeName);
        var localDeliveryFee = GetFeeAmount(fees, LocalDeliveryFeeName);

        return new PriceCalculationResponse
        {
            ProductId = product.Id,
            ProductTitle = product.Title,

            ShippingOptionId = option.Id,
            ShippingOptionName = option.Name,

            ProductPrice = productPriceAzn,
            ShippingCost = shippingCost,

            CustomsFee = customsFee,
            WarehouseFee = warehouseFee,
            LocalDeliveryFee = localDeliveryFee,

            FinalPrice = productPriceAzn + shippingCost + totalFees,

            TransportType = option.TransportType,
            EstimatedMinDays = option.EstimatedMinDays,
            EstimatedMaxDays = option.EstimatedMaxDays,

            Fees = fees
        };
    }

    private static decimal CalculateShippingCost(Product product, ShippingOption option)
    {
        return (product.WeightKg * option.PricePerKg) + option.FixedFee;
    }

    private static decimal GetFeeAmount(IEnumerable<FeeBreakdownItem> fees, string feeName)
    {
        return fees.FirstOrDefault(x => x.Name == feeName)?.Amount ?? 0m;
    }
}
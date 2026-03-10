using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services.FeeRules;

public class LocalDeliveryFeeRule : IFeeRule
{
    public string Name => "LocalDelivery";

    public decimal Calculate(Product product, ShippingOption shippingOption)
    {
        return 3m;
    }
}
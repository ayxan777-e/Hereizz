using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services.FeeRules;

public class CustomsFeeRule : IFeeRule
{
    public string Name => "Customs";

    public decimal Calculate(Product product, ShippingOption shippingOption)
    {
        return product.Price * 0.05m;
    }
}
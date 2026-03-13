using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services.FeeRules;

public class WarehouseFeeRule : IFeeRule
{
    public string Name => "Warehouse";

    public decimal Calculate(Product product, ShippingOption shippingOption)
    {
        return 2m;
    }
}
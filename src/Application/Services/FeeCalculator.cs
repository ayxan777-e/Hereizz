using Application.DTOs.Calculation;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services;

public class FeeCalculator : IFeeCalculator
{
    private readonly IEnumerable<IFeeRule> _feeRules;

    public FeeCalculator(IEnumerable<IFeeRule> feeRules)
    {
        _feeRules = feeRules;
    }

    public List<FeeBreakdownItem> CalculateFees(Product product, ShippingOption shippingOption)
    {
        var result = new List<FeeBreakdownItem>();

        foreach (var rule in _feeRules)
        {
            var amount = rule.Calculate(product, shippingOption);

            result.Add(new FeeBreakdownItem
            {
                Name = rule.Name,
                Amount = amount
            });
        }

        return result;
    }
}
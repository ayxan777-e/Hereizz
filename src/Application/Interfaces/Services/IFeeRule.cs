using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IFeeRule
{
    string Name { get; }
    decimal Calculate(Product product, ShippingOption shippingOption);
}
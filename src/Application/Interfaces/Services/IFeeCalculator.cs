using Application.DTOs.Calculation;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IFeeCalculator
{
    List<FeeBreakdownItem> CalculateFees(Product product, ShippingOption shippingOption);
}
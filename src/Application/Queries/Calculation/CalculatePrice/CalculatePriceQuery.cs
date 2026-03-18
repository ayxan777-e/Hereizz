using Application.DTOs.Calculation;
using MediatR;

namespace Application.Queries.Calculation.CalculatePrice;

public class CalculatePriceQuery : IRequest<List<PriceCalculationResponse>>
{
    public int ProductId { get; set; }

    public CalculatePriceQuery(int productId)
    {
        ProductId = productId;
    }
}
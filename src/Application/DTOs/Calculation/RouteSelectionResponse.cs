namespace Application.DTOs.Calculation;

public class RouteSelectionResponse
{
    public PriceCalculationResponse? Cheapest { get; set; }
    public PriceCalculationResponse? Fastest { get; set; }
    public PriceCalculationResponse? Balanced { get; set; }
}
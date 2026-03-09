using Domain.Enums;

namespace Application.DTOs.Calculation;

public class PriceCalculationResponse
{
    public int ProductId { get; set; }

    public string ProductTitle { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal CustomsFee { get; set; }

    public decimal WarehouseFee { get; set; }

    public decimal LocalDeliveryFee { get; set; }

    public decimal FinalPrice { get; set; }

    public TransportType TransportType { get; set; }

    public int EstimatedMinDays { get; set; }

    public int EstimatedMaxDays { get; set; }
}
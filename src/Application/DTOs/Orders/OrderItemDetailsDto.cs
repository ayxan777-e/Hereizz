namespace Application.DTOs.Orders;

public class OrderItemDetailsDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int ShippingOptionId { get; set; }
    public string ShippingOptionName { get; set; } = string.Empty;

    public decimal ShippingCost { get; set; }
    public decimal CustomsFee { get; set; }
    public decimal WarehouseFee { get; set; }
    public decimal LocalDeliveryFee { get; set; }

    public decimal FinalPrice { get; set; }

    public string TransportType { get; set; } = string.Empty;
    public int EstimatedMinDays { get; set; }
    public int EstimatedMaxDays { get; set; }
}
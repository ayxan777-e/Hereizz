namespace Application.DTOs.Orders;

public class OrderDetailsDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;

    public int ShippingOptionId { get; set; }
    public string ShippingOptionName { get; set; } = string.Empty;

    public decimal ProductPrice { get; set; }
    public decimal ShippingCost { get; set; }

    public decimal CustomsFee { get; set; }
    public decimal WarehouseFee { get; set; }
    public decimal LocalDeliveryFee { get; set; }

    public decimal FinalPrice { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
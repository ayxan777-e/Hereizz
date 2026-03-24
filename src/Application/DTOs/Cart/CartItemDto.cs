namespace Application.DTOs.Cart;

public class CartItemDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public string ProductTitle { get; set; } = null!;

    public int Quantity { get; set; }

    // 🔥 Pricing snapshot
    public decimal UnitPrice { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal CustomsFee { get; set; }
    public decimal WarehouseFee { get; set; }
    public decimal LocalDeliveryFee { get; set; }

    public decimal FinalPrice { get; set; }

    // 🔥 Route məlumatı
    public string ShippingOptionName { get; set; } = null!;
    public string TransportType { get; set; } = null!;

    public int EstimatedMinDays { get; set; }
    public int EstimatedMaxDays { get; set; }
}
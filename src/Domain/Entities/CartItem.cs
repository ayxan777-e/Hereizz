using Domain.Enums;

namespace Domain.Entities;

public class CartItem
{
    public int Id { get; set; }

    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    // 🔥 Snapshot pricing
    public decimal UnitPrice { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal CustomsFee { get; set; }
    public decimal WarehouseFee { get; set; }
    public decimal LocalDeliveryFee { get; set; }

    public decimal FinalPrice { get; set; }

    // 🔥 Route məlumatı
    public string ShippingOptionName { get; set; } = null!;
    public TransportType TransportType { get; set; }

    public int EstimatedMinDays { get; set; }
    public int EstimatedMaxDays { get; set; }
}
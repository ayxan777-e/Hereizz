using Domain.Entities.Common;

namespace Domain.Entities;

public class OrderItem : BaseEntity<int>
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public string ProductTitle { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int ShippingOptionId { get; set; }
    public string ShippingOptionName { get; set; } = null!;

    public decimal ShippingCost { get; set; }
    public decimal CustomsFee { get; set; }
    public decimal WarehouseFee { get; set; }
    public decimal LocalDeliveryFee { get; set; }

    public decimal FinalPrice { get; set; }

    public string TransportType { get; set; } = null!;
    public int EstimatedMinDays { get; set; }
    public int EstimatedMaxDays { get; set; }
}
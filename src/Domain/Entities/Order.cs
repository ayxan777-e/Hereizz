using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Order : BaseEntity<int>
{

    public int ProductId { get; set; }

    public int ShippingOptionId { get; set; }

    public decimal ProductPrice { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal CustomsFee { get; set; }

    public decimal WarehouseFee { get; set; }

    public decimal LocalDeliveryFee { get; set; }

    public decimal FinalPrice { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;


    public Product Product { get; set; } = null!;

    public ShippingOption ShippingOption { get; set; } = null!;
}
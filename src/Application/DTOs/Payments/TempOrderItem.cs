public class TempOrderItem
{
    public int ProductId { get; set; }
    public string Title { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int ShippingOptionId { get; set; }
    public string ShippingOptionName { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal CustomsFee { get; set; }
    public decimal WarehouseFee { get; set; }
    public decimal LocalDeliveryFee { get; set; }
    public decimal FinalPrice { get; set; }
    public string TransportType { get; set; }
    public int EstimatedMinDays { get; set; }
    public int EstimatedMaxDays { get; set; }
}
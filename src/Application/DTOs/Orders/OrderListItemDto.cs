namespace Application.DTOs.Orders;

public class OrderListItemDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;

    public int ShippingOptionId { get; set; }
    public string ShippingOptionName { get; set; } = string.Empty;

    public decimal FinalPrice { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
namespace Application.DTOs.Orders;

public class OrderListItemDto
{
    public int Id { get; set; }
    public int ItemCount { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
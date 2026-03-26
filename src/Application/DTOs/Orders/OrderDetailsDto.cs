using Application.DTOs.Orders;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<OrderItemDetailsDto> Items { get; set; } = new();
}
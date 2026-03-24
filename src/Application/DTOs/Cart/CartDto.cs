namespace Application.DTOs.Cart;

public class CartDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;

    public List<CartItemDto> Items { get; set; } = new();

    public int TotalItemCount { get; set; }
    public decimal TotalPrice { get; set; }
}

using Domain.Enums;

namespace Application.DTOs.Product;

public class ProductListItemResponse
{
    public int Id { get; set; }
    public string ExternalProductId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public Currency Currency { get; set; }
    public Marketplace Marketplace { get; set; }
    public string Category { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
}
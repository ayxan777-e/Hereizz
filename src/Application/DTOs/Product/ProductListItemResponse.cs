using Domain.Enums;

namespace Application.DTOs.Product;

public class ProductListItemResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public Marketplace Marketplace { get; set; }

    public decimal Price { get; set; }

    public Currency Currency { get; set; }
}
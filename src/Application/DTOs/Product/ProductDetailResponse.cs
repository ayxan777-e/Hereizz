using Domain.Enums;

namespace Application.DTOs.Product;

public class ProductDetailResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public Currency Currency { get; set; }

    public Marketplace Marketplace { get; set; }

    public Country OriginCountry { get; set; }

    public decimal WeightKg { get; set; }

    public string Category { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string AffiliateUrl { get; set; } = null!;
}
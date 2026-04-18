using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Product:BaseEntity<int>
{
    public string ExternalProductId { get; set; } = null!;
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

    public bool IsActive { get; set; }
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

}
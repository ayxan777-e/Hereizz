using Domain.Enums;

namespace Application.DTOs.Product;

public class ImportProductRequest
{
    public Marketplace Marketplace { get; set; }
    public string ExternalProductId { get; set; } = null!;
}
using Domain.Enums;

namespace Application.DTOs.ShippingOption;

public class ShippingOptionDetailResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Country OriginCountry { get; set; }
    public Country DestinationCountry { get; set; }
    public TransportType TransportType { get; set; }
    public int EstimatedMinDays { get; set; }
    public int EstimatedMaxDays { get; set; }
    public decimal PricePerKg { get; set; }
    public decimal FixedFee { get; set; }
    public bool IsActive { get; set; }
}
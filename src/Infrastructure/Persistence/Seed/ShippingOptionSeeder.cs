using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed;

public static class ShippingOptionSeeder
{
    public static async Task SeedAsync(HereizzzDbContext context)
    {
        Console.WriteLine("ShippingOptionSeeder started");

        var hasAny = await context.ShippingOptions.AnyAsync();
        Console.WriteLine($"ShippingOptions.AnyAsync(): {hasAny}");

        if (hasAny)
            return;

        var options = new List<ShippingOption>
    {
        new ShippingOption
        {
            Name = "USA Air Cargo",
            OriginCountry = Country.USA,
            DestinationCountry = Country.Azerbaijan,
            TransportType = TransportType.Air,
            EstimatedMinDays = 5,
            EstimatedMaxDays = 8,
            PricePerKg = 12m,
            FixedFee = 6m,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new ShippingOption
        {
            Name = "USA Sea Cargo",
            OriginCountry = Country.USA,
            DestinationCountry = Country.Azerbaijan,
            TransportType = TransportType.Sea,
            EstimatedMinDays = 20,
            EstimatedMaxDays = 30,
            PricePerKg = 3m,
            FixedFee = 12m,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new ShippingOption
        {
            Name = "Turkey Truck Delivery",
            OriginCountry = Country.Turkey,
            DestinationCountry = Country.Azerbaijan,
            TransportType = TransportType.Truck,
            EstimatedMinDays = 3,
            EstimatedMaxDays = 5,
            PricePerKg = 6m,
            FixedFee = 3m,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        }
    };

        await context.ShippingOptions.AddRangeAsync(options);
        await context.SaveChangesAsync();

        Console.WriteLine("ShippingOptionSeeder completed");
    }
}
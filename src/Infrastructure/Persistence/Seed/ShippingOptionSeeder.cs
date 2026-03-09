using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed;

public static class ShippingOptionSeeder
{
    public static async Task SeedAsync(HereizzzDbContext context)
    {
        if (await context.ShippingOptions.AnyAsync())
            return;

        var options = new List<ShippingOption>
        {
            new ShippingOption
            {
                Name = "USA Air Cargo",
                OriginCountry = (Country) 2,
                DestinationCountry = (Country)1,
                TransportType = (TransportType)1,
                EstimatedMinDays = 5,
                EstimatedMaxDays = 8,
                PricePerKg = 10,
                FixedFee = 5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            new ShippingOption
            {
                Name = "Turkey Truck Delivery",
                OriginCountry = (Country) 3,
                DestinationCountry = (Country) 1,
                TransportType = (TransportType) 3,
                EstimatedMinDays = 3,
                EstimatedMaxDays = 5,
                PricePerKg = 6,
                FixedFee = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            new ShippingOption
            {
                Name = "USA Sea Cargo",
                OriginCountry = (Country) 2,
                DestinationCountry = (Country) 1,
                TransportType = (TransportType) 2,
                EstimatedMinDays = 25,
                EstimatedMaxDays = 35,
                PricePerKg = 4,
                FixedFee = 10,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.ShippingOptions.AddRangeAsync(options);
        await context.SaveChangesAsync();
    }
}
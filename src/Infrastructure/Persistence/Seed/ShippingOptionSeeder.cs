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

        var now = DateTime.UtcNow;

        var options = new List<ShippingOption>
        {
            // USA -> Azerbaijan
            new()
            {
              Name = "USA Express Air",
              OriginCountry = Country.USA,
              DestinationCountry = Country.Azerbaijan,
              TransportType = TransportType.Air,
              EstimatedMinDays = 3,
              EstimatedMaxDays = 5,
              PricePerKg = 45m,
              FixedFee = 35m,
              IsActive = true,
              CreatedAt = now
            },
new()
{
    Name = "USA Balanced Air",
    OriginCountry = Country.USA,
    DestinationCountry = Country.Azerbaijan,
    TransportType = TransportType.Air,
    EstimatedMinDays = 9,
    EstimatedMaxDays = 14,
    PricePerKg = 18m,
    FixedFee = 15m,
    IsActive = true,
    CreatedAt = now
},
new()
{
    Name = "USA Economy Sea",
    OriginCountry = Country.USA,
    DestinationCountry = Country.Azerbaijan,
    TransportType = TransportType.Sea,
    EstimatedMinDays = 35,
    EstimatedMaxDays = 55,
    PricePerKg = 2m,
    FixedFee = 3m,
    IsActive = true,
    CreatedAt = now
},

// Turkey -> Azerbaijan
new()
{
    Name = "Turkey Express Air",
    OriginCountry = Country.Turkey,
    DestinationCountry = Country.Azerbaijan,
    TransportType = TransportType.Air,
    EstimatedMinDays = 1,
    EstimatedMaxDays = 2,
    PricePerKg = 30m,
    FixedFee = 20m,
    IsActive = true,
    CreatedAt = now
},
new()
{
    Name = "Turkey Balanced Truck",
    OriginCountry = Country.Turkey,
    DestinationCountry = Country.Azerbaijan,
    TransportType = TransportType.Truck,
    EstimatedMinDays = 5,
    EstimatedMaxDays = 8,
    PricePerKg = 12m,
    FixedFee = 7m,
    IsActive = true,
    CreatedAt = now
},
new()
{
    Name = "Turkey Economy Truck",
    OriginCountry = Country.Turkey,
    DestinationCountry = Country.Azerbaijan,
    TransportType = TransportType.Truck,
    EstimatedMinDays = 12,
    EstimatedMaxDays = 20,
    PricePerKg = 2m,
    FixedFee = 1m,
    IsActive = true,
    CreatedAt = now
},

// Germany -> Azerbaijan
new()
{
    Name = "Germany Express Air",
    OriginCountry = Country.Germany,
    DestinationCountry = Country.Azerbaijan,
    TransportType = TransportType.Air,
    EstimatedMinDays = 3,
    EstimatedMaxDays = 6,
    PricePerKg = 40m,
    FixedFee = 30m,
    IsActive = true,
    CreatedAt = now
},
new()
{
    Name = "Germany Balanced Air",
    OriginCountry = Country.Germany,
    DestinationCountry = Country.Azerbaijan,
    TransportType = TransportType.Air,
    EstimatedMinDays = 8,
    EstimatedMaxDays = 13,
    PricePerKg = 16m,
    FixedFee = 12m,
    IsActive = true,
    CreatedAt = now
},
new()
{
    Name = "Germany Economy Sea",
    OriginCountry = Country.Germany,
    DestinationCountry = Country.Azerbaijan,
    TransportType = TransportType.Sea,
    EstimatedMinDays = 30,
    EstimatedMaxDays = 50,
    PricePerKg = 3m,
    FixedFee = 3m,
    IsActive = true,
    CreatedAt = now
}
        };

        await context.ShippingOptions.AddRangeAsync(options);
        await context.SaveChangesAsync();

        Console.WriteLine("ShippingOptionSeeder completed");
    }
}
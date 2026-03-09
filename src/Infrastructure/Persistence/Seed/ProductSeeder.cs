using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed;

public static class ProductSeeder
{
    public static async Task SeedAsync(HereizzzDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var products = new List<Product>
        {
            new Product
            {
                Id=1,
                Title = "Apple Watch Series 9",
                Description = "Latest Apple smartwatch",
                Price = 399,
                Currency = Currency.USD,
                Marketplace = Marketplace.Amazon,
                OriginCountry = Country.USA,
                WeightKg = 0.4m,
                Category = "Smart Watch",
                ImageUrl = "",
                AffiliateUrl = "https://amazon.com/apple-watch",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Title = "Nike Air Max 270",
                Description = "Popular Nike sneakers",
                Price = 150,
                Currency = Currency.USD,
                Marketplace = Marketplace.Ebay,
                OriginCountry = Country.USA,
                WeightKg = 1.1m,
                Category = "Shoes",
                ImageUrl = "",
                AffiliateUrl = "https://ebay.com/nike-air-max",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Title = "Casio G-Shock",
                Description = "Durable digital watch",
                Price = 120,
                Currency = Currency.USD,
                Marketplace = Marketplace.Trendyol,
                OriginCountry = Country.Turkey,
                WeightKg = 0.3m,
                Category = "Watch",
                ImageUrl = "",
                AffiliateUrl = "https://trendyol.com/casio-gshock",
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
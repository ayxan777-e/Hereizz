using Application.Abstracts.Services;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed;

public static class ProductSeeder
{
    public static async Task SeedAsync(
        HereizzzDbContext context,
        IProductProviderService provider)
    {
        var existingProducts = await context.Products.ToListAsync();
        if (await context.Products.AnyAsync())
            return;

        var externalIds = new[]
        {
            "iphone15",
            "samsung-s24",
            "sony-headphones",
            "ps5",
            "macbook",
            "nike-airforce",
            "robot-vacuum",
            "kindle"
        };

        var products = new List<Product>();

        foreach (var id in externalIds)
        {
            var data = await provider.GetProductAsync(Marketplace.Amazon, id);

            products.Add(new Product
            {
                ExternalProductId = id,
                Title = data.Title,
                Description = data.Description,
                Price = data.Price,
                Currency = data.Currency,
                Marketplace = Marketplace.Amazon,
                OriginCountry = data.OriginCountry,
                WeightKg = data.WeightKg,
                Category = data.Category,
                ImageUrl = data.ImageUrl,
                AffiliateUrl = data.AffiliateUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
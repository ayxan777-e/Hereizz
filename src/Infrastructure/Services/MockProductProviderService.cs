using Application.Abstracts.Services;
using Application.DTOs.Product;
using Domain.Enums;

namespace Infrastructure.Services;

public class MockProductProviderService : IProductProviderService
{
    public Task<ProductProviderDataResponse> GetProductAsync(
        Marketplace marketplace,
        string externalProductId,
        CancellationToken ct = default)
    {
        var products = new Dictionary<string, ProductProviderDataResponse>
        {
            ["iphone15"] = new()
            {
                Title = "Apple iPhone 15 Pro Max 256GB",
                Description = "Latest Apple flagship smartphone with A17 Pro chip.",
                Price = 1199m,
                Currency = Currency.USD,
                OriginCountry = Country.USA,
                WeightKg = 0.35m,
                Category = "Smartphones",
                ImageUrl = "https://m.media-amazon.com/images/I/81Os1SDWpcL._AC_SL1500_.jpg",
                AffiliateUrl = "https://www.amazon.com/"
            },

            ["samsung-s24"] = new()
            {
                Title = "Samsung Galaxy S24 Ultra",
                Description = "Flagship Android smartphone with premium camera.",
                Price = 1299m,
                Currency = Currency.USD,
                OriginCountry = Country.USA,
                WeightKg = 0.38m,
                Category = "Smartphones",
                ImageUrl = "https://m.media-amazon.com/images/I/71RVuBs3q9L._AC_SL1500_.jpg",
                AffiliateUrl = "https://www.amazon.com/"
            },

            ["sony-headphones"] = new()
            {
                Title = "Sony WH-1000XM5 Headphones",
                Description = "Industry leading noise cancelling headphones.",
                Price = 349m,
                Currency = Currency.USD,
                OriginCountry = Country.USA,
                WeightKg = 0.45m,
                Category = "Audio",
                ImageUrl = "https://m.media-amazon.com/images/I/61vJtKbAssL._AC_SL1500_.jpg",
                AffiliateUrl = "https://www.amazon.com/"
            },

            ["ps5"] = new()
            {
                Title = "PlayStation 5 Console",
                Description = "Next-gen gaming console.",
                Price = 499m,
                Currency = Currency.USD,
                OriginCountry = Country.USA,
                WeightKg = 3.2m,
                Category = "Gaming",
                ImageUrl = "https://m.media-amazon.com/images/I/51fM0CKG+HL._SL1500_.jpg",
                AffiliateUrl = "https://www.amazon.com/"
            },

            ["macbook"] = new()
            {
                Title = "MacBook Air M2",
                Description = "Lightweight Apple laptop with M2 chip.",
                Price = 999m,
                Currency = Currency.USD,
                OriginCountry = Country.USA,
                WeightKg = 1.24m,
                Category = "Laptops",
                ImageUrl = "https://m.media-amazon.com/images/I/71f5Eu5lJSL._AC_SL1500_.jpg",
                AffiliateUrl = "https://www.amazon.com/"
            },

            ["nike-airforce"] = new()
            {
                Title = "Nike Air Force 1",
                Description = "Classic sneakers.",
                Price = 109m,
                Currency = Currency.USD,
                OriginCountry = Country.Turkey,
                WeightKg = 0.9m,
                Category = "Fashion",
                ImageUrl = "https://cdn.dsmcdn.com/ty1000/product/media/images/prod/SPM/PIM/20230920/12/b0ad8c9d-c47e-3f53-80a1-bd63d07d733e/1_org_zoom.jpg",
                AffiliateUrl = "https://www.trendyol.com/"
            },

            ["robot-vacuum"] = new()
            {
                Title = "Xiaomi Robot Vacuum",
                Description = "Smart vacuum cleaner.",
                Price = 279m,
                Currency = Currency.USD,
                OriginCountry = Country.Turkey,
                WeightKg = 3.5m,
                Category = "Home Appliances",
                ImageUrl = "https://cdn.dsmcdn.com/ty600/product/media/images/20221013/10/193453382/586620198/1/1_org_zoom.jpg",
                AffiliateUrl = "https://www.trendyol.com/"
            },

            ["kindle"] = new()
            {
                Title = "Kindle Paperwhite",
                Description = "E-reader device.",
                Price = 149m,
                Currency = Currency.USD,
                OriginCountry = Country.Germany,
                WeightKg = 0.22m,
                Category = "Electronics",
                ImageUrl = "https://i.ebayimg.com/images/g/JdIAAOSwkVJk3vJ-/s-l1600.jpg",
                AffiliateUrl = "https://www.ebay.com/"
            }
        };

        if (products.TryGetValue(externalProductId, out var product))
            return Task.FromResult(product);

        return Task.FromResult(new ProductProviderDataResponse
        {
            Title = "Generic Marketplace Product",
            Description = "Imported from provider",
            Price = 199.99m,
            Currency = Currency.USD,
            OriginCountry = Country.USA,
            WeightKg = 1.2m,
            Category = "Electronics",
            ImageUrl = null,
            AffiliateUrl = "https://example.com"
        });
    }
}
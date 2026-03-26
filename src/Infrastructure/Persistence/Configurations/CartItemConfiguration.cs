using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ShippingOption)
             .WithMany(x => x.CartItems)
             .HasForeignKey(x => x.ShippingOptionId)
             .OnDelete(DeleteBehavior.Restrict);

        // 🔥 Cart ilə əlaqə
        builder.HasOne(x => x.Cart)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔥 Product ilə əlaqə
        builder.HasOne(x => x.Product)
            .WithMany(x => x.CartItems)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔥 Quantity
        builder.Property(x => x.Quantity)
            .IsRequired();

        // 🔥 Decimal precision (ÇOX VACİB)
        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.ShippingCost)
            .HasPrecision(18, 2);

        builder.Property(x => x.CustomsFee)
            .HasPrecision(18, 2);

        builder.Property(x => x.WarehouseFee)
            .HasPrecision(18, 2);

        builder.Property(x => x.LocalDeliveryFee)
            .HasPrecision(18, 2);

        builder.Property(x => x.FinalPrice)
            .HasPrecision(18, 2);

        // 🔥 String field-lər
        builder.Property(x => x.ShippingOptionName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.TransportType)
            .HasConversion<string>()
            .IsRequired();

        // 🔥 Delivery days
        builder.Property(x => x.EstimatedMinDays)
            .IsRequired();

        builder.Property(x => x.EstimatedMaxDays)
            .IsRequired();
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductPrice)
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

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ShippingOption)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.ShippingOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
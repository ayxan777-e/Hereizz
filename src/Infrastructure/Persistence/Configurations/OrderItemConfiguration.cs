using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ShippingOptionName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.TransportType)
            .IsRequired()
            .HasMaxLength(50);

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
    }
}
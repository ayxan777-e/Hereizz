using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ShippingOptionConfiguration : IEntityTypeConfiguration<ShippingOption>
{
    public void Configure(EntityTypeBuilder<ShippingOption> builder)
    {
        builder.ToTable("ShippingOptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.OriginCountry)
            .IsRequired();

        builder.Property(x => x.DestinationCountry)
            .IsRequired();

        builder.Property(x => x.TransportType)
            .IsRequired();

        builder.Property(x => x.EstimatedMinDays)
            .IsRequired();

        builder.Property(x => x.EstimatedMaxDays)
            .IsRequired();

        builder.Property(x => x.PricePerKg)
            .HasPrecision(18, 2);

        builder.Property(x => x.FixedFee)
            .HasPrecision(18, 2);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

      
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class CurrencyInfoConfiguration : IEntityTypeConfiguration<CurrencyInfo>
    {
        public void Configure(EntityTypeBuilder<CurrencyInfo> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.DestinationCurrencyName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.DestinationCurrencySymbol)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.DestinationCurrencyCode)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(c => c.UserCurrencyCode)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(c => c.ExchangeRate)
                .HasPrecision(18, 6);

            builder.Property(c => c.FetchedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(c => c.TravelPlanId)
                .IsUnique();

            builder.HasOne(c => c.TravelPlan)
                .WithOne(p => p.CurrencyInfo)
                .HasForeignKey<CurrencyInfo>(c => c.TravelPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

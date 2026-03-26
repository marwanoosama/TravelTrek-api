using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class WeatherInfoConfiguration : IEntityTypeConfiguration<WeatherInfo>
    {
        public void Configure(EntityTypeBuilder<WeatherInfo> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.CurrentTempCelsius)
                .HasPrecision(5, 1);

            builder.Property(w => w.ExpectedHighCelsius)
                .HasPrecision(5, 1);

            builder.Property(w => w.ExpectedLowCelsius)
                .HasPrecision(5, 1);

            builder.Property(w => w.SeasonalNote)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(w => w.WeatherCondition)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.FetchedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(w => w.TravelPlanId)
                .IsUnique();

            builder.HasOne(w => w.TravelPlan)
                .WithOne(p => p.WeatherInfo)
                .HasForeignKey<WeatherInfo>(w => w.TravelPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

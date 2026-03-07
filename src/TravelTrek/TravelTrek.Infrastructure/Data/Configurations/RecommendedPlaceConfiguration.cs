using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class RecommendedPlaceConfiguration : IEntityTypeConfiguration<RecommendedPlace>
    {
        public void Configure(EntityTypeBuilder<RecommendedPlace> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.Category)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasOne(p => p.TravelPlan)
                .WithMany(tp => tp.RecommendedPlaces)
                .HasForeignKey(p => p.TravelPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

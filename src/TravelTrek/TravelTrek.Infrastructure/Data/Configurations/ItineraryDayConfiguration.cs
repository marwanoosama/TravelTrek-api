using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class ItineraryDayConfiguration : IEntityTypeConfiguration<ItineraryDay>
    {
        public void Configure(EntityTypeBuilder<ItineraryDay> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(d => d.TravelPlan)
                .WithMany(p => p.ItineraryDays)
                .HasForeignKey(d => d.TravelPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

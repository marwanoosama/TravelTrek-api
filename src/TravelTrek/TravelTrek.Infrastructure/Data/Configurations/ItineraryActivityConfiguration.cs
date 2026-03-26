using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class ItineraryActivityConfiguration : IEntityTypeConfiguration<ItineraryActivity>
    {
        public void Configure(EntityTypeBuilder<ItineraryActivity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(a => a.Location)
                .IsRequired()
                .HasMaxLength(300);

            // ItineraryDay can have many Activities
            builder.HasOne(a => a.ItineraryDay)
                .WithMany(d => d.Activities)
                .HasForeignKey(a => a.ItineraryDayId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

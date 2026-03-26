using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class TravelPlanConfiguration : IEntityTypeConfiguration<TravelPlan>
    {
        public void Configure(EntityTypeBuilder<TravelPlan> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.OriginalPrompt)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.Destination)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.TripType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(p => p.Budget)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(p => p.Preferences)
                .HasMaxLength(500);

            builder.Property(p => p.Language)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(p => p.ShareToken)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(p => p.ShareToken)
                .IsUnique();

            builder.HasIndex(p => p.UserId);

            builder.Property(p => p.GeneratedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(p => p.User)
                .WithMany(u => u.TravelPlans)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class PlanRatingConfiguration : IEntityTypeConfiguration<PlanRating>
    {
        public void Configure(EntityTypeBuilder<PlanRating> builder)
        {
            builder.HasKey(r => r.Id);

            // unique
            builder.HasIndex(r => new { r.UserId, r.TravelPlanId })
                .IsUnique();

            builder.Property(r => r.Stars)
                .IsRequired();

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_PlanRating_Stars", "[Stars] BETWEEN 1 AND 5"));

            builder.Property(r => r.Comment)
                .HasMaxLength(500);

            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(r => r.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(r => r.TravelPlan)
                .WithMany(p => p.PlanRatings)
                .HasForeignKey(r => r.TravelPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.User)
                .WithMany(u => u.PlanRatings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent multiple cascade paths
        }
    }
}

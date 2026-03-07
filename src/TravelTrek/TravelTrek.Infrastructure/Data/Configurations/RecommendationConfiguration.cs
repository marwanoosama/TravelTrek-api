using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class RecommendationConfiguration : IEntityTypeConfiguration<Recommendation>
    {
        public void Configure(EntityTypeBuilder<Recommendation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasOne(r => r.TravelPlan)
                .WithMany(p => p.Recommendations)
                .HasForeignKey(r => r.TravelPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

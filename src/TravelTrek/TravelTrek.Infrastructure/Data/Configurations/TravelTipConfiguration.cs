using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class TravelTipConfiguration : IEntityTypeConfiguration<TravelTip>
    {
        public void Configure(EntityTypeBuilder<TravelTip> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasOne(t => t.TravelPlan)
                .WithMany(p => p.TravelTips)
                .HasForeignKey(t => t.TravelPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

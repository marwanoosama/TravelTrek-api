using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PreferredLanguage)
                .IsRequired()
                .HasMaxLength(5)
                .HasDefaultValue("en");

            builder.Property(u => u.Country)
                .HasMaxLength(100);

            builder.Property(u => u.ProfilePictureUrl)
                .HasMaxLength(500);

            builder.Property(u => u.GoogleId)
                .HasMaxLength(200);

            // to make null not treated as unique
            builder.HasIndex(u => u.GoogleId)
                .IsUnique()
                .HasFilter("[GoogleId] IS NOT NULL");

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}

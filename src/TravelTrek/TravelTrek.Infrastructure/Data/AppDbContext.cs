using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<TravelPlan> TravelPlans => Set<TravelPlan>();
        public DbSet<ItineraryDay> ItineraryDays => Set<ItineraryDay>();
        public DbSet<ItineraryActivity> ItineraryActivities => Set<ItineraryActivity>();
        public DbSet<RecommendedPlace> RecommendedPlaces => Set<RecommendedPlace>();
        public DbSet<TravelTip> TravelTips => Set<TravelTip>();
        public DbSet<Recommendation> Recommendations => Set<Recommendation>();
        public DbSet<PlanRating> PlanRatings => Set<PlanRating>();
        public DbSet<CurrencyInfo> CurrencyInfos => Set<CurrencyInfo>();
        public DbSet<WeatherInfo> WeatherInfos => Set<WeatherInfo>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified))
            {
                switch (entry.Entity)
                {
                    case User user:
                        user.UpdatedAt = now;
                        break;
                    case TravelPlan plan:
                        plan.UpdatedAt = now;
                        break;
                    case PlanRating rating:
                        rating.UpdatedAt = now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}

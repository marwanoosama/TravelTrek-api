using TravelTrek.Domain.Enums;

namespace TravelTrek.Domain.Entities
{
    public class TravelPlan
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string OriginalPrompt { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public TripType TripType { get; set; }
        public int DurationDays { get; set; }
        public BudgetLevel Budget { get; set; }
        public string? Preferences { get; set; }
        public string Language { get; set; } = "en";
        public string ShareToken { get; set; } = Guid.NewGuid().ToString("N");
        public bool IsPublic { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<ItineraryDay> ItineraryDays { get; set; } = new List<ItineraryDay>();
        public ICollection<RecommendedPlace> RecommendedPlaces { get; set; } = new List<RecommendedPlace>();
        public ICollection<TravelTip> TravelTips { get; set; } = new List<TravelTip>();
        public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
        public ICollection<PlanRating> PlanRatings { get; set; } = new List<PlanRating>();
        public CurrencyInfo? CurrencyInfo { get; set; }
        public WeatherInfo? WeatherInfo { get; set; }
    }
}

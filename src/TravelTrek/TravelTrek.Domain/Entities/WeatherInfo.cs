namespace TravelTrek.Domain.Entities
{
    public class WeatherInfo
    {
        public Guid Id { get; set; }
        public Guid TravelPlanId { get; set; }
        public decimal CurrentTempCelsius { get; set; }
        public decimal ExpectedHighCelsius { get; set; }
        public decimal ExpectedLowCelsius { get; set; }
        public string SeasonalNote { get; set; } = string.Empty;
        public string WeatherCondition { get; set; } = string.Empty;
        public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public TravelPlan TravelPlan { get; set; } = null!;
    }
}

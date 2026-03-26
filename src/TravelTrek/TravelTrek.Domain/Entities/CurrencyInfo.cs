namespace TravelTrek.Domain.Entities
{
    public class CurrencyInfo
    {
        public Guid Id { get; set; }
        public Guid TravelPlanId { get; set; }
        public string DestinationCurrencyName { get; set; } = string.Empty;
        public string DestinationCurrencySymbol { get; set; } = string.Empty;
        public string DestinationCurrencyCode { get; set; } = string.Empty;
        public string UserCurrencyCode { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public TravelPlan TravelPlan { get; set; } = null!;
    }
}

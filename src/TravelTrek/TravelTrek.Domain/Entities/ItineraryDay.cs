namespace TravelTrek.Domain.Entities
{
    public class ItineraryDay
    {
        public Guid Id { get; set; }
        public Guid TravelPlanId { get; set; }
        public int DayNumber { get; set; }
        public string Title { get; set; } = string.Empty;

        // Navigation properties
        public TravelPlan TravelPlan { get; set; } = null!;
        public ICollection<ItineraryActivity> Activities { get; set; } = new List<ItineraryActivity>();
    }
}

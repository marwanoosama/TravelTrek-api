namespace TravelTrek.Domain.Entities
{
    public class ItineraryActivity
    {
        public Guid Id { get; set; }
        public Guid ItineraryDayId { get; set; }
        public TimeOnly StartTime { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        // Navigation property
        public ItineraryDay ItineraryDay { get; set; } = null!;
    }
}

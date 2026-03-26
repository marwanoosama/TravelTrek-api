namespace TravelTrek.Domain.Entities
{
    public class Recommendation
    {
        public Guid Id { get; set; }
        public Guid TravelPlanId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        // Navigation property
        public TravelPlan TravelPlan { get; set; } = null!;
    }
}

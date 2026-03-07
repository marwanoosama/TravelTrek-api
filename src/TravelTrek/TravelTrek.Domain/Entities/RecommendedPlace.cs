using TravelTrek.Domain.Enums;

namespace TravelTrek.Domain.Entities
{
    public class RecommendedPlace
    {
        public Guid Id { get; set; }
        public Guid TravelPlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PlaceCategory Category { get; set; }
        public int SortOrder { get; set; }

        // Navigation property
        public TravelPlan TravelPlan { get; set; } = null!;
    }
}

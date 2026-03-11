namespace TravelTrek.Domain.Entities
{
    public class PlanRating
    {
        public Guid Id { get; set; }
        public Guid TravelPlanId { get; set; }
        public Guid UserId { get; set; }
        public int Stars { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public TravelPlan TravelPlan { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}

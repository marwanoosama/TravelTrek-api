using Microsoft.AspNetCore.Identity;

namespace TravelTrek.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        // Inherited from IdentityUser<Guid>:
        // Guid Id
        // string? Email
        // string? UserName (set = Email)
        // string? PasswordHash
        // string? PhoneNumber
        // bool EmailConfirmed
        // bool LockoutEnabled
        // DateTimeOffset? LockoutEnd
        // int AccessFailedCount
        // string? SecurityStamp
        // string? ConcurrencyStamp

        public string FullName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public string PreferredLanguage { get; set; } = "en";
        public string? Country { get; set; } 
        public string? GoogleId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<TravelPlan> TravelPlans { get; set; } = new List<TravelPlan>();
        public ICollection<PlanRating> PlanRatings { get; set; } = new List<PlanRating>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


       
    }
}

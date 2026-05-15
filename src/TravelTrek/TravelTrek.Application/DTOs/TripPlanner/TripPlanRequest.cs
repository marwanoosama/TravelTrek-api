using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.TripPlanner;

public class TripPlanRequest
{
    [Required(ErrorMessage = "A trip description prompt is required.")]
    [MinLength(10, ErrorMessage = "Prompt must be at least 10 characters.")]
    public string Prompt { get; set; } = string.Empty;
}

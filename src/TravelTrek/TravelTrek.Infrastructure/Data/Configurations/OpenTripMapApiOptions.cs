using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Infrastructure.Data.Configurations;

public class OpenTripMapApiOptions
{
    public const string SectionName = "OpenTripMap";
    
    [Required(ErrorMessage = "OpenTripMap BaseUrl is required.")]
    public string BaseUrl { get; set; } = default!;
    
    [Required(ErrorMessage = "OpenTripMap ApiKey is required.")]
    public string ApiKey { get; set; } = default!;
}
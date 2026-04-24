using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Infrastructure.Data.Configurations;

public class OpenWeatherApiOptions
{
    public const string SectionName = "OpenWeather";

    [Required(ErrorMessage = "OpenWeather BaseUrl is required.")]
    public string BaseUrl { get; set; } = default!;

    [Required(ErrorMessage = "OpenWeather ApiKey is required.")]
    public string ApiKey { get; set; } = default!;
}
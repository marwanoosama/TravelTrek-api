namespace TravelTrek.Infrastructure.Data.Configurations;

public class OpenWeatherApiOptions
{
    public string BaseUrl { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
}
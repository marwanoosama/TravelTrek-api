using System.Text.Json.Serialization;

namespace TravelTrek.Application.DTOs.Weather;

public record WeatherGeocodeResponse(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("lat")] double Lat,
    [property: JsonPropertyName("lon")] double Lon,
    [property: JsonPropertyName("country")] string? Country
);

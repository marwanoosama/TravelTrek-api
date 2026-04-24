using System.Text.Json.Serialization;

namespace TravelTrek.Application.DTOs.Weather;

public record WeatherResponse(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("main")] WeatherMain Main,
    [property: JsonPropertyName("weather")] IReadOnlyList<WeatherCondition> Weather,
    [property: JsonPropertyName("wind")] WeatherWind Wind
);

public record WeatherMain(
    [property: JsonPropertyName("temp")] double Temp,
    [property: JsonPropertyName("feels_like")] double FeelsLike,
    [property: JsonPropertyName("humidity")] int Humidity
);

public record WeatherCondition(
    [property: JsonPropertyName("main")] string Main,
    [property: JsonPropertyName("description")] string Description
);

public record WeatherWind(
    [property: JsonPropertyName("speed")] double Speed
);
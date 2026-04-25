using System.Text.Json.Serialization;

namespace TravelTrek.Application.DTOs.Weather;

public record ForecastResponse(
    [property: JsonPropertyName("cnt")] int Count,
    [property: JsonPropertyName("list")] IReadOnlyList<ForecastItem> Items,
    [property: JsonPropertyName("city")] ForecastCity City
);

public record ForecastItem(
    [property: JsonPropertyName("dt")] long Dt,
    [property: JsonPropertyName("main")] WeatherMain Main,
    [property: JsonPropertyName("weather")] IReadOnlyList<WeatherCondition> Weather,
    [property: JsonPropertyName("wind")] WeatherWind Wind,
    [property: JsonPropertyName("dt_txt")] string DtTxt
);

public record ForecastCity(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("country")] string Country
);

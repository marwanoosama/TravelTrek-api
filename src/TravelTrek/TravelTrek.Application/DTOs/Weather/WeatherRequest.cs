using TravelTrek.Application.Interfaces;
using TravelTrek.Infrastructure.Services.Weather;

namespace TravelTrek.Application.DTOs.Weather;

public record WeatherRequest(
    double Latitude,
    double Longitude
);

builder.Services.AddHttpClient<IOpenWeatherService, OpenWeatherService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["OpenWeather:BaseUrl"]!);
client.Timeout = TimeSpan.FromSeconds(15);
})
.AddStandardResilienceHandler();

builder.Services.Configure<OpenWeatherApiOptions>(builder.Configuration.GetSection("OpenWeather")); namespace TravelTrek.Application.DTOs.Weather;

public record WeatherRequest(
    double Latitude,
    double Longitude
);
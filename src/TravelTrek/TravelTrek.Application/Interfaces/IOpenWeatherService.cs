using TravelTrek.Application.DTOs.Weather;
using TravelTrek.Domain.Common;

namespace TravelTrek.Application.Interfaces;

public interface IOpenWeatherService
{
    Task<Result<WeatherGeocodeResponse>> GeocodeByNameAsync(string cityName, CancellationToken ct = default);
    Task<Result<WeatherResponse>> GetCurrentWeatherAsync(WeatherRequest request, CancellationToken ct = default);
    Task<Result<ForecastResponse>> GetForecastAsync(WeatherRequest request, CancellationToken ct = default);
}
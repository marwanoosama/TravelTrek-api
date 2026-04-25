using Microsoft.AspNetCore.Mvc;
using TravelTrek.Application.DTOs.Weather;
using TravelTrek.Application.Interfaces;

namespace TravelTrek.API.Controllers
{
    [Route("api/weather")]
    public class WeatherController : ApiBaseController
    {
        private readonly IOpenWeatherService _weatherService;
        private readonly IOpenTripMapService _openTripMapService;

        public WeatherController(
            IOpenWeatherService weatherService,
            IOpenTripMapService openTripMapService)
        {
            _weatherService = weatherService;
            _openTripMapService = openTripMapService;
        }

        /// <summary>
        /// Get weather by city name — uses OpenTripMap geocode to resolve lat/lon automatically.
        /// </summary>
        [HttpGet("city")]
        public async Task<IActionResult> GetWeatherByCity(
            [FromQuery] string name,
            CancellationToken ct)
        {
            var geocodeResult = await _openTripMapService.GetCityGeocode(name, ct);

            if (geocodeResult.IsFailure)
                return ToActionResult(geocodeResult);

            var geocode = geocodeResult.Value;
            var weatherResult = await _weatherService.GetCurrentWeatherAsync(
                new WeatherRequest(geocode.Lat, geocode.Lon), ct);

            return ToActionResult(weatherResult);
        }

        /// <summary>
        /// Get weather by coordinates directly.
        /// </summary>
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWeather(
            [FromQuery] double lat,
            [FromQuery] double lon,
            CancellationToken ct)
        {
            var result = await _weatherService.GetCurrentWeatherAsync(
                new WeatherRequest(lat, lon), ct);

            return ToActionResult(result);
        }

        /// <summary>
        /// Get 5-day / 3-hour forecast by city name — uses OpenTripMap geocode to resolve lat/lon automatically.
        /// </summary>
        [HttpGet("forecast/city")]
        public async Task<IActionResult> GetForecastByCity(
            [FromQuery] string name,
            CancellationToken ct)
        {
            var geocodeResult = await _openTripMapService.GetCityGeocode(name, ct);

            if (geocodeResult.IsFailure)
                return ToActionResult(geocodeResult);

            var geocode = geocodeResult.Value;
            var forecastResult = await _weatherService.GetForecastAsync(
                new WeatherRequest(geocode.Lat, geocode.Lon), ct);

            return ToActionResult(forecastResult);
        }

        /// <summary>
        /// Get 5-day / 3-hour forecast by coordinates directly.
        /// </summary>
        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecast(
            [FromQuery] double lat,
            [FromQuery] double lon,
            CancellationToken ct)
        {
            var result = await _weatherService.GetForecastAsync(
                new WeatherRequest(lat, lon), ct);

            return ToActionResult(result);
        }
    }
}
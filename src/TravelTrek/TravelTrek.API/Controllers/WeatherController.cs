using Microsoft.AspNetCore.Mvc;
using TravelTrek.Application.DTOs.Weather;
using TravelTrek.Application.Interfaces;

namespace TravelTrek.API.Controllers
{
    [Route("api/weather")]
    public class WeatherController : ApiBaseController
    {
        private readonly IOpenWeatherService _weatherService;

        public WeatherController(IOpenWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test([FromQuery] double lat, [FromQuery] double lon)
        {
            var result = await _weatherService.GetCurrentWeatherAsync(new WeatherRequest(lat, lon));
            return ToActionResult(result);
        }
    }
}
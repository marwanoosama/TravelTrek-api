using Microsoft.AspNetCore.Mvc;
using TravelTrek.Application.DTOs.Auth;
using TravelTrek.Application.DTOs.OpenTrip;
using TravelTrek.Application.Interfaces;

namespace TravelTrek.API.Controllers;

[Route("api/OpenTrip")]
public class OpenTripController : ApiBaseController
{
    private readonly IOpenTripMapService _openTripMapService;

    public OpenTripController(IOpenTripMapService openTripMapService)
    {
        _openTripMapService = openTripMapService;
    }

    [HttpGet("city-geocode")]
    public async Task<IActionResult> GetCityGeocode([FromQuery] string name)
    {
        var result = await _openTripMapService.GetCityGeocode(name);
        return ToActionResult(result);
    }

    [HttpGet("get-place-by-radius")]
    public async Task<IActionResult> GetPlacesByRadius([FromQuery] RadiusRequest request)
    {
        var result = await _openTripMapService.GetPlacesByRadiusAsync(request);
        return ToActionResult(result);
    }
}
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TravelTrek.Application.DTOs.Auth;
using TravelTrek.Application.DTOs.OpenTrip;
using TravelTrek.Application.Interfaces;
using TravelTrek.Domain.Common;
using TravelTrek.Infrastructure.Data.Configurations;

namespace TravelTrek.Infrastructure.Services.OpenTrip;

public class OpenTripMapService : IOpenTripMapService
{
    private readonly HttpClient _httpClient;
    private readonly OpenTripMapApiOptions _options;
    private readonly ILogger<OpenTripMapService> _logger;

    public OpenTripMapService(
        HttpClient httpClient,
        IOptions<OpenTripMapApiOptions> options,
        ILogger<OpenTripMapService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }
    
    public async Task<Result<GeocodeResponse>> GetCityGeocode(string name, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync(
            $"geoname" +
            $"?name={Uri.EscapeDataString(name)}" +
            $"&apikey={Uri.EscapeDataString(_options.ApiKey)}",
            ct);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Failure<GeocodeResponse>(Error.Unauthorized(
                "OpenTripMap.Unauthorized",
                "Invalid or missing OpenTripMap API key."));
        }

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            return Result.Failure<GeocodeResponse>(Error.TooManyRequests(
                "OpenTripMap.RateLimited",
                "Rate limit exceeded."));
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result.Failure<GeocodeResponse>(Error.Validation(
                "OpenTripMap.Validation",
                "Invalid request parameters sent to OpenTripMap."));
        }

        if ((int)response.StatusCode >= 500)
        {
            return Result.Failure<GeocodeResponse>(Error.External(
                "OpenTripMap.ServerError",
                $"OpenTripMap server error: {(int)response.StatusCode}."));
        }

        if (!response.IsSuccessStatusCode)
        {
            return Result.Failure<GeocodeResponse>(Error.Internal(
                "OpenTripMap.Error",
                $"Unexpected response: {(int)response.StatusCode}."));
        }

        try
        {
            var value = await response.Content.ReadFromJsonAsync<GeocodeResponse>(ct);

            if (value is null)
            {
                return Result.Failure<GeocodeResponse>(Error.Internal(
                    "OpenTripMap.EmptyResponse",
                    "Empty response from OpenTripMap."));
            }

            return Result.Success(value);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Failed to parse OpenTripMap geocode response.");
            return Result.Failure<GeocodeResponse>(Error.Internal(
                "OpenTripMap.ParseError",
                "Failed to parse OpenTripMap geocode response."));
        }
    }

    public async Task<Result<List<RadiusResponse>>> GetPlacesByRadiusAsync(RadiusRequest request, CancellationToken ct = default)
    {
        var radius = Uri.EscapeDataString(request.Radius.ToString(CultureInfo.InvariantCulture));
        var lon = Uri.EscapeDataString(request.Lon.ToString(CultureInfo.InvariantCulture));
        var lat = Uri.EscapeDataString(request.Lat.ToString(CultureInfo.InvariantCulture));
        var kinds = Uri.EscapeDataString(request.Kinds);
        var rate = Uri.EscapeDataString(request.Rate);
        var apiKey = Uri.EscapeDataString(_options.ApiKey);
        
        var response = await _httpClient.GetAsync(
            $"radius" +
            $"?radius={radius}" +
            $"&lon={lon}" +
            $"&lat={lat}" +
            $"&kinds={kinds}" +
            $"&rate={rate}" +
            $"&format=json" +
            $"&apikey={apiKey}",
            ct);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Failure<List<RadiusResponse>>(Error.Unauthorized(
                "OpenTripMap.Unauthorized",
                "Invalid or missing OpenTripMap API key."));
        }
        
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            return Result.Failure<List<RadiusResponse>>(Error.TooManyRequests(
                "OpenTripMap.RateLimited",
                "Rate limit exceeded."));
        }
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result.Failure<List<RadiusResponse>>(Error.Validation(
                "OpenTripMap.Validation",
                "Invalid request parameters sent to OpenTripMap."));
        }
        
        if ((int)response.StatusCode >= 500)
        {
            return Result.Failure<List<RadiusResponse>>(Error.External(
                "OpenTripMap.ServerError",
                $"OpenTripMap server error: {(int)response.StatusCode}."));
        }
        
        if (!response.IsSuccessStatusCode)
        {
            return Result.Failure<List<RadiusResponse>>(Error.Internal(
                "OpenTripMap.Error",
                $"Unexpected response: {(int)response.StatusCode}."));
        }

        try
        {
            var value = await response.Content.ReadFromJsonAsync<List<RadiusResponse>>(ct);
            
            if (value is null)
            {
                return Result.Failure<List<RadiusResponse>>(Error.Internal(
                    "OpenTripMap.EmptyResponse",
                    "Empty response from OpenTripMap."));
            }

            return Result.Success(value);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Failed to parse OpenTripMap radius response.");
            return Result.Failure<List<RadiusResponse>>(Error.Internal(
                "OpenTripMap.ParseError",
                "Failed to parse OpenTripMap radius response."));
        }
    }

}
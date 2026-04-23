using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
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

    public OpenTripMapService(HttpClient httpClient, IOptions<OpenTripMapApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }
    
    public async Task<Result<GeocodeResponse>> GetCityGeocode(string name, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync(
            $"geoname" +
            $"?name={new UriBuilder(name)}" +
            $"&apikey={_options.ApiKey}",
            ct);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Failure<GeocodeResponse>(Error.NotFound("OpenTripMap.Unauthorized",
                "Invalid or missing OpenTripMap Api key."));
        }

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            return Result.Failure<GeocodeResponse>(Error.TooManyRequests("OpenTripMap.RateLimited",
                "Rate limit exceeded."));
        }
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result.Failure<GeocodeResponse>(Error.Validation("OpenTripMap.Validation",
                "Invalid request parameters sent to OpenTripMap."));
        }
        if (!response.IsSuccessStatusCode)
        {
            return Result.Failure<GeocodeResponse>(Error.Internal("OpenTripMap.Error",
                $"Unexpected response: {(int)response.StatusCode}"));
        }

        var parseResult = await ParseGeocodeResponseAsync(response, ct);
        return parseResult;
    }

    public async Task<Result<List<RadiusResponse>>> GetPlacesByRadiusAsync(RadiusRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync(
            $"radius" +
            $"?radius={request.Radius}" +
            $"&lon={request.Lon}" +
            $"&lat={request.Lat}" +
            $"&kinds={request.Kinds}" +
            $"&rate={request.Rate}" +
            $"&format=json" +
            $"&apikey={_options.ApiKey}",
            ct);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Failure<List<RadiusResponse>>(Error.NotFound("OpenTripMap.Unauthorized",
                "Invalid or missing OpenTripMap Api key."));
        }
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            return Result.Failure<List<RadiusResponse>>(Error.TooManyRequests("OpenTripMap.RateLimited",
                "Rate limit exceeded."));
        }
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return Result.Failure<List<RadiusResponse>>(Error.Validation("OpenTripMap.Validation",
                "Invalid request parameters sent to OpenTripMap."));
        }
        
        if (!response.IsSuccessStatusCode)
        {
            return Result.Failure<List<RadiusResponse>>(Error.Internal("OpenTripMap.Error",
                $"Unexpected response: {(int)response.StatusCode}"));
        }

        var parseResult = await ParsePlacesByRadiusResponseAsync(response, ct);
        return parseResult;
    }


    #region Helpers

    private async Task<Result<List<RadiusResponse>>> ParsePlacesByRadiusResponseAsync(HttpResponseMessage response, CancellationToken ct)
    {
        List<RadiusResponse>? value;
    
        try
        {
            value = await response.Content.ReadFromJsonAsync<List<RadiusResponse>>(ct);
        }
        catch(JsonException)
        {
            return Result.Failure<List<RadiusResponse>>(
                Error.Internal("OpenTripMap.ParseError", "Failed to parse places by radius response."));
        }
    
        if (value is null)
        {
            return Result.Failure<List<RadiusResponse>>(
                Error.Internal("OpenTripMap.EmptyResponse", "Empty response from places by radius endpoint."));
        }

        return Result.Success(value);
    }
    private async Task<Result<GeocodeResponse>> ParseGeocodeResponseAsync(HttpResponseMessage response, CancellationToken ct)
    {
        GeocodeResponse? value;

        try
        {
            value = await response.Content.ReadFromJsonAsync<GeocodeResponse>(ct);
        }
        catch (JsonException)
        {
            return Result.Failure<GeocodeResponse>(
                Error.Internal("OpenTripMap.ParseError", "Failed to parse geocode response."));
        }

        if (value is null)
        {
            return Result.Failure<GeocodeResponse>(
                Error.Internal("OpenTripMap.EmptyResponse", "Empty response from geocode endpoint."));
        }

        if (value.Status == "NOT_FOUND")
        {
            return Result.Failure<GeocodeResponse>(
                Error.NotFound("OpenTripMap.NotFound", $"City '{value.Name}' could not be found."));
        }

        return Result.Success(value);
    }

    #endregion
}
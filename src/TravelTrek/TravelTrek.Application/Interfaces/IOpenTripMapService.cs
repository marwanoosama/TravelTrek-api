using TravelTrek.Application.DTOs.Auth;
using TravelTrek.Application.DTOs.OpenTrip;
using TravelTrek.Domain.Common;

namespace TravelTrek.Application.Interfaces;

public interface IOpenTripMapService
{
    Task<Result<GeocodeResponse>> GetCityGeocode(string name, CancellationToken ct = default);
    Task<Result<List<RadiusResponse>>> GetPlacesByRadiusAsync(RadiusRequest request, CancellationToken ct = default);
}
using System.Text.Json.Serialization;

namespace TravelTrek.Application.DTOs.Auth;

public record RadiusResponse(
    [property: JsonPropertyName("xid")] string Xid,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("dist")] double Dist,
    [property: JsonPropertyName("rate")] int Rate,
    [property: JsonPropertyName("wikidata")] string Wikidata,
    [property: JsonPropertyName("kinds")] string Kinds,
    [property: JsonPropertyName("point")] PointResponse Point
    
    
);

public record PointResponse(
    [property: JsonPropertyName("lon")] double Lon, 
    [property: JsonPropertyName("lat")] double Lat
);
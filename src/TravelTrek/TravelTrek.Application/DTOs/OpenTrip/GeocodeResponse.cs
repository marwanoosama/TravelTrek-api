namespace TravelTrek.Application.DTOs.Auth;

public class GeocodeResponse
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public int Population { get; set; }
    public string? Timezone { get; set; }
    public string Status { get; set; }
    public string? Error { get; set; }
}
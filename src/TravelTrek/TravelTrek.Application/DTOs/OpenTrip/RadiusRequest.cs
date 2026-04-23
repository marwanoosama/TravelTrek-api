using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.OpenTrip;

public class RadiusRequest
{
    [Required]
    public int Radius { get; set; }
    [Required]
    public double Lon { get; set; }
    [Required]
    public double Lat { get; set; }
    [Required]
    public string Kinds { get; set; }
    [Required]
    public string Rate { get; set; }
}
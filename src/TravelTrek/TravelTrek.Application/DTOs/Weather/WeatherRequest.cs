using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Weather;

public record WeatherRequest(   
    [property: Required]
    [property: Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
    double Latitude,

    [property: Required]
    [property: Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
    double Longitude
);
namespace TravelTrek.Application.DTOs.Weather;

public record WeatherRequest(
    double Latitude,
    double Longitude
);
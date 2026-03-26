using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Auth;

public record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required][MaxLength(100)] string Password
);

using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Auth;

public record RefreshTokenRequest(
    [Required] string AccessToken,
    [Required] string RefreshToken
);

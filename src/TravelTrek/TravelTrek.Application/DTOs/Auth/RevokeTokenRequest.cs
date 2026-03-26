using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Auth;

public record RevokeTokenRequest(
    [Required] string RefreshToken
);

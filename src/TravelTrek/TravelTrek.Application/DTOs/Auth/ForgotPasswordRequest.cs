using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Auth;

public record ForgotPasswordRequest(
    [Required][EmailAddress] string Email
);

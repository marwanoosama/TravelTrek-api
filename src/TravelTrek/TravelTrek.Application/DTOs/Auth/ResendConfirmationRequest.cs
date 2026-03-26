using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Auth;

public record ResendConfirmationRequest(
    [Required][EmailAddress] string Email
);

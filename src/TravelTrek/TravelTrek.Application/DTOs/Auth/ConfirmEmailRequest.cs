using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Auth;

public record ConfirmEmailRequest(
    [Required] Guid UserId,
    [Required] string Token
);

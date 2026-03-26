using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Auth;

public record ResetPasswordRequest(
    [Required][EmailAddress] string Email,
    [Required] string Token,
    [Required][MinLength(8)][MaxLength(100)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must have uppercase, lowercase, and digit.")]
    string NewPassword
);

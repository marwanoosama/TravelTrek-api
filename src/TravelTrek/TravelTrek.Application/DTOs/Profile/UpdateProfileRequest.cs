using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Profile;

public record UpdateProfileRequest(
    [Required][MaxLength(100)] string FullName,
    [MaxLength(100)] string? Country,
    [Required][MaxLength(5)] string PreferredLanguage
);

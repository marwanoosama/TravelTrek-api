namespace TravelTrek.Application.DTOs.Profile;

public record UserProfileResponse(
    Guid Id,
    string Email,
    string FullName,
    string? ProfilePictureUrl,
    string? Country,
    string PreferredLanguage,
    DateTime CreatedAt
);

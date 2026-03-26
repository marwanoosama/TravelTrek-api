using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TravelTrek.Application.DTOs.Profile;
using TravelTrek.Application.Interfaces;
using TravelTrek.Domain.Common;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(UserManager<User> userManager, ILogger<UserProfileService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<UserProfileResponse>> GetProfileAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                _logger.LogWarning("Get profile failed — user not found. UserId: {UserId}", userId);
                return Result.Failure<UserProfileResponse>(Error.NotFound("User.NotFound", $"User with ID '{userId}' was not found."));
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Get profile failed — account deactivated. UserId: {UserId}", userId);
                return Result.Failure<UserProfileResponse>(Error.Forbidden("User.Deactivated", "This account has been deactivated."));
            }

            return Result.Success(MapToResponse(user));
        }

        public async Task<Result<UserProfileResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                _logger.LogWarning("Update profile failed — user not found. UserId: {UserId}", userId);
                return Result.Failure<UserProfileResponse>(Error.NotFound("User.NotFound", $"User with ID '{userId}' was not found."));
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Update profile failed — account deactivated. UserId: {UserId}", userId);
                return Result.Failure<UserProfileResponse>(Error.Forbidden("User.Deactivated", "This account has been deactivated."));
            }

            user.FullName = request.FullName;
            user.Country = request.Country;
            user.PreferredLanguage = request.PreferredLanguage;
            user.UpdatedAt = DateTime.UtcNow;

            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                _logger.LogWarning("Update profile failed — identity errors. UserId: {UserId}, Errors: {@Errors}",
                    userId, identityResult.Errors.Select(e => e.Code));
                var errors = identityResult.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToArray();
                return ValidationResult<UserProfileResponse>.WithErrors(errors);
            }

            _logger.LogInformation("Profile updated successfully. UserId: {UserId}", userId);
            return Result.Success(MapToResponse(user));
        }

        private static UserProfileResponse MapToResponse(User user)
        {
            return new UserProfileResponse(
                user.Id,
                user.Email!,
                user.FullName,
                user.ProfilePictureUrl,
                user.Country,
                user.PreferredLanguage,
                user.CreatedAt
            );
        }
    }
}

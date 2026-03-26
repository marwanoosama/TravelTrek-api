using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TravelTrek.Application.DTOs.Profile;
using TravelTrek.Application.Interfaces;
using TravelTrek.Domain.Common;

namespace TravelTrek.API.Controllers
{
    [Route("api/profile")]
    [Authorize]
    public class UserProfileController : ApiBaseController
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
            {
                return ToActionResult(Result.Failure<UserProfileResponse>(
                    Error.Unauthorized("Auth.InvalidToken", "Invalid access token.")));
            }

            var result = await _userProfileService.GetProfileAsync(userId.Value);
            return ToActionResult(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = GetCurrentUserId();
            if (userId is null)
            {
                return ToActionResult(Result.Failure<UserProfileResponse>(
                    Error.Unauthorized("Auth.InvalidToken", "Invalid access token.")));
            }

            var result = await _userProfileService.UpdateProfileAsync(userId.Value, request);
            return ToActionResult(result);
        }

        private Guid? GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (Guid.TryParse(userIdStr, out var userId))
                return userId;

            return null;
        }
    }
}

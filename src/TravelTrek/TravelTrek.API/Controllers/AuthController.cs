using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TravelTrek.Application.DTOs.Auth;
using TravelTrek.Application.Interfaces;
using TravelTrek.Domain.Common;

namespace TravelTrek.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : ApiBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return ToActionResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return ToActionResult(result);
        }

        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] SignupWithGoogleRequest request)
        {
            var result = await _authService.SignupWithGoogleAsync(request);
            return ToActionResult(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            return ToActionResult(result);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                return ToActionResult(Result.Failure(Error.Unauthorized("Auth.InvalidToken", "Invalid access token.")));
            }

            var result = await _authService.RevokeTokenAsync(request.RefreshToken, userId);
            return ToActionResult(result);
        }
    }
}
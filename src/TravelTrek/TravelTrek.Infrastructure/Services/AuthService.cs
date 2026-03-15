using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TravelTrek.Application.DTOs.Auth;
using TravelTrek.Application.Interfaces;
using TravelTrek.Domain.Common;
using TravelTrek.Domain.Entities;
using TravelTrek.Domain.Interfaces;
using TravelTrek.Infrastructure.Auth;

namespace TravelTrek.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _cache;
        private readonly ILogger<AuthService> _logger;


        public AuthService(
            UserManager<User> userManager,
            ITokenService tokenService,
            IGenericRepository<RefreshToken> refreshTokenRepo,
            IUnitOfWork unitOfWork,
            IOptions<JwtSettings> jwtSettings,
            IUserRepository userRepository,
            IDistributedCache cache,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenRepo = refreshTokenRepo;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
            _cache = cache;
            _logger = logger;
        }
        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
        {
            var key = FailKey(request.Email);
            var record = await GetFailRecordAsync(key);

            if (record.Count >= 5 && DateTimeOffset.UtcNow < record.LockedUntil)
            {
                return Result.Failure<AuthResponse>(Error.Unauthorized("Auth.MaxAttempts", "Account temporarily locked. Try again in 15 minutes."));
            }
                
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Failure<AuthResponse>(Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password."));
            }

            if (!user.IsActive)
            {
                await IncrementFailAsync(key, record);
                return Result.Failure<AuthResponse>(Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password."));
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                await IncrementFailAsync(key, record);
                return Result.Failure<AuthResponse>(Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password."));
            }

            await _cache.RemoveAsync(key);
            return await GenerateAuthResponseAsync(user);
        }

        public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return Result.Failure<AuthResponse>(Error.Conflict("Auth.EmailTaken", "A user with this email already exists."));

            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName,
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToArray();
                
                return ValidationResult<AuthResponse>.WithErrors(errors);
            }

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<Result<AuthResponse>> SignupWithGoogleAsync(SignupWithGoogleRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

                var existingUser = await _userRepository.GetByGoogleIdAsync(payload.Subject)
                    ?? await _userManager.FindByEmailAsync(payload.Email);

                if (existingUser != null)
                {
                    if (!existingUser.IsActive)
                        return Result.Failure<AuthResponse>(Error.Forbidden("Auth.AccountDeactivated", "This account has been deactivated."));

                    if (existingUser.GoogleId == null)
                    {
                        existingUser.GoogleId = payload.Subject;
                        existingUser.ProfilePictureUrl ??= payload.Picture;
                        await _userManager.UpdateAsync(existingUser);
                    }

                    return await GenerateAuthResponseAsync(existingUser);
                }

                var user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    FullName = payload.Name,
                    EmailConfirmed = true,
                    ProfilePictureUrl = payload.Picture,
                    GoogleId = payload.Subject
                };

                var identityResult = await _userManager.CreateAsync(user);
                if (!identityResult.Succeeded)
                {
                    var errors = identityResult.Errors
                        .Select(e => Error.Validation(e.Code, e.Description))
                        .ToArray();
                    
                    return ValidationResult<AuthResponse>.WithErrors(errors);
                }

                return await GenerateAuthResponseAsync(user);
            }
            catch (InvalidJwtException)
            {
                return Result.Failure<AuthResponse>(Error.Unauthorized("Auth.InvalidGoogleToken", "Invalid or expired Google token."));
            }
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return Result.Failure<AuthResponse>(Error.Unauthorized("Auth.InvalidToken", "Invalid access token."));
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                              ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                              
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Result.Failure<AuthResponse>(Error.Unauthorized("Auth.InvalidToken", "Invalid access token."));
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Result.Failure<AuthResponse>(Error.NotFound("Auth.UserNotFound", "User not found."));
            }

            var storedTokens = await _refreshTokenRepo.FindAsync(r => r.UserId == userId && r.Token == request.RefreshToken);

            var storedToken = storedTokens.FirstOrDefault();
            if (storedToken == null || !storedToken.IsActive)
            {
                return Result.Failure<AuthResponse>(Error.Unauthorized("Auth.InvalidRefreshToken", "Invalid or expired refresh token."));
            }

            storedToken.RevokedAt = DateTime.UtcNow;

            var newRefreshToken = _tokenService.GenerateRefreshToken(userId);
            storedToken.ReplacedByToken = newRefreshToken.Token;

            _refreshTokenRepo.Update(storedToken);
            await _refreshTokenRepo.AddAsync(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(new AuthResponse
            {
                AccessToken = _tokenService.GenerateAccessToken(user),
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes)
            });
        }

        public async Task<Result> RevokeTokenAsync(string refreshToken, Guid userId)
        {
            var storedTokens = await _refreshTokenRepo.FindAsync(
                r => r.UserId == userId && r.Token == refreshToken);

            var storedToken = storedTokens.FirstOrDefault();
            if (storedToken == null || !storedToken.IsActive)
                return Result.Failure(Error.NotFound("Auth.TokenNotFound", "Refresh token not found or already revoked."));

            storedToken.RevokedAt = DateTime.UtcNow;
            _refreshTokenRepo.Update(storedToken);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        #region Helpers
        private async Task<Result<AuthResponse>> GenerateAuthResponseAsync(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            await _refreshTokenRepo.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes)
            });
        } 

        private async Task<(int Count, DateTimeOffset LockedUntil)> GetFailRecordAsync(string key)
        {
            try
            {
                var raw = await _cache.GetStringAsync(key);

                if(raw is null)
                {
                    return (0, DateTimeOffset.MinValue);
                }

                var parts = raw.Split('|');
                if(parts.Length == 2
                    && int.TryParse(parts[0], out var count)
                    && DateTimeOffset.TryParse(parts[1], out var lockedUntil))
                {
                    return (count, lockedUntil);
                }

                await _cache.RemoveAsync(key);
                return (0, DateTimeOffset.MinValue);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read login fail record from cache for key {Key}", key);
                return (0, DateTimeOffset.MinValue);
            }
        }

        private async Task IncrementFailAsync(string key, (int Count, DateTimeOffset LockedUntil) record)
        {
            try
            {
                var newCount = record.Count + 1;

                var lockedUntil = newCount >= 5
                    ? DateTimeOffset.UtcNow.AddMinutes(15)
                    : record.LockedUntil == DateTimeOffset.MinValue
                        ? DateTimeOffset.UtcNow.AddMinutes(15)
                        : record.LockedUntil;

                var value = $"{newCount}|{lockedUntil:O}";

                await _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = lockedUntil
                });
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "Failed to increment login fail counter for key {Key}", key);
            }
        }

        private static string FailKey(string email) => $"login-fails:{email}";

        #endregion
    }
}
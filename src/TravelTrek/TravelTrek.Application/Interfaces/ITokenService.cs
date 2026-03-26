using System.Security.Claims;
using TravelTrek.Domain.Entities;

namespace TravelTrek.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(Guid userId);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}

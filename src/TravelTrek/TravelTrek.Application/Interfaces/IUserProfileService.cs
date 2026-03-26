using TravelTrek.Application.DTOs.Profile;
using TravelTrek.Domain.Common;

namespace TravelTrek.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<Result<UserProfileResponse>> GetProfileAsync(Guid userId);
        Task<Result<UserProfileResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
    }
}

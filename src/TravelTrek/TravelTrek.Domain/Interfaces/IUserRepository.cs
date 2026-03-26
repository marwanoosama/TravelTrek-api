using TravelTrek.Domain.Entities;

namespace TravelTrek.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByGoogleIdAsync(string googleId);
    }
}

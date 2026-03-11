namespace TravelTrek.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<Entities.User>
    {
        Task<Entities.User?> GetByGoogleIdAsync(string googleId);
    }
}

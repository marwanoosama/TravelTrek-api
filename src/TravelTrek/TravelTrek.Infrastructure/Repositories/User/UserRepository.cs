using Microsoft.EntityFrameworkCore;
using TravelTrek.Domain.Entities;
using TravelTrek.Domain.Interfaces;
using TravelTrek.Infrastructure.Data;

namespace TravelTrek.Infrastructure.Repositories.User
{
    public class UserRepository : GenericRepository<Domain.Entities.User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Domain.Entities.User?> GetByGoogleIdAsync(string googleId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.GoogleId == googleId);
        }
    }
}


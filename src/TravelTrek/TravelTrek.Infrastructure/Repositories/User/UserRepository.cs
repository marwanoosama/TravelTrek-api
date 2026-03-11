using Microsoft.EntityFrameworkCore;
using TravelTrek.Domain.Entities;
using TravelTrek.Infrastructure.Data;

namespace TravelTrek.Infrastructure.Repositories.User
{
    public class UserRepository : GenericRepository<Domain.Entities.User>
    {
        public UserRepository(AppDbContext context): base(context)
        {
        }
        
        public async Task<Domain.Entities.User?> GetByGoogleId(string googleId)
        {
            var user = await base._dbSet.FirstOrDefaultAsync(u => u.GoogleId == googleId);
            return user;
        }
    }
}

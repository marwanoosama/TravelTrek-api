using TravelTrek.Domain.Interfaces;
using TravelTrek.Infrastructure.Data;

namespace TravelTrek.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            // AppDbContext lifetime is managed by the DI container (Scoped).
            // We do NOT dispose it here to avoid ObjectDisposedException
            // for other services that share the same scoped context.
            GC.SuppressFinalize(this);
        }
    }
}
using Lab12.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly TripsDbContext _context;

        public TripRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalTripsAsync()
        {
            return await _context.Trips.CountAsync();
        }

        public async Task<List<Trip>> GetTripsAsync(int page, int pageSize)
        {
            return await _context.Trips
                .Include(t => t.IdCountries)
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        
        
    }
}
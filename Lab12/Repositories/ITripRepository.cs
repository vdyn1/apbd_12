using Lab12.Models;

namespace Lab12.Repositories
{
    public interface ITripRepository
    {
        Task<int> GetTotalTripsAsync();
        Task<List<Trip>> GetTripsAsync(int page, int pageSize);
    }
}
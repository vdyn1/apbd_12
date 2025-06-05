using Lab12.Models;

namespace Lab12.Repositories
{
    public interface IClientsRepository
    {
        Task<bool> HasAssignedTripsAsync(int idClient);
        Task<bool> ClientExistsAsync(int idClient);
        Task DeleteClientAsync(int idClient);
        Task<bool> ClientExistsByPeselAsync(string pesel);
        Task<int> AddClientAsync(Client client);
        Task AddClientTripAsync(int idClient, int idTrip, DateTime registeredAt, DateTime? paymentDate);
        Task<bool> TripExistsAsync(int idTrip);
        Task<DateTime?> GetTripStartDateAsync(int idTrip);
        
        Task<int> GetClientIdByPeselAsync(string pesel);
    }
}
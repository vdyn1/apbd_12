using Lab12.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly TripsDbContext _context;

    public ClientsRepository(TripsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasAssignedTripsAsync(int idClient)
    {
        return await _context.ClientTrips.AnyAsync(ct => ct.IdClient == idClient);
    }

    public async Task<bool> ClientExistsAsync(int idClient)
    {
        return await _context.Clients.AnyAsync(c => c.IdClient == idClient);
    }

    public async Task DeleteClientAsync(int idClient)
    {
        var client = await _context.Clients.FindAsync(idClient);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ClientExistsByPeselAsync(string pesel)
    {
        return await _context.Clients.AnyAsync(c => c.Pesel == pesel);
    }

    public async Task<int> AddClientAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client.IdClient;
    }

    public async Task AddClientTripAsync(int idClient, int idTrip, DateTime registeredAt, DateTime? paymentDate)
    {
        var clientTrip = new ClientTrip
        {
            IdClient = idClient,
            IdTrip = idTrip,
            RegisteredAt = registeredAt,
            PaymentDate = paymentDate
        };
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> TripExistsAsync(int idTrip)
    {
        return await _context.Trips.AnyAsync(t => t.IdTrip == idTrip);
    }

    public async Task<DateTime?> GetTripStartDateAsync(int idTrip)
    {
        var trip = await _context.Trips
            .Where(t => t.IdTrip == idTrip)
            .Select(t => (DateTime?)t.DateFrom)
            .FirstOrDefaultAsync();
        return trip;
    }

    public async Task<int> GetClientIdByPeselAsync(string pesel)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == pesel);
        return client?.IdClient ?? 0;
    }

}
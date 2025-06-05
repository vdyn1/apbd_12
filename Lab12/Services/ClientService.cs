using Lab12.DTOs;
using Lab12.Models;
using Lab12.Repositories;

namespace Lab12.Services;

public class ClientService
{
    private readonly IClientsRepository _clientsRepository;

    public ClientService(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task<(bool Success, string Message)> DeleteClientAsync(int idClient)
    {
        if (!await _clientsRepository.ClientExistsAsync(idClient))
        {
            return (false, "Client not found.");
        }

        if (await _clientsRepository.HasAssignedTripsAsync(idClient))
        {
            return (false, "Cannot delete client. The client is assigned to at least one trip.");
        }

        await _clientsRepository.DeleteClientAsync(idClient);
        return (true, "Client deleted successfully.");
    }

    public async Task<(bool Success, string Message)> AssignClientToTripAsync(int idTrip, NewClientDTo dto)
    {
        var exists = await _clientsRepository.ClientExistsByPeselAsync(dto.Pesel);
        if (exists)
        {
            return (false, "Client with this PESEL already exists.");
        }
        
        var newClient = new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };
        var idClient = await _clientsRepository.AddClientAsync(newClient);

        if (!await _clientsRepository.TripExistsAsync(idTrip))
        {
            return (false, "Trip not found.");
        }

        var tripStartDate = await _clientsRepository.GetTripStartDateAsync(idTrip);
        if (tripStartDate == null)
        {
            return (false, "Trip start date not found.");
        }

        if (tripStartDate <= DateTime.UtcNow)
        {
            return (false, "Cannot assign client. Trip has already started.");
        }

        var registeredAt = DateTime.UtcNow;
        await _clientsRepository.AddClientTripAsync(idClient, idTrip, registeredAt, dto.PaymentDate);

        return (true, "Client assigned to trip successfully.");
    }

}

using Lab12.DTOs;
using Lab12.Repositories;

namespace Lab12.Services
{
    public class TripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<object> GetTripsAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var totalTrips = await _tripRepository.GetTotalTripsAsync();
            var allPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

            var trips = await _tripRepository.GetTripsAsync(page, pageSize);

            var result = new
            {
                pageNum = page,
                pageSize = pageSize,
                allPages = allPages,
                trips = trips.Select(t => new TripDto
                {
                    Name = t.Name,
                    Description = t.Description,
                    DateFrom = t.DateFrom,
                    DateTo = t.DateTo,
                    MaxPeople = t.MaxPeople,
                    Countries = t.IdCountries.Select(c => new CountryDto
                    {
                        Name = c.Name
                    }).ToList(),
                    Clients = t.ClientTrips.Select(ct => new ClientDto
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    }).ToList()
                }).ToList()
            };

            return result;
        }
    }
}
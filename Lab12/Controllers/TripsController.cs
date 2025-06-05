using Lab12.DTOs;
using Lab12.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab12.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly TripService _tripService;
        private readonly ClientService _clientService;

        public TripsController(TripService tripService, ClientService clientService)
        {
            _tripService = tripService;
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _tripService.GetTripsAsync(page, pageSize);
            return Ok(result);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] NewClientDTo dto)
        {
            var result = await _clientService.AssignClientToTripAsync(idTrip, dto);
            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { error = result.Message });
        }
    }
    
}
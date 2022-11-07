using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EuroTrip2.Contexts;
using EuroTrip2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace EuroTrip2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly FlightDBContext _context;

        public FlightsController(FlightDBContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllFlights()
        {
            var flights = await _context.Flights.ToListAsync();

            return Ok(flights);
        }

        // GET: api/Flights/5
        [HttpGet]
        [Route("{flightId:int}")]
        [Authorize]
        public async Task<IActionResult> GetFlight([FromRoute] int flightId)
        {
            var flight = await _context.Flights.FirstOrDefaultAsync(x => x.flightId == flightId);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        // PUT: api/Flights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{flightId:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateFlight([FromRoute] int flightId, Flight updateFlightRequest)
        {
            var flight = await _context.Flights.FindAsync(flightId);

            if (flight == null || flightId != flight.flightId)
            {
                return NotFound();
            }
            flight.flightName = updateFlightRequest.flightName;
            flight.seatCount = updateFlightRequest.seatCount;
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(flightId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(flight);
        }

        // POST: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFlight([FromBody] Flight flightRequest)
        {
            await _context.Flights.AddAsync(flightRequest);
            _context.SaveChanges();
            int id = flightRequest.flightId;
            char letter = 'A';
            for (int i = 0; i < flightRequest.seatCount / 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    var seat = new Seat();
                    seat.Name = j.ToString() + letter;
                    seat.Flight_Id = flightRequest.flightId;
                    _context.Add(seat);
                }
                letter++;
            }
            await _context.SaveChangesAsync();
            //return Ok(flightRequest);
            return CreatedAtAction("GetAllFlights", new { id = flightRequest.flightId }, flightRequest);
        }

        // DELETE: api/Flights/5
        [HttpDelete]
        [Route("{flightId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteFlight([FromRoute] int flightId)
        {
            var flight = await _context.Flights.FindAsync(flightId);

            if (flight == null)
            {
                return NotFound();
            }
            _context.Flights.Remove(flight);

            await _context.SaveChangesAsync();

            return Ok(flight);
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.flightId == id);
        }
    }
}


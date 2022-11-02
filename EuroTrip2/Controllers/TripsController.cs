using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EuroTrip2.Contexts;
using EuroTrip2.Models;
using EuroTrip2.ModelView;

namespace EuroTrip2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly FlightDBContext _context;

        public TripsController(FlightDBContext context)
        {
            _context = context;
        }

        // GET: api/Trips
        [HttpGet]
        public async Task<IActionResult> GetTrip()
        {
            var trips = await _context.Trips.ToListAsync();

            return Ok(trips);
        }

        // GET: api/Trips/5
        [HttpGet]
        [Route("{Id:int}")]
        //in the post method
        public async Task<IActionResult> GetAllTrips([FromRoute] int Id)
        {
            var trips = await _context.Trips.FirstOrDefaultAsync(x => x.Id == Id);

            if (trips == null)
            {
                return NotFound();
            }

            return Ok(trips);
        }

        // PUT: api/Trips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{Id:int}")]
        public async Task<IActionResult> UpdateTrip([FromRoute] int Id, Trip updateTripRequest)
        {
            var trip = await _context.Trips.FindAsync(Id);

            if (trip == null || Id != trip.Id)
            {
                return NotFound();
            }
            trip.RouteName = updateTripRequest.RouteName;
            trip.PassengerCount = updateTripRequest.PassengerCount;
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(trip);
        }

        // POST: api/Trips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddTrip([FromBody] Trip tripRequest)
        {
            await _context.Trips.AddAsync(tripRequest);
            if (tripRequest.PassengerCount == 0 || tripRequest.PassengerCount==null)
            {
                tripRequest.PassengerCount = _context.Trips.FirstOrDefault(x => x.Id == tripRequest.Flight_Id).PassengerCount;
            }
            var flag = await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllTrips", new { Id = tripRequest.Id }, tripRequest);
        }

        // DELETE: api/Trips/5
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> DeleteTrip([FromRoute] int Id)
        {
            var trip = await _context.Trips.FindAsync(Id);

            if (trip == null)
            {
                return NotFound();
            }
            _context.Trips.Remove(trip);

            await _context.SaveChangesAsync();

            return Ok(trip);
        }

        private bool TripExists(int Id)
        {
            return _context.Trips.Any(e => e.Id == Id);
        }
    }
}


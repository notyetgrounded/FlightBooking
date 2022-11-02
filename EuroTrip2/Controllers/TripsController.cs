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
        public ActionResult<IEnumerable<AdminTripView>> GetTrips()
        {
            var temp = _context.Trips;
            if(!temp.Any())
            {
                return NoContent();
            }
            var trips = temp.Include(x => x.Flight).Include(x => x.TripRoute).ThenInclude(x => x.Destination).Include(x => x.TripRoute).ThenInclude(x => x.Source);
            List<AdminTripView> result = new List<AdminTripView>();
            foreach(var trip in trips)
            {
                AdminTripView tripView = new AdminTripView();
                tripView.PassengerCount=trip.PassengerCount;
                tripView.Price = trip.Price;
                tripView.SourceTime = trip.SourceTime;
                tripView.SourceName = trip.TripRoute.Source.Name;
                tripView.SourceIOTA = trip.TripRoute.Source.IOTA;
                tripView.DestinationName = trip.TripRoute.Destination.Name;
                tripView.DestinationIOTA = trip.TripRoute.Destination.IOTA;
                tripView.TripRoute_Id = trip.TripRoute_Id;
                tripView.DestinationTime = trip.DestinationTime;
                tripView.Id = trip.Id;
                tripView.FlightId = trip.Flight_Id;
                tripView.FlightName = trip.Flight.Name;
                tripView.Name = trip.Name;
                result.Add(tripView);
            }
            return result;

            
        }

        // GET: api/Trips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(int id)
        {
            var trip = await _context.Trips.FindAsync(id);

            if (trip == null)
            {
                return NotFound();
            }

            return trip;
        }

        // PUT: api/Trips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(int id, Trip trip)
        {
            if (id != trip.Id)
            {
                return BadRequest();
            }

            _context.Entry(trip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Trips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Trip>> PostTrip(Trip trip)
        {
            if (trip.PassengerCount == 0)
            {
                trip.PassengerCount = _context.Flights.FirstOrDefault(x => x.Id == trip.Flight_Id).SeatCount;
            }
            int count = 0;
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
            foreach (var seat in _context.Seats.Where(x => x.Flight_Id == trip.Flight_Id))
            {
                if (count >= trip.PassengerCount)
                {
                    break;
                }
                SeatStatus seatStatus = new SeatStatus();
                seatStatus.Seat_Id = seat.Id;
                seatStatus.Trip_Id = trip.Id;
                seatStatus.IsFree = true;
                _context.Add(seatStatus);
            }
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetTrip", new { id = trip.Id }, trip);
        }

        // DELETE: api/Trips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}

using EuroTrip2.Contexts;
using EuroTrip2.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EuroTrip2.Models;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;

namespace EuroTrip2.Controllers.Services
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeatBookingController : ControllerBase
    {
        protected readonly FlightDBContext _context;
        public SeatBookingController(FlightDBContext context)
        {
            _context=context;
        }
        [HttpPost]

        public ActionResult<int> BookSeats(MakeBookingView makeBooking)
        {           
            if (makeBooking.Email == null){ return BadRequest(); }
            var user =_context.Users.Where(x=>x.Email==makeBooking.Email).FirstOrDefault();
            if(user == null)
            {
                user = new User();
                user.Email =makeBooking.Email;
                user.Name= makeBooking.Name;
                _context.Add(user);
                _context.SaveChanges();
            }
            var trip = _context.Trips.Find(makeBooking.TripId);
            var passengers = makeBooking.Passengers;
            if (trip==null || passengers.Count() > trip.PassengerCount) { return BadRequest(); }
            var freeseats = GetFreeSeats(makeBooking.TripId, passengers.Count());
            if (freeseats.Count() == 0) {  return BadRequest(freeseats.Count()+"seats left"); }
            int id;
            if (_context.Bookings.Any())
            {
                id = _context.Bookings.Max(x => x.Id) + 1;
            }
            else
            {
                id = 1;
            }
                for(int i=0;i<passengers.Count();i++)
            {
                var passenger = passengers[i];
                Booking booking = new Booking();
                booking.Status = (int)Options.BookingStatus.Booked;
                booking.Trip_Id = makeBooking.TripId;
                booking.User_Id = user.Id;
                booking.PassengerAge= passenger.Age;
                booking.PassengerGender =(short) passenger.Gender;
                booking.PassengerName = passenger.Name;
                booking.DateTime = DateTime.Now;
                booking.Seat_Id = freeseats[i];
                booking.Id = id;
                trip.PassengerCount--;
                _context.Bookings.Add(booking);
                _context.SaveChanges();

            }
            return id;
        }
        [NonAction]
        public List<int> GetFreeSeats(int trip_Id,int count)
        {
            var flightId = _context.Trips.Find(trip_Id).Flight_Id;
            var bookedSeats = (from book in _context.Bookings.Where(x => x.Trip_Id == trip_Id
                             && (x.Status == (int)Options.BookingStatus.Booked || x.Status == (int)Options.BookingStatus.Pending))
                               select book.Seat_Id).ToList();
            var FreeSeats = _context.Seats.Where(x => !bookedSeats.Contains(x.Id)).Take(count).Select(x=>x.Id).ToList();
            return FreeSeats;
        }
        [HttpDelete]
        public ActionResult<HttpResponseMessage> CancelBooking (string Email,int booking_Id,string passengerName)
        {
            var bookings = _context.Bookings.Include(x=>x.User).Where(x=>x.Id==booking_Id && x.PassengerName==passengerName && x.User.Email==Email);
            if (bookings.Any() == null) { BadRequest(); }
            var currentBooking = bookings.Include(x => x.Trip).First();
            currentBooking.Status = (int)Options.BookingStatus.Cancelled;
            currentBooking.Trip.PassengerCount++;
            _context.SaveChanges();
            return Ok();

        }
    }
}

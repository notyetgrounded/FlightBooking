using EuroTrip2.Contexts;
using EuroTrip2.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EuroTrip2.Models;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.JSInterop;

namespace EuroTrip2.Controllers.Services
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeatBookingController : ControllerBase
    {
        protected readonly FlightDBContext _context;

        public SeatBookingController(FlightDBContext context)
        {
            _context = context;
        }
        [HttpPost]

        public async Task<ActionResult<HttpResponseMessage>> BookSeats(MakeBookingView makeBooking)
        {
            User user;

            //
            if (!_context.Users.Any()) { user = new User() { Email = "User1@gmail.com", Name = "User1" }; _context.Users.Add(user); _context.SaveChanges(); }
            else { user = _context.Users.First(); }
            //need to be changes
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            List<Passenger> passengers = new List<Passenger>();
            foreach (var passenger in makeBooking.Passengers)
            {
                passengers.Add(passenger);
                _context.Passengers.Add(passengers.Last());
            }
            _context.SaveChanges();

            Booking PreviousBooking = null;

            foreach (var tripId in makeBooking.TripIds)
            {
                var FreeSeats = GetFreeSeats(tripId, makeBooking.Passengers.Count());
                if (FreeSeats.Count() < makeBooking.Passengers.Count()) { return NotFound(); }
                Trip trip = _context.Trips.Include(x => x.SeatStatuses).Where(x => x.Id == tripId).FirstOrDefault();
                Booking booking = new Booking();

                booking.TotalPrice = trip.Price * makeBooking.Passengers.Count();
                booking.Name = makeBooking.Name;
                booking.Email = makeBooking.Email;
                booking.PhoneNo = makeBooking.PhoneNo;
                booking.User_Id = user.Id;
                booking.Trip_Id = tripId;

                _context.Bookings.Add(booking);
                _context.SaveChanges();
                if (PreviousBooking != null)
                {
                    PreviousBooking.NextBooking_Id = booking.Id;
                }
                PreviousBooking = booking;
                for (int i = 0; i < FreeSeats.Count(); i++)
                {
                    makeTicket(seat_Id: FreeSeats[i], booking_Id: booking.Id, passenger_id: passengers[i].Id, price: trip.Price);

                    trip.PassengerCount--;
                    trip.SeatStatuses.Where(x => x.Seat_Id == FreeSeats[i]).FirstOrDefault().IsFree = false;

                }
            }
            await _context.SaveChangesAsync();
            return Ok();

        }
        [NonAction]
        public bool makeTicket(int seat_Id, int booking_Id, int passenger_id, int price)
        {
            Ticket ticket = new Ticket();
            ticket.Status = (int)Options.BookingStatus.Booked;
            ticket.Seat_Id = seat_Id;
            ticket.Price = price;
            ticket.Booking_Id = booking_Id;
            ticket.Passenger_Id = passenger_id;
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return true;
        }
        [HttpDelete]
        [Route("{booking_Id}")]
        public async Task<ActionResult<HttpResponseMessage>> CancelBookings([FromRoute]int booking_Id)
        {
            var booking = _context.Bookings.Include(x => x.Trip).ThenInclude(x => x.SeatStatuses).Include(x => x.Tickets).Where(x => x.Id == booking_Id).FirstOrDefault();

            foreach (var ticket in booking.Tickets)
            {
                ticket.Status = (int)Options.BookingStatus.Cancelled;
                booking.Trip.PassengerCount++;
                booking.Trip.SeatStatuses.Where(x => x.Seat_Id == ticket.Seat_Id).FirstOrDefault().IsFree = true;

            }
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<HttpResponseMessage>> UpdatePassengers(List<Passenger> passengers)
        {

            _context.Passengers.UpdateRange(passengers);
            await _context.SaveChangesAsync();
            return Ok("Updated passengers");
        }
        //[HttpPut]
        //public async Task<ActionResult<HttpResponseMessage>> UpdatePassenger(Passenger passenger)
        //{
        //    _context.Passengers.Add(passenger);
        //    await _context.SaveChangesAsync();
        //    return Ok("Updated passenger");
        //}

        [NonAction]
        public List<int> GetFreeSeats(int trip_Id, int count)
        {
            var seatsIds = _context.SeatStatuses.Where(x => x.Trip_Id == trip_Id && x.IsFree == true).Select(x => x.Seat_Id).Take(count).ToList();
            return seatsIds;
        }
    }
}

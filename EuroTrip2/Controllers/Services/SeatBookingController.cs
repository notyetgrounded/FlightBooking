using EuroTrip2.Contexts;
using EuroTrip2.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EuroTrip2.Models;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = _context.Users.Where(x => x.Email == makeBooking.Email).FirstOrDefault();
            if(user==null)
            {
                user = new User()
                {
                    Email = makeBooking.Email,
                    Name = makeBooking.Name
                };
                _context.Users.Add(user);
                _context.SaveChanges();
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
                var ij = FreeSeats.Count();
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
                IncrementPrice(tripId);
            }
            await _context.SaveChangesAsync();
            return Ok();

        }
        [NonAction]
        public  void IncrementPrice(int trip_Id)
        {
            var trip = _context.Trips.Find(trip_Id);
            trip.Price = (int)(trip.Price + trip.Price * 0.02);
            
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
            if(booking == null)
            {
                return BadRequest("that booking id does not exist ");
            }

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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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

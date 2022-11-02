using EuroTrip2.Contexts;
using EuroTrip2.Models;
using EuroTrip2.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace EuroTrip2.BussinessLayer
{
    public class BookingRepository: IBookingInterface
    {
        protected readonly FlightDBContext _context;
        public BookingRepository(FlightDBContext context)
        {
            _context = context;
        }
        public async Task<bool> BookSeats(MakeBookingView makeBooking)
        {
            
            User user = _context.Users.Where(x => x.Email == makeBooking.Email).FirstOrDefault();
            if (user == null)
            {
                user = new User()
                {
                    Email = makeBooking.Email,
                    Name = makeBooking.Name
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
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
                if (FreeSeats.Count() < makeBooking.Passengers.Count()) {
                    return false;
                   
                }
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
            return true;

        }
        
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
        public async Task< bool> BookingExists(int booking_Id)
        {
            return await _context.Bookings.FindAsync(booking_Id) != null;
        }
        public async Task<bool> CancelBookings(int booking_Id)
        {

            var booking = _context.Bookings.Include(x => x.Trip).ThenInclude(x => x.SeatStatuses).Include(x => x.Tickets).Where(x => x.Id == booking_Id).FirstOrDefault();
    

            foreach (var ticket in booking.Tickets)
            {
                ticket.Status = (int)Options.BookingStatus.Cancelled;
                booking.Trip.PassengerCount++;
                booking.Trip.SeatStatuses.Where(x => x.Seat_Id == ticket.Seat_Id).FirstOrDefault().IsFree = true;

            }
            await _context.SaveChangesAsync();
            return true;
        }
      
        public async Task<bool> UpdatePassengers(List<Passenger> passengers)
        {
            
            _context.Passengers.UpdateRange(passengers);
            await _context.SaveChangesAsync();
            return true;
        }
        //[HttpPut]
        //public async Task<ActionResult<HttpResponseMessage>> UpdatePassenger(Passenger passenger)
        //{
        //    _context.Passengers.Add(passenger);
        //    await _context.SaveChangesAsync();
        //    return Ok("Updated passenger");
        //}

       
        public List<int> GetFreeSeats(int trip_Id, int count)
        {
            var seatsIds = _context.SeatStatuses.Where(x => x.Trip_Id == trip_Id && x.IsFree == true).Select(x => x.Seat_Id).Take(count).ToList();
            return seatsIds;
        }
        public async Task<bool> IncremntPrice(int trip_Id,int Percentage,int Change)
        {
            var trip = _context.Trips.Find(trip_Id);
            if(trip==null || trip.PassengerCount%10>0)
            {
                return false;
            }
            trip.Price = (int)(trip.Price + trip.Price * 0.1);
            await _context.SaveChangesAsync();
            return true;
        }
        //public a Task<bool>
    }
}

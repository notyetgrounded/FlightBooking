using EuroTrip2.Contexts;
using EuroTrip2.Models;
using EuroTrip2.ModelView;
using EuroTrip2.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EuroTrip2.Controllers.Services
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        protected readonly FlightDBContext _context;
        public GeneralController(FlightDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            return await _context.Places.ToListAsync();
        }
        [HttpGet]
        // new part 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripRoute>>> GetTripRoutes()
        {
            return await _context.TripRoutes.ToListAsync();
        }
        //new part

        [HttpGet]
        [Route("{sourceId}/{destinationId}/{sourceTime}/{passengerCount}")]
        [Authorize]
        public ActionResult<IEnumerable<TripView>> GetTrips([FromRoute]int sourceId, int destinationId,DateTime sourceTime,int passengerCount)
        {
            List<TripView> completeTrips = new List<TripView>();
            var tripRoute=_context.TripRoutes.Where(x=>x.Source_Id==sourceId && x.Destination_Id==destinationId);
            if (tripRoute == null)
            { return NoContent(); }
            var gap = 0;
            
            foreach (var route in tripRoute)
            {
                var directTripIds = _context.Trips.Where(x => x.TripRoute.Id == route.Id && x.PassengerCount >= passengerCount && x.SourceTime.Date== sourceTime.Date ).Select(x => x.Id).ToList();

                foreach (var tripId in directTripIds)
                {
                    var completeTrip = new TripView();
                    completeTrip= FillTripView(tripId) ;
                    completeTrips.Add(completeTrip);
                }
            }
            /*
            var oneStopRoutes = from route1 in _context.TripRoutes.Where(x => x.Source_Id == sourceId)
                                join route2 in _context.TripRoutes.Where(x => x.Destination_Id == destinationId)
                                on route1.Destination_Id equals route2.Source_Id
                                select new List<int>() { route1.Id, route2.Id };
            foreach (var route in oneStopRoutes)
            {                
                var combinations = from trip1 in _context.Trips.Where(x => x.TripRoute_Id == route[0] && x.SourceTime >= sourceTime && x.SourceTime < sourceTime.AddDays(gap))
                                   from trip2 in _context.Trips.Where(x => x.TripRoute_Id == route[1] && x.SourceTime >= trip1.DestinationTime && x.SourceTime <= trip1.DestinationTime.AddDays(gap))
                                   select new List<TripView>() { FillTripView(trip1.Id,_context), FillTripView(trip2.Id,_context) };
                foreach(var trip in combinations)
                {
                    var completeTrip = new CompleteTrip();
                    completeTrip.TripViews= trip;
                    completeTrips.Add(completeTrip);
                }
            }
            */
            return completeTrips;

        }
        [NonAction]
        public TripView FillTripView(int id)
        {
            var trip = _context.Trips.Include(x => x.TripRoute).ThenInclude(x => x.Source).Include(x => x.TripRoute).ThenInclude(x => x.Destination).Include(x => x.Flight).Where(x=>x.Id==id).SingleOrDefault();
           
            TripView tripView = new TripView();
            if (trip == null)
            {
                return tripView;
            }
            /*old one
            tripView.Id=trip.Id;
            tripView.FlightName = tripView.FlightName;
            tripView.SourceName = trip.TripRoute.Source.Name;
            tripView.SourceIOTA = trip.TripRoute.Source.IOTA;
            tripView.DestinationName = trip.TripRoute.Destination.Name;
            tripView.DestinationIOTA = trip.TripRoute.Destination.IOTA;
            tripView.DestinationTime = trip.DestinationTime;
            tripView.SourceTime = trip.SourceTime;
            tripView.Price = trip.Price;
            tripView.Name = trip.Name;
            tripView.PassengerCount = trip.PassengerCount;
            return tripView;
            new one*/
            
            var result = trip.Flight.flightName.Split(' ');
            tripView.Id = trip.Id;
            tripView.airlines = result[0];
            tripView.planeNo = result[1];
            tripView.Source = trip.TripRoute.Source.Name;
            tripView.SourceIOTA = trip.TripRoute.Source.IOTA;
            tripView.Destination = trip.TripRoute.Destination.Name;
            tripView.DestinationIOTA = trip.TripRoute.Destination.IOTA;
            tripView.DestinationDate = trip.DestinationTime.ToString("dd MMM yyyy");
            tripView.SourceDate = trip.SourceTime.ToString("dd MMM yyyy");
            tripView.DestinationTime = trip.DestinationTime.ToString("HH:mm");
            tripView.SourceTime = trip.SourceTime.ToString("HH:mm");
            //logic to find duration
            TimeSpan diff = (trip.DestinationTime - trip.SourceTime);
            tripView.Duration = diff.Hours + " hours " + diff.Minutes + " mins";

            tripView.Price = trip.Price;
            tripView.Name = trip.RouteName;
            tripView.stops = 0;
            tripView.SeatCount = trip.PassengerCount;
            return tripView;
        }

        [HttpGet]
        [Route("{email}")]
        [Authorize]
        public ActionResult<IEnumerable<BookingsView>> GetMyBookings([FromRoute]string email)
        {
            List<BookingsView> bookingsViews = new List<BookingsView>();
            var userQueary=_context.Users.Where(x=>x.Email==email);
            if (userQueary==null)
            {
                return bookingsViews;
            }
            var user = userQueary.Include(x => x.Bookings).FirstOrDefault();
            if (user== null)
            {
                return bookingsViews;
            }
            var bookings = _context.Bookings.Where(x => x.User_Id == user.Id).Include(x => x.Trip).ThenInclude(x => x.TripRoute).Include(x => x.Tickets).ThenInclude(x => x.Passenger).Include(x => x.Tickets).ThenInclude(x => x.Seat);

            

            foreach (var booking in bookings)
            {
                if (booking.NextBooking != null || booking.FromBooking != null)
                {
                    continue;
                }
                BookingsView bookingsView = new BookingsView()
                {
                    BookingId = booking.Id,
                    Name = booking.Name,
                    Email = booking.Email,
                    PhoneNo = booking.PhoneNo,
                    DateTime = booking.BookingDate.ToString("dd MMM yyyy"),
                    TripId = (int)booking.Trip_Id,
                    TripName = booking.Trip.RouteName,
                    Source = GetLocation(booking.Trip.TripRoute.Source_Id),
                    Destination = GetLocation(booking.Trip.TripRoute.Destination_Id),
                    Status = (string)Enum.GetName(typeof(Options.BookingStatus), booking.Tickets.First().Status)
                };
                var passengers = new List<PassengerView>();
                foreach (var ticket in booking.Tickets)
                {
                    PassengerView passenger = new PassengerView()
                    {
                        Name = ticket.Passenger.Name,
                        Age = ticket.Passenger.Age,
                        Gender = (string)Enum.GetName(typeof(Options.Gender), ticket.Passenger.Gender),
                        TicketId = ticket.Id,
                        Id = ticket.Passenger.Id,
                        SeatName = ticket.Seat.Name,
                        Price = ticket.Price,
                        Status = (string)Enum.GetName(typeof(Options.BookingStatus),booking.Tickets.First().Status)

                    };
                    passengers.Add(passenger);
                }
                bookingsView.Passengers = passengers;


                bookingsViews.Add(bookingsView);
            }
            return bookingsViews;
        }
        [NonAction]
        public string GetLocation(int id)
        {
            return _context.Places.FirstOrDefault(x => x.Id == id).Name;
        }

        
        //[HttpGet]
        //public ActionResult<SeatsView> GetSeatStatusByTripId(int id )
        //{
        //    var temp = _context.Trips.Where(x => x.Id == id);
        //    if (!temp.Any())
        //    {
        //        return NotFound();
        //    }
        //    var trip = temp.Include(x => x.TripRoute).ThenInclude(x => x.Destination).Include(x => x.TripRoute).ThenInclude(x => x.Destination).Include(x=>x.TripRoute).ThenInclude(x=>x.Source).First();
        //    var seats= _context.Seats.Where(x=>x.Flight_Id==trip.Flight_Id).ToList();
        //    if (!seats.Any())
        //    {
        //        return NoContent();
        //    }
        //    var seatsView = new SeatsView();
        //    seatsView.Price=trip.Price;
        //    seatsView.TripName = trip.Name;
        //    seatsView.TripId = trip.Id;
        //    seatsView.Destination = trip.TripRoute.Destination.Name;
        //    seatsView.DestinationIOTA = trip.TripRoute.Destination.IOTA;
        //    seatsView.DestinationTime = trip.DestinationTime;
        //    seatsView.Source = trip.TripRoute.Source.Name;
        //    seatsView.SourceIOTA = trip.TripRoute.Source.IOTA;
        //    seatsView.SourceTime = trip.SourceTime;
            
        //    var seatStatusList = new List<SeatStatus>();
            
        //    var bookedSeatsIds= _context.Bookings.Where(x=>x.Trip_Id==trip.Id && (x.Status==(int)BookingStatus.Booked || x.Status== (int)BookingStatus.Pending)).Select(x=>x.Id).ToList();
        //    foreach(var seat in seats)
        //    {
        //        var seatStatus = new SeatStatus();
        //        seatStatus.SeatName=seat.Name; 
        //        seatStatus.SeatId= seat.Id;
        //        seatStatus.Status = true;
        //        if (bookedSeatsIds.Contains(seat.Id))
        //        {
        //            seatStatus.Status = false;
        //        }
        //        seatStatusList.Add(seatStatus);
        //    }
        //    seatsView.Seats=seatStatusList;
        //    return seatsView;
        //}

    }
}

using EuroTrip2.BussinessLayer;
using EuroTrip2.Contexts;
using EuroTrip2.Models;
using EuroTrip2.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        protected readonly IGeneralInterface _generalRepository;
        public GeneralController(IGeneralInterface general)
        {
            _generalRepository = general;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            
            return Ok(_generalRepository.GetPlaces());
        }
        [HttpGet]
        // new part 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripRoute>>> GetTripRoutes()
        {
            return  Ok(_generalRepository.GetTripRoutes());
        }
        //new part

        [HttpGet]
        [Route("{sourceId}/{destinationId}/{sourceTime}/{passengerCount}")]
        public ActionResult<IEnumerable<TripView>> GetTrips([FromRoute]int sourceId, int destinationId,DateTime sourceTime,int passengerCount)
        {
            var res = _generalRepository.GetTrips(sourceId, destinationId, sourceTime, passengerCount);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);

        }
        
       

        [HttpGet]
        [Route("{email}")]
        public ActionResult<IEnumerable<BookingsView>> GetMyBookings([FromRoute]string email)
        {
            var res = _generalRepository.GetMyBookings(email);
            if (res == null) { return NotFound(); }
            return Ok(res);
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

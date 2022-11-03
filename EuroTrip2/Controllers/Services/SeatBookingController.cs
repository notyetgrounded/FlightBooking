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
using EuroTrip2.BussinessLayer;

namespace EuroTrip2.Controllers.Services
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [System.Web.Http.Authorize]
    public class SeatBookingController : ControllerBase
    {
        protected readonly FlightDBContext _context;
        protected readonly IBookingInterface _bookingRepository;
        public SeatBookingController(IBookingInterface bookingRepository) 
        {
            _bookingRepository = bookingRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<HttpResponseMessage>> BookSeats(MakeBookingView makeBooking)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_bookingRepository.BookSeats(makeBooking).Result)
            {
              
                return Ok();
            }
            ModelState.AddModelError("Internal_Error", "Somthing Went Wrong Please Try again");
            return BadRequest(ModelState);

        }

        [HttpDelete]
        [Route("{booking_Id}")]
        [Authorize]
        public async Task<ActionResult<HttpResponseMessage>> CancelBookings([FromRoute]int booking_Id)
        {

            if(!_bookingRepository.BookingExists(booking_Id).Result)
            {
                return BadRequest("booking Doesnot Exsist ");
            }

            if (_bookingRepository.CancelBookings(booking_Id).Result)
            {

                return Ok();
            }
            else
            {
                return BadRequest("Somthing Went Wrong");
            }
        }
        [HttpPut]
        public async Task<ActionResult<HttpResponseMessage>> UpdatePassengers(List<Passenger> passengers)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           if(_bookingRepository.UpdatePassengers(passengers).Result)
            {
                return Ok("Successfully Updated Passengers");
            }
           else
            {
                return BadRequest("Somthing Went Wrong");
            }

        }
        //[HttpPut]
        //public async Task<ActionResult<HttpResponseMessage>> UpdatePassenger(Passenger passenger)
        //{
        //    _context.Passengers.Add(passenger);
        //    await _context.SaveChangesAsync();
        //    return Ok("Updated passenger");
        //}


    }
}

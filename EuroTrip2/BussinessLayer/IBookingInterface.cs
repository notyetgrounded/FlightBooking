using EuroTrip2.Models;
using EuroTrip2.ModelView;
using Microsoft.AspNetCore.Mvc;

namespace EuroTrip2.BussinessLayer
{
    public interface IBookingInterface
    {
        public Task<bool> BookSeats(MakeBookingView makeBooking);
        public Task<bool> CancelBookings(int booking_Id);
        public Task<bool> BookingExists(int booking_id);
        public Task<bool> UpdatePassengers(List<Passenger> passengers);
        public Task<bool> IncrementPrice (int Trip_Id,int percentage,int Change);
        //public Task<bool> DecrementPrice (int Trip_Id,int percentage,int Change);
    }
}

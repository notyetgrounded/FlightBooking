using EuroTrip2.Contexts;
using EuroTrip2.Models;
using EuroTrip2.ModelView;
using Microsoft.AspNetCore.Mvc;

namespace EuroTrip2.BussinessLayer
{
    public interface IGeneralInterface
    {
        public Task<IEnumerable<Place>> GetPlaces();
        public  Task<IEnumerable<TripRoute>> GetTripRoutes();

        public IEnumerable<TripView> GetTrips(int sourceId, int destinationId, DateTime sourceTime, int passengerCount);
        public IEnumerable<BookingsView> GetMyBookings(string email);
    }
}

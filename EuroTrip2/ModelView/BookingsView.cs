using EuroTrip2.Models;

namespace EuroTrip2.ModelView
{
    public class BookingsView
    {
        public int BookingId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string DateTime { get; set; }
        public string Status { get; set; }
        public int TripId { get; set; }
        public string TripName { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public List<PassengerView> Passengers { get; set; }
    }
}

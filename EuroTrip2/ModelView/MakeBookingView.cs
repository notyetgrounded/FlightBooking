using EuroTrip2.Models;

namespace EuroTrip2.ModelView
{
    public class MakeBookingView
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public List<int> TripIds { get; set; }
        public List<Passenger> Passengers { get; set; }
    }
}

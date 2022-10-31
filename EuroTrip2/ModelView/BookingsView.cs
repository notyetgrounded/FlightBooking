namespace EuroTrip2.ModelView
{
    public class BookingsView
    {
        public int BookingId    { get; set; }
        public string PassengerName { get; set; }
        public int PassengerAge { get; set; }
        public short PassengerGender { get; set; }
        public string DateTime { get; set; }
        public string Status { get; set; }
        public int TripId { get; set; }
        public string TripName { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }

    }
}

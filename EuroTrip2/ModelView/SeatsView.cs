using EuroTrip2.Models;
using System.Security.Permissions;

namespace EuroTrip2.ModelView
{
    public class SeatsView
    {
        public int TripId   { get; set; }
        public string TripName { get; set; }
        public string Source { get; set; }
        public string SourceIOTA { get; set; }

        public DateTime SourceTime { get; set; }
        public DateTime DestinationTime  { get; set; }
        
        public string Destination { get; set; }
        public string DestinationIOTA { get; set; }

        public int Price { get; set; }
        public IEnumerable<SeatStatus> Seats { get; set; }
    }
}

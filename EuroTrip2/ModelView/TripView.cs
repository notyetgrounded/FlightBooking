using System.Text.Json.Serialization;

namespace EuroTrip2.ModelView
{
    
    public class TripView
    {
        /* old one
        public int Id { get; set; }

        
        public string Name { get; set; }
        public int FlightId { get; set; }
        public string FlightName { get; set; }
        public string SourceName { get; set; }
        public string SourceIOTA { get; set; }
        public DateTime SourceTime { get; set; }
        public string DestinationName { get; set; }
        public string DestinationIOTA { get; set; }
        public DateTime DestinationTime { get; set; }
        public int Price { get; set; }
        public int PassengerCount { get; set; }
        */

        //flightname has been deleted and trip id also

        public int Id { get; set; }
        public string airlines { get; set; }
        public string planeNo { get; set; }
        public string Name { get; set; }
        public int SeatCount { get; set; }
        public int stops { get; set; }

        public int Price { get; set; }
        public string Source { get; set; }
        public string SourceIOTA { get; set; }

        public string Destination { get; set; }
        public string DestinationIOTA { get; set; }
        public string SourceTime { get; set; }
        public string DestinationTime { get; set; }
        public string SourceDate { get; set; }
        public string DestinationDate { get; set; }
        public string Duration { get; set; }
        //public int Route_Id { get; set; }
        //srcTime and destTime Datatype has beenchanged
    }
}

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EuroTrip2.Models
{
    public class Flight
    {
        [Key]

        public int flightId { get; set; }
        public string flightName { get; set; }
        public int seatCount { get; set; }

        //public int Price { get; set; }

        public ICollection<Seat>? Seats { get; set; }


        public ICollection<Trip>? Trips { get; set; }


    }
}

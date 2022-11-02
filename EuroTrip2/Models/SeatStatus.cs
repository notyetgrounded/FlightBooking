using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace EuroTrip2.Models
{
    public class SeatStatus
    {
        public int Seat_Id { get; set; }
        public int Trip_Id { get; set; }
        public bool IsFree { get; set; }

        [ForeignKey("Seat_Id")]
        public Seat Seat { get; set; }

        [ForeignKey("Trip_Id")]
        public Trip Trip { get; set; }
    }
}

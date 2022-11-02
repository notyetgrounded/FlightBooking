using System.ComponentModel.DataAnnotations.Schema;

namespace EuroTrip2.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int Status { get; set; }
        public int Price { get; set; }

        public int Seat_Id { get; set; }

        [ForeignKey("Seat_Id")]
        public Seat Seat { get; set; }

        public int Booking_Id { get; set; }

        [ForeignKey("Booking_Id")]
        public Booking Booking { get; set; }

        public int Passenger_Id { get; set; }

        [ForeignKey("Passenger_Id")]
        public Passenger Passenger { get; set; }
    }
}

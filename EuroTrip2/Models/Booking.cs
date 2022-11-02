using EuroTrip2.Options;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EuroTrip2.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;

        public int TotalPrice { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }


        public int Trip_Id { get; set; }



        [ForeignKey("Trip_Id")]
        public Trip? Trip { get; set; }

        public int User_Id { get; set; }



        [ForeignKey("User_Id")]
        public User? User { get; set; }


        public ICollection<Ticket>? Tickets { get; set; }

        public int? NextBooking_Id { get; set; }

        [ForeignKey("NextBooking_Id")]
        public Booking? NextBooking { get; set; }

        public Booking? FromBooking { get; set; }


    }
}

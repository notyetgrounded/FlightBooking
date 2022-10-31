using EuroTrip2.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EuroTrip2.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string PassengerName { get; set; }
        public int PassengerAge { get; set; }
        public short PassengerGender { get; set; } = (int)Gender.Others;
        public DateTime DateTime { get; set; }=DateTime.Now;
        public int Status { get; set; }


        public int Trip_Id { get; set; }



        [ForeignKey("Trip_Id")]
        public Trip? Trip { get; set; }

        public int User_Id { get; set; }



        [ForeignKey("User_Id")]
        public User? User { get; set; }


        public int Seat_Id { get; set; }



        [ForeignKey("Seat_Id")]
        public Seat? Seat { get; set; }

    }
}

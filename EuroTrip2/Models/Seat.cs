using EuroTrip2.ModelView;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EuroTrip2.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }


        public int Flight_Id { get; set; }



        [ForeignKey("Flight_Id")]
        public Flight? Flight { get; set; }


        public ICollection<SeatStatus>? SeatStatuses { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}

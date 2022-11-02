
using System.ComponentModel.DataAnnotations;

namespace EuroTrip2.Models
{
    public class Passenger
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public int Gender { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}

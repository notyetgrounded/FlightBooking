using EuroTrip2.Models;
using System.ComponentModel.DataAnnotations;

namespace EuroTrip2.ModelView
{
    public class MakeBookingView
    {
        [Required(ErrorMessage ="Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        public string PhoneNo { get; set; }
        public List<int> TripIds { get; set; }
        public List<Passenger> Passengers { get; set; }
    }
}

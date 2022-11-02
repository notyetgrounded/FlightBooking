using System.ComponentModel.DataAnnotations;

namespace EuroTrip2.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public Registration(int id, string userName, string emailID, string password)
        {
            Id = id;
            UserName = userName;
            EmailID = emailID;
            Password = password;
        }
    }
}

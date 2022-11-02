using System.ComponentModel.DataAnnotations;

namespace EuroTrip2.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string AdminPassword { get; set; }
        public Admin(int id, string name, string email, string adminPassword)
        {
            Id = id;
            Name = name;
            Email = email;
            AdminPassword = adminPassword;
        }
    }
}

namespace EuroTrip2.Models
{
    public class Login
    {
        public string EmailID { get; set; }
        public string Password { get; set; }
        public Login() { }
        public Login(string emailID, string password)
        {
            EmailID = emailID;
            Password = password;
        }
    }
}

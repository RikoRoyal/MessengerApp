using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Models.Account
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
    }
}

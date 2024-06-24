using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Models.Account
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }
    }
}

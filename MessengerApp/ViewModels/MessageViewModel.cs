using System;
using System.ComponentModel.DataAnnotations;

namespace MessengerApp.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public DateTime Timestamp { get; set; }

        public int ChatId { get; set; } 
    }
}

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MessengerApp.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUser> ChatUsers { get; set; }
    }
}

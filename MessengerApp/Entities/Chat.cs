using System.Collections.Generic;

namespace MessengerApp.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChatUser> ChatUsers { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}

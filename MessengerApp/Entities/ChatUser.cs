namespace MessengerApp.Entities
{
    public class ChatUser
    {
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}

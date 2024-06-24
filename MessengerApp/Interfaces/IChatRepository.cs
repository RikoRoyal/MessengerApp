using MessengerApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.Interfaces
{
    public interface IChatRepository
    {
        Task<IEnumerable<Chat>> GetAllChatsAsync();
        Task<Chat> GetChatByIdAsync(int chatId);
        Task AddChatAsync(Chat chat);
        Task<Chat> GetChatByNameAsync(string chatName);
        Task<Chat> GetChatBetweenUsersAsync(string user1, string user2);
    }
}

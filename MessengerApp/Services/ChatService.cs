using MessengerApp.Entities;
using MessengerApp.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApp.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            return await _chatRepository.GetAllChatsAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            return await _chatRepository.GetChatByIdAsync(chatId);
        }

        public async Task AddChatAsync(Chat chat)
        {
            await _chatRepository.AddChatAsync(chat);
        }

        public async Task<Chat> GetChatByNameAsync(string chatName)
        {
            return await _chatRepository.GetChatByNameAsync(chatName);
        }

        public async Task<Chat> GetChatBetweenUsersAsync(string user1, string user2)
        {
            return await _chatRepository.GetChatBetweenUsersAsync(user1, user2);
        }
    }
}

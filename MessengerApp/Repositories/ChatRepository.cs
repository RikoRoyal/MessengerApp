using MessengerApp.Data;
using MessengerApp.Entities;
using MessengerApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApp.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            return await _context.Chats.Include(c => c.ChatUsers).ThenInclude(cu => cu.User).ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            return await _context.Chats.Include(c => c.Messages).ThenInclude(m => m.User).FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task AddChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task<Chat> GetChatByNameAsync(string chatName)
        {
            return await _context.Chats.FirstOrDefaultAsync(c => c.Name == chatName);
        }

        public async Task<Chat> GetChatBetweenUsersAsync(string user1, string user2)
        {
            return await _context.Chats
                .Include(c => c.ChatUsers)
                .FirstOrDefaultAsync(c => c.ChatUsers.Any(cu => cu.UserId == user1) && c.ChatUsers.Any(cu => cu.UserId == user2));
        }
    }
}

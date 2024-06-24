using MessengerApp.Data;
using MessengerApp.Entities;
using MessengerApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApp.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _context.Messages.Include(m => m.User).ToListAsync();
        }

        public async Task<Message> GetByIdAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task AddAsync(Message entity)
        {
            await _context.Messages.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Message entity)
        {
            _context.Messages.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatId(int chatId)
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId)
                .Include(m => m.User)
                .ToListAsync();
        }

        public async Task RemoveAll(Func<Message, bool> predicate)
        {
            var messagesToRemove = _context.Messages.Where(predicate).ToList();

            if (messagesToRemove.Any())
            {
                _context.Messages.RemoveRange(messagesToRemove);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using MessengerApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task<Message> GetByIdAsync(int id);
        Task AddAsync(Message message);
        Task UpdateAsync(Message message);
        Task DeleteAsync(int id);
        Task<IEnumerable<Message>> GetMessagesByChatId(int chatId);
        Task RemoveAll(Func<Message, bool> predicate);
    }
}

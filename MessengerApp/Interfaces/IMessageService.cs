using MessengerApp.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerApp.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetAllMessagesAsync();
        Task<Message> GetMessageByIdAsync(int id);
        Task AddMessageAsync(Message message);
        Task UpdateMessageAsync(Message message);
        Task DeleteMessageAsync(int id);
        Task<IEnumerable<Message>> GetMessagesByChatId(int chatId);
        Task CleanUpOldMessagesAsync(CancellationToken cancellationToken);
        Task SendPendingNotificationsAsync(CancellationToken cancellationToken);
    }
}

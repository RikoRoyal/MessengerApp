using MessengerApp.Entities;
using MessengerApp.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerApp.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IMessageRepository messageRepository, ILogger<MessageService> logger)
        {
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            return await _messageRepository.GetAllAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _messageRepository.GetByIdAsync(id);
        }

        public async Task AddMessageAsync(Message message)
        {
            await _messageRepository.AddAsync(message);
        }

        public async Task UpdateMessageAsync(Message message)
        {
            await _messageRepository.UpdateAsync(message);
        }

        public async Task DeleteMessageAsync(int id)
        {
            await _messageRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatId(int chatId)
        {
            return await _messageRepository.GetMessagesByChatId(chatId);
        }

        public async Task CleanUpOldMessagesAsync(CancellationToken cancellationToken)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-30);
            await _messageRepository.RemoveAll(m => m.Timestamp < cutoffDate);
            _logger.LogInformation("Cleaned up old messages.");
        }

        public async Task SendPendingNotificationsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending pending notifications...");
            await Task.Delay(5000, cancellationToken); 
            _logger.LogInformation("Pending notifications sent.");
        }
    }
}

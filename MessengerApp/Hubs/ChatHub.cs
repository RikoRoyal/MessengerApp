using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MessengerApp.Interfaces;
using MessengerApp.Entities;
using System;
using System.Collections.Generic;

namespace MessengerApp.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        public ChatHub(IMessageService messageService, IUserService userService, IChatService chatService)
        {
            _messageService = messageService;
            _userService = userService;
            _chatService = chatService;
        }

        public override Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            if (username != null)
            {
                _userConnections[username] = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var username = Context.User.Identity.Name;
            if (username != null)
            {
                _userConnections.TryRemove(username, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string fromUser, string toUser, string message)
        {
            try
            {
                Console.WriteLine($"SendMessage called: fromUser={fromUser}, toUser={toUser}, message={message}");
                var chatId = await EnsureChatExists(fromUser, toUser);

                var sender = await _userService.GetUserByUsernameAsync(fromUser);
                if (sender == null)
                {
                    Console.WriteLine($"Sender not found: {fromUser}");
                    return;
                }

                var newMessage = new Message
                {
                    Text = message,
                    Timestamp = DateTime.Now,
                    UserId = sender.Id,
                    ChatId = chatId
                };

                await _messageService.AddMessageAsync(newMessage);

                if (_userConnections.TryGetValue(toUser, out var recipientConnectionId))
                {
                    await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", fromUser, message);
                }
                await Clients.Caller.SendAsync("ReceiveMessage", fromUser, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessage: {ex.Message}");
                throw;
            }
        }

        private async Task<int> EnsureChatExists(string user1, string user2)
        {
            try
            {
                var chat = await _chatService.GetChatBetweenUsersAsync(user1, user2);
                if (chat == null)
                {
                    var user1Entity = await _userService.GetUserByUsernameAsync(user1);
                    var user2Entity = await _userService.GetUserByUsernameAsync(user2);

                    if (user1Entity == null)
                    {
                        Console.WriteLine($"User not found: {user1}");
                        throw new InvalidOperationException($"User not found: {user1}");
                    }

                    if (user2Entity == null)
                    {
                        Console.WriteLine($"User not found: {user2}");
                        throw new InvalidOperationException($"User not found: {user2}");
                    }

                    chat = new Chat
                    {
                        Name = $"{user1}-{user2}",
                        ChatUsers = new List<ChatUser>
                {
                    new ChatUser { UserId = user1Entity.Id },
                    new ChatUser { UserId = user2Entity.Id }
                }
                    };
                    await _chatService.AddChatAsync(chat);
                }
                return chat.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EnsureChatExists: {ex.Message}");
                throw;
            }
        }
    }
}


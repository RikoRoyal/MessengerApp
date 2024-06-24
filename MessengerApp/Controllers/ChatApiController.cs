using AutoMapper;
using MessengerApp.Entities;
using MessengerApp.Interfaces;
using MessengerApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MessengerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatApiController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ChatApiController(IChatService chatService, IMessageService messageService, IUserService userService, IMapper mapper)
        {
            _chatService = chatService;
            _messageService = messageService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var chats = await _chatService.GetAllChatsAsync();
            var chatViewModels = _mapper.Map<IEnumerable<ChatViewModel>>(chats);
            return Ok(chatViewModels);
        }

        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> GetChatMessages(int chatId)
        {
            var messages = await _messageService.GetMessagesByChatId(chatId);
            var messageViewModels = _mapper.Map<IEnumerable<MessageViewModel>>(messages);
            return Ok(messageViewModels);
        }

        [HttpPost("{chatId}/send")]
        public async Task<IActionResult> SendMessage(int chatId, [FromBody] SendMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chat = await _chatService.GetChatByIdAsync(chatId);
            if (chat == null)
            {
                return NotFound();
            }

            var sender = await _userService.GetUserByUsernameAsync(model.FromUser);
            var recipient = await _userService.GetUserByUsernameAsync(model.ToUser);
            if (sender == null || recipient == null)
            {
                return BadRequest("Invalid users.");
            }

            var newMessage = new Message
            {
                Text = model.Message,
                Timestamp = DateTime.Now,
                UserId = sender.Id,
                ChatId = chat.Id
            };

            await _messageService.AddMessageAsync(newMessage);

            return Ok(new { Success = true });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUser(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return Ok(new { success = false, message = "User not found" });
            }
            var userViewModel = _mapper.Map<UserViewModel>(user);
            return Ok(new { success = true, user = userViewModel });
        }
    }
}

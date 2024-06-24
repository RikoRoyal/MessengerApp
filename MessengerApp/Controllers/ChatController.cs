using AutoMapper;
using MessengerApp.Entities;
using MessengerApp.Interfaces;
using MessengerApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MessengerApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ChatController(IChatService chatService, IMessageService messageService, IUserService userService, IMapper mapper)
        {
            _chatService = chatService;
            _messageService = messageService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var chats = await _chatService.GetAllChatsAsync();
            var chatViewModels = _mapper.Map<IEnumerable<ChatViewModel>>(chats);
            return View(chatViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var chats = await _chatService.GetAllChatsAsync();
            var chatViewModels = _mapper.Map<IEnumerable<ChatViewModel>>(chats);
            return Json(chatViewModels);
        }

        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> GetChatMessages(int chatId)
        {
            var messages = await _messageService.GetMessagesByChatId(chatId);
            var messageViewModels = _mapper.Map<IEnumerable<MessageViewModel>>(messages);
            return Json(messageViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var message = _mapper.Map<Message>(model);
                await _messageService.AddMessageAsync(message);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchUser(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }
            var userViewModel = _mapper.Map<UserViewModel>(user);
            return Json(new { success = true, user = userViewModel });
        }
    }
}

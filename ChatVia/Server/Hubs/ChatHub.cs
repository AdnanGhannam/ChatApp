using AutoMapper;
using ChatVia.Domain.Entities;
using ChatVia.Shared.ResponseDtos;
using Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatVia.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ChatHub(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task SendMessage(string username, string chatId, string message)
        {
            var user = await _context.Users
                   .FirstOrDefaultAsync(u => u.UserName == username);

            var chat = await _context.Chats.FindAsync(chatId);

            if (user is not null && chat is not null)
            {
                var newMessage = new Message(user, chat, message);

                chat.SendMessage(newMessage);
                await _context.SaveChangesAsync();

                await Clients.Group(chatId).SendAsync("ReceiveMessage", 
                    _mapper.Map<AppUserByIdDto>(user), 
                    _mapper.Map<MessageDto>(newMessage));
            }
        }

        public async Task JoinGroup(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
    }
}

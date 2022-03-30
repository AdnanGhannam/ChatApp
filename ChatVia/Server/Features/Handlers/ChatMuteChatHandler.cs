using AutoMapper;
using ChatVia.Domain.Entities;
using ChatVia.Domain.Interfaces;
using ChatVia.Server.Features.Commands;
using ChatVia.Shared.Helpers;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatVia.Server.Features.Handlers
{
    public class ChatMuteChatHandler
        : IRequestHandler<ChatMuteChatCommand, object>
    {
        private readonly IEfRepository<Chat> _repository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ChatMuteChatHandler(IEfRepository<Chat> repository,
            AppDbContext context,
            IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<object> Handle(ChatMuteChatCommand request, 
            CancellationToken cancellationToken)
        {
            if(request is not null)
            {
                if(request is { ChatId: not null })
                {
                    var chat = await _repository.GetByIdAsync(request.ChatId);

                    var user = await _context.Users
                        .Include(u => u.MutedChats)
                        .FirstOrDefaultAsync(u => u.Id == request.UserId);

                    if(chat is not null)
                    {
                        user?.MuteChat(chat);
                        await _repository.SaveChangesAsync();

                        return $"Chat with Id { request.ChatId } has been muted";
                    }

                    return new ErrorModel("NotFound", $"Couldn't find chat with Id { request.ChatId }");
                }

                return new ErrorModel("RequiredField", "ChatId can't be null");
            }

            throw new ArgumentNullException(nameof(request));
        }
    }
}

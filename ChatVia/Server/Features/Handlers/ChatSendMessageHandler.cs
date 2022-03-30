using AutoMapper;
using ChatVia.Domain.Entities;
using ChatVia.Domain.Interfaces;
using ChatVia.Server.Features.Commands;
using ChatVia.Shared.Helpers;
using ChatVia.Shared.ResponseDtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatVia.Server.Features.Handlers
{
    public class ChatSendMessageHandler
        : IRequestHandler<ChatSendMessageCommand, object>
    {
        private readonly IEfRepository<Chat> _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ChatSendMessageHandler(IEfRepository<Chat> repository,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<object> Handle(ChatSendMessageCommand request, 
            CancellationToken cancellationToken)
        {
            if (request is not null)
            {
                if(request is { ChatId: not null, Text: not null })
                {
                    var chat = await _repository.GetByIdAsync(request.ChatId, cancellationToken);
                    var user = await _userManager.FindByIdAsync(request.UserId);

                    if(chat is not null)
                    {
                        var message = new Message(user, chat, request.Text);

                        chat.SendMessage(message);

                        await _repository.SaveChangesAsync();

                        var dto = _mapper.Map<MessageDto>(message);

                        return dto;
                    }

                    return new ErrorModel("NotFound", $"Couldn't find Chat with Id { request.ChatId }");
                }

                return new ErrorModel("RequiredField", "ChatId and Text are required!");
            }

            throw new ArgumentNullException(nameof(request));
        }
    }
}

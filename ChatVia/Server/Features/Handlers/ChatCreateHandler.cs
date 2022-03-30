using AutoMapper;
using ChatVia.Application.Specifications;
using ChatVia.Domain.Entities;
using ChatVia.Domain.Interfaces;
using ChatVia.Server.Features.Commands;
using ChatVia.Shared.Helpers;
using ChatVia.Shared.ResponseDtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatVia.Server.Features.Handlers
{
    public class ChatCreateHandler
        : IRequestHandler<ChatCreateCommand, object>
    {
        private readonly IEfRepository<Chat> _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ChatCreateHandler(IEfRepository<Chat> repository,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<object> Handle(ChatCreateCommand request,
            CancellationToken cancellationToken)
        {
            if(request is not null)
            {
                if (request is { Username: not null })
                {
                    var user = await _userManager.FindByNameAsync(request.Username);
                    var sender = await _userManager.FindByIdAsync(request.SenderId);

                    if(user is not null)
                    {
                        var specification = new GetChatWithMembersSpecifications();

                        if(!await _repository.AnyAsync(c 
                                => c.Members.Contains(user) && c.Members.Contains(sender), 
                            specification, 
                            cancellationToken))
                        {
                            var newChat = new Chat(user, sender);
                            await _repository.AddAsync(newChat);
                            await _repository.SaveChangesAsync();
                        }

                        var chat = await _repository.GetFirstOrDefaultAsync(
                            new GetChatByMembersSpecifications(c => c.Members.Contains(user) 
                                                                    && c.Members.Contains(sender)), 
                            cancellationToken);

                        if(request.Text is not null)
                        {
                            var message = new Message(sender, chat, request.Text);
                            chat.SendMessage(message);
                            await _repository.SaveChangesAsync();
                        }

                        var dto = _mapper.Map<ChatDto>(chat);
                        dto.Member = _mapper.Map<AppUserByIdDto>(
                            chat.Members.FirstOrDefault(m => m.Id != request.SenderId));

                        return dto;
                    }

                    return new ErrorModel("NotFound", $"Couldn't find user with Username { request.Username }");
                }

                return new ErrorModel("RequiedField", "Username is required!");
            }

            throw new ArgumentNullException(nameof(request));
        }
    }
}

using AutoMapper;
using ChatVia.Application.Specifications;
using ChatVia.Domain.Entities;
using ChatVia.Domain.Interfaces;
using ChatVia.Server.Features.Queries;
using ChatVia.Shared.Helpers;
using ChatVia.Shared.ResponseDtos;
using MediatR;

namespace ChatVia.Server.Features.Handlers
{
    public class ChatByIdHandler
        : IRequestHandler<ChatByIdQuery, object>
    {
        private readonly IEfRepository<Chat> _repository;
        private readonly IMapper _mapper;

        public ChatByIdHandler(IEfRepository<Chat> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<object> Handle(ChatByIdQuery request, 
            CancellationToken cancellationToken)
        {
            if(request is not null)
            {
                if(request is { ChatId: not null })
                {
                    var chat = await _repository.GetByIdAsync(
                        request.ChatId,
                        new GetUserChatSpecifications(),
                        cancellationToken);

                    if(chat is not null)
                    {
                        if(chat.Members.Any(m => m.Id == request.UserId))
                        {
                            var dto = _mapper.Map<ChatDto>(chat);

                            dto.Member = _mapper.Map<AppUserByIdDto>(
                                chat.Members.FirstOrDefault(m => m.Id != request.UserId));

                            if(chat.MutedBy.Any(e => e.Id == request.UserId))
                            {
                                dto.IsMuted = true;
                            }

                            return dto;
                        }

                        return new ErrorModel("NoAccessAbility", "You are not a member in this chat");
                    }

                    return new ErrorModel("NotFound", "Chat not found");
                }

                return new ErrorModel("RequiedField", "ChatId can't be null");
            }

            throw new ArgumentNullException(nameof(request));
        }
    }
}

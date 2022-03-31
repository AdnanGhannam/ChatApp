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
    public class ChatGetUserChatsHandler 
        : IRequestHandler<ChatGetUserChatsQuery, object>
    {
        private readonly IEfRepository<Chat> _repository;
        private readonly IMapper _mapper;

        public ChatGetUserChatsHandler(IEfRepository<Chat> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<object> Handle(ChatGetUserChatsQuery request, 
            CancellationToken cancellationToken)
        {
            if(request is not null)
            {
                if(request is { UserId: not null })
                {
                    var chats = await _repository.GetListAsync(
                        new GetUserChatsSpecifications(request.UserId), cancellationToken);

                    var dtos = chats.Select(c =>
                    {
                        ChatListDto dto = new();

                        if(c.Messages?.Count() > 0)
                        {
                            dto = _mapper.Map<ChatListDto>(c);
                        }
                        else
                        {
                            dto.Id = c.Id;
                            dto.CreationTime = c.CreationTime;
                        }

                        dto.Member = _mapper.Map<AppUserByIdDto>(
                            c.Members.First(m => m.Id != request.UserId));

                        return dto;
                    });

                    return dtos;
                }

                return new ErrorModel("UnAuthorized", "Your are not authorized");
            }

            throw new ArgumentNullException(nameof(request));
        }
    }
}

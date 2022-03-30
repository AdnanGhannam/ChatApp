using MediatR;

namespace ChatVia.Server.Features.Commands
{
    public record ChatMuteChatCommand(string ChatId, string? UserId) 
        : IRequest<object>;
}

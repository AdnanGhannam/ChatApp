using MediatR;

namespace ChatVia.Server.Features.Commands
{
    public record ChatUnmuteChatCommand(string ChatId, string? UserId) 
        : IRequest<object>;
}

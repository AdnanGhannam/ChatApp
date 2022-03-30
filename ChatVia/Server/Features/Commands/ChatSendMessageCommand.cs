using MediatR;

namespace ChatVia.Server.Features.Commands
{
    public record ChatSendMessageCommand(string ChatId, string? UserId, string Text) 
        : IRequest<object>;
}

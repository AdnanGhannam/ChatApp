using MediatR;

namespace ChatVia.Server.Features.Commands
{
    public record ChatCreateCommand(string Username, string? SenderId, string? Text) 
        : IRequest<object>;
}

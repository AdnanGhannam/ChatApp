using MediatR;

namespace ChatVia.Server.Features.Commands
{
    public record ContactRemoveCommand(string ContactName, string UserId) : IRequest<object>;
}

using MediatR;

namespace ChatVia.Server.Features.Commands
{
    public record AppUserAddContactCommand(string? ContactName, string? UserId) 
        : IRequest<object>;
}

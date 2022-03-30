using MediatR;

namespace ChatVia.Server.Features.Commands
{
    public record AppUserLogoutCommand() : IRequest<object>;
}

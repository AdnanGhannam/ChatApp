using ChatVia.Shared.RequestDtos;
using MediatR;

namespace ChatVia.Server.Features.Commands
{
    public record AppUserRegisterCommand(AppUserRegisterDto User) 
        : IRequest<object>;
}

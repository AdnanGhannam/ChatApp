using MediatR;

namespace ChatVia.Server.Features.Commands;

public record AppUserLoginCommand(string UserName, string Password, bool IsPersistent) 
    : IRequest<object>;

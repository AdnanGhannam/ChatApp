using MediatR;

namespace ChatVia.Server.Features.Queries
{
    public record AppUserByIdQuery(string UserId) 
        : IRequest<object>;
}

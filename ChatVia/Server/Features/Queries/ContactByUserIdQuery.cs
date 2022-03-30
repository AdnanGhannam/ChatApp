using MediatR;

namespace ChatVia.Server.Features.Queries
{
    public record ContactByUserIdQuery(string? UserId) 
        : IRequest<object>;
}

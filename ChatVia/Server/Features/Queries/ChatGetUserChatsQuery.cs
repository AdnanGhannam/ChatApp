using MediatR;

namespace ChatVia.Server.Features.Queries
{
    public record ChatGetUserChatsQuery(string? UserId) : IRequest<object>;
}

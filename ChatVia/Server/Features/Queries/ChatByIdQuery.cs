using MediatR;

namespace ChatVia.Server.Features.Queries
{
    public record ChatByIdQuery(string ChatId, string? UserId) : IRequest<object>;
}

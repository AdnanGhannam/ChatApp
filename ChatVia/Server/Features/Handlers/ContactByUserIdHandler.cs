using AutoMapper;
using ChatVia.Domain.Entities;
using ChatVia.Domain.Interfaces;
using ChatVia.Server.Features.Queries;
using ChatVia.Shared.Helpers;
using ChatVia.Shared.ResponseDtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatVia.Server.Features.Handlers
{
    public class ContactByUserIdHandler
        : IRequestHandler<ContactByUserIdQuery, object>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactByUserIdHandler(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<object> Handle(ContactByUserIdQuery request, 
            CancellationToken cancellationToken)
        {
            if(request is not null)
            {
                var contacts = _context.Users
                    .IgnoreAutoIncludes()
                    .Include(e => e.Contacts)
                    .FirstOrDefault(e => e.Id == request.UserId)?
                    .Contacts;

                if(contacts is not null)
                {
                    var dtos = contacts.Select(e => _mapper.Map<AppUserListDto>(e));
                    return dtos;
                }

                return new ErrorModel("EmptyResponse", "no contacts");
            }

            throw new ArgumentNullException(nameof(request));
        }
    }
}

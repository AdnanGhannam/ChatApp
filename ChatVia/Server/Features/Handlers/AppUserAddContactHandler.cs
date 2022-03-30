using AutoMapper;
using ChatVia.Domain.Entities;
using ChatVia.Server.Features.Commands;
using ChatVia.Shared.Helpers;
using ChatVia.Shared.ResponseDtos;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatVia.Server.Features.Handlers
{
    public class AppUserAddContactHandler
        : IRequestHandler<AppUserAddContactCommand, object>
    {
        private readonly AppDbContext _context;
        public readonly IMapper _mapper;

        public AppUserAddContactHandler(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<object> Handle(AppUserAddContactCommand request, 
            CancellationToken cancellationToken)
        {
            if(request is not null)
            {
                if(request is { ContactName: not null })
                {
                    var contact = await _context.Users
                        .FirstOrDefaultAsync(u => u.UserName == request.ContactName);

                    if(contact is not null)
                    {
                        var user = await _context.Users
                            .Include(u => u.Contacts)
                            .FirstOrDefaultAsync(u => u.Id == request.UserId);

                        if(user is not null && !user.Contacts.Any(c => c.Id == contact.Id))
                        {
                            user.AddContact(contact);

                            await _context.SaveChangesAsync();

                            var dto = _mapper.Map<AppUserListDto>(contact);

                            return dto;
                        }

                        return new ErrorModel("AlreadyExisted", $"User with username: { request.ContactName } is already existed");
                    }

                    return new ErrorModel("NotFound", $"User with username: { request.ContactName } is not found");
                }

                return new ErrorModel("RequiredField", "ContactName is required!");
            }

            throw new ArgumentNullException(nameof(request));
        }
    }
}

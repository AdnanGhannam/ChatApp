using AutoMapper;
using ChatVia.Server.Features.Commands;
using ChatVia.Shared.Helpers;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatVia.Server.Features.Handlers
{
    public class ContactRemoveHandler
        : IRequestHandler<ContactRemoveCommand, object>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactRemoveHandler(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<object> Handle(ContactRemoveCommand request, 
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

                        user?.RemoveContact(contact);

                        await _context.SaveChangesAsync();

                        return $"Contact with username: { contact.UserName } has been removed!";
                    }

                    return new ErrorModel("NotFound", $"User with username: { request.ContactName } not found!");
                }

                return new ErrorModel("RequiredField", "ContactName is required!");
            }

            throw new ArgumentNullException(nameof(request));
        }
    }
}

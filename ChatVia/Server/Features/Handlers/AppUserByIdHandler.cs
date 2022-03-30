using AutoMapper;
using ChatVia.Domain.Entities;
using ChatVia.Domain.Enums;
using ChatVia.Server.Features.Queries;
using ChatVia.Shared.Helpers;
using ChatVia.Shared.ResponseDtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatVia.Server.Features.Handlers
{
    public class AppUserByIdHandler : IRequestHandler<AppUserByIdQuery, object>
    {
        private readonly UserManager<AppUser> _userManager;

        public readonly IMapper _mapper;

        public AppUserByIdHandler(UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<object> Handle(AppUserByIdQuery request, 
            CancellationToken cancellationToken)
        {
            if (request is not null)
            {
                if(request is { UserId: not null })
                {
                    var user = await _userManager.FindByIdAsync(request.UserId);

                    if(user is not null)
                    {
                        AppUserByIdDto responseUser = _mapper.Map<AppUserByIdDto>(user);

                        return responseUser;
                    }

                    return new ErrorModel("NotFound", $"User with Id: {request.UserId} is not found");
                }

                return new ErrorModel("RequiredField", "UserId can't be null");
            }

            throw new ArgumentNullException(nameof(request));

        }

        private void ModifyResponseUser(AppUser user, AppUser sender)
        {
        }
    }
}

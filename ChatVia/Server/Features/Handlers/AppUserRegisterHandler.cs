using ChatVia.Domain.Entities;
using ChatVia.Server.Features.Commands;
using ChatVia.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatVia.Server.Features.Handlers
{
    public class AppUserRegisterHandler : IRequestHandler<AppUserRegisterCommand, object>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AppUserRegisterHandler(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<object> Handle(AppUserRegisterCommand request, 
            CancellationToken cancellationToken)
        {
            if(request is not null)
            {
                if(request is { User: not null })
                {
                    var requestUser = request.User;
                    var user = new AppUser(
                            requestUser.UserName, requestUser.Email, requestUser.Bio, requestUser.Location);

                    var results = await _userManager.CreateAsync(user, requestUser.Password);

                    if (results.Succeeded)
                    {
                        return "Register succeeded!";
                    }

                    var errors = new List<ErrorModel>();

                    foreach(var error in results.Errors)
                    {
                        errors.Add(new(error.Code, error.Description));
                    }

                    return errors;
                }

                return new ErrorModel("BodyRequired", "Request body can't be empty!");
            }

            throw new ArgumentNullException(nameof(request));
        }
    }
}

using ChatVia.Domain.Entities;
using ChatVia.Server.Features.Commands;
using ChatVia.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatVia.Server.Features.Handlers;

public class AppUserLoginHandler : IRequestHandler<AppUserLoginCommand, object>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AppUserLoginHandler(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<object> Handle(AppUserLoginCommand request, CancellationToken cancellationToken)
    {
        if (request is not null)
        {
            if(request is { UserName: not null, Password: not null })
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user is not null)
                {
                    var results = await _signInManager.PasswordSignInAsync(
                                            user, request.Password, request.IsPersistent, false);

                    if (results.Succeeded)
                    {
                        return user.UserName;
                    }

                    return new ErrorModel("WrondPassword", "Login failed, Password is wrong!");
                }

                return new ErrorModel("NotFound", "User not found!");
            }

            return new ErrorModel("RequiredField", "Either the 'UserName' Or the 'Password' is null!");
        }
        
        throw new ArgumentNullException(nameof(request));
    }
}

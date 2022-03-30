using ChatVia.Domain.Entities;
using ChatVia.Server.Features.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatVia.Server.Features.Handlers
{
    public class AppUserLogoutHandler : IRequestHandler<AppUserLogoutCommand, object>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AppUserLogoutHandler(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<object> Handle(AppUserLogoutCommand request, 
            CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return "Logged out";
        }
    }
}

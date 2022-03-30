using ChatVia.Server.Features.Commands;
using ChatVia.Server.Features.Queries;
using ChatVia.Shared.ControllersRoutes;
using ChatVia.Shared.Helpers;
using ChatVia.Shared.RequestDtos;
using ChatVia.Shared.ResponseDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatVia.Server.Controllers;

[ApiController]
[Route(AppUserRoutes.Root)]
public class AppUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Endpoints

    [HttpPost(AppUserRoutes.LoginRoute)]
    public async Task<IActionResult> Login([FromHeader] string username,
        [FromHeader] string password,
        [FromHeader] bool isPersistent = false)
    {
        var results = await _mediator.Send(new AppUserLoginCommand(username, password, isPersistent));

        return results switch 
        {
            string message => Ok(new ResponseModel<string>(message, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [Authorize]
    [HttpPost(AppUserRoutes.LogoutRoute)]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new AppUserLogoutCommand());

        return Ok();
    }

    [HttpPost(AppUserRoutes.RegisterRoute)]
    public async Task<IActionResult> Register([FromBody] AppUserRegisterDto user)
    {
        var results = await _mediator.Send(new AppUserRegisterCommand(user));

        return results switch
        {
            string message => Ok(new ResponseModel<string>(message, error: null)),
            List<ErrorModel> errors 
                => BadRequest(new ResponseModel<object>(null, errors: errors)),
            ErrorModel error 
                => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [Authorize]
    [HttpGet(AppUserRoutes.ByIdRoute)]
    public async Task<IActionResult> GetById([FromQuery] string? userId)
    {
        var results = await _mediator.Send(
            new AppUserByIdQuery(userId ?? User.FindFirstValue(ClaimTypes.NameIdentifier)));

        return results switch
        {
            AppUserByIdDto dto => Ok(new ResponseModel<AppUserByIdDto>(dto, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [Authorize]
    [HttpGet(AppUserRoutes.GetContacts)]
    public async Task<IActionResult> GetContacts()
    {
        var results = await _mediator.Send(
            new ContactByUserIdQuery(User.FindFirstValue(ClaimTypes.NameIdentifier)));

        return results switch
        {
            IEnumerable<AppUserListDto> dtos 
                => Ok(new ResponseModel<IEnumerable<AppUserListDto>>(dtos, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [Authorize]
    [HttpPost(AppUserRoutes.AddContact)]
    public async Task<IActionResult> AddContect([FromHeader] string? contactName)
    {
        var results = await _mediator.Send(
            new AppUserAddContactCommand(contactName, User.FindFirstValue(ClaimTypes.NameIdentifier)));

        return results switch
        {
            AppUserListDto dto => Ok(new ResponseModel<AppUserListDto>(dto, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [Authorize]
    [HttpPost(AppUserRoutes.RemoveContact)]
    public async Task<IActionResult> RemoveContact([FromHeader] string? contactName)
    {
        var results = await _mediator.Send(
            new ContactRemoveCommand(contactName, User.FindFirstValue(ClaimTypes.NameIdentifier)));

        return results switch
        {
            string message => Ok(new ResponseModel<string>(message, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }
    #endregion
}

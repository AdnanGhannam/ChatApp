using ChatVia.Server.Features.Commands;
using ChatVia.Server.Features.Queries;
using ChatVia.Shared.ControllersRoutes;
using ChatVia.Shared.Helpers;
using ChatVia.Shared.ResponseDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatVia.Server.Controllers;

[ApiController]
[Route(ChatsRoutes.Root)]
[Authorize]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Endpoints

    [HttpGet(ChatsRoutes.ByUserId)]
    public async Task<IActionResult> GetByUserId()
    {
        var results = await _mediator.Send(
            new ChatGetUserChatsQuery(User.FindFirstValue(ClaimTypes.NameIdentifier)));

        return results switch
        {
            IEnumerable<ChatListDto> dtos 
                => Ok(new ResponseModel<IEnumerable<ChatListDto>>(dtos, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [HttpGet(ChatsRoutes.ById)]
    public async Task<IActionResult> GetById([FromQuery] string? chatId)
    {
        var results = await _mediator.Send(
            new ChatByIdQuery(chatId, User.FindFirstValue(ClaimTypes.NameIdentifier)));

        return results switch
        {
            ChatDto dto => Ok(new ResponseModel<ChatDto>(dto, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [HttpPost(ChatsRoutes.SendMessage)]
    public async Task<IActionResult> SendMessage([FromHeader] string chatId,
        [FromHeader] string text)
    {
        var results = await _mediator.Send(
            new ChatSendMessageCommand(chatId, User.FindFirstValue(ClaimTypes.NameIdentifier), text));

        return results switch
        {
            MessageDto dto => Ok(new ResponseModel<MessageDto>(dto, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [HttpPost(ChatsRoutes.CreateChat)]
    public async Task<IActionResult> CreateChat([FromHeader] string? username,
        [FromHeader] string? text)
    {
        var results = await _mediator.Send(
            new ChatCreateCommand(username, User.FindFirstValue(ClaimTypes.NameIdentifier), text));

        return results switch
        {
            ChatDto dto => Ok(new ResponseModel<ChatDto>(dto, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [HttpPost(ChatsRoutes.MuteChat)]
    public async Task<IActionResult> MuteChat([FromHeader] string chatId)
    {
        var results = await _mediator.Send(
            new ChatMuteChatCommand(chatId, User.FindFirstValue(ClaimTypes.NameIdentifier)));

        return results switch
        {
            string message => Ok(new ResponseModel<string>(message, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }

    [HttpPost(ChatsRoutes.UnmuteChat)]
    public async Task<IActionResult> UnmuteChat([FromHeader] string chatId)
    {
        var results = await _mediator.Send(
            new ChatUnmuteChatCommand(chatId, User.FindFirstValue(ClaimTypes.NameIdentifier)));

        return results switch
        {
            string message => Ok(new ResponseModel<string>(message, error: null)),
            ErrorModel error => BadRequest(new ResponseModel<object>(null, error: error)),
            _ => BadRequest(new ResponseModel<object>(null, error: new("Unknown", "Unknown error")))
        };
    }
    #endregion
}

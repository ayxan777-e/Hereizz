using API.Controllers.Common;
using Application.Commands.Auth.Login;
using Application.Commands.Auth.Logout;
using Application.Commands.Auth.RefreshToken;
using Application.Queries.Auth.GetProfile;
using Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResponse(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResponse(result);
    }


    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResponse(result);
    }
    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var result = await _mediator.Send(new GetProfileQuery());
        return HandleResponse(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(LogoutCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResponse(result);
    }


}
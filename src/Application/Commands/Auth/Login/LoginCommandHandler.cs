using Application.DTOs.Auth;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Auth.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, BaseResponse<AuthResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<BaseResponse<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        User? user;

        // Email yox username?
        if (request.EmailOrUserName.Contains("@"))
        {
            user = await _userManager.FindByEmailAsync(request.EmailOrUserName);
        }
        else
        {
            user = await _userManager.FindByNameAsync(request.EmailOrUserName);
        }

        if (user is null)
        {
            return BaseResponse<AuthResponse>.Fail(
                "Email/Username or password is incorrect",
                new List<string> { "Invalid credentials." },
                ErrorType.Unauthorized);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            return BaseResponse<AuthResponse>.Fail(
                "Email/Username or password is incorrect",
                new List<string> { "Invalid credentials." },
                ErrorType.Unauthorized);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtTokenService.CreateToken(user, roles);

        return BaseResponse<AuthResponse>.Ok(token, "Login successful");
    }
}
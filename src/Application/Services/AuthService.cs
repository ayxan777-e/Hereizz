using Application.DTOs.Auth;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenService jwtTokenService,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<BaseResponse<AuthResponse>> LoginAsync(string emailOrUserName, string password, CancellationToken ct)
    {
        _logger.LogInformation("Login requested. Identity={Identity}", emailOrUserName);

        User? user = emailOrUserName.Contains("@")
            ? await _userManager.FindByEmailAsync(emailOrUserName)
            : await _userManager.FindByNameAsync(emailOrUserName);

        if (user is null)
        {
            _logger.LogWarning("Login failed. Identity={Identity} was not found", emailOrUserName);
            return BaseResponse<AuthResponse>.Fail(
                "Email/Username or password is incorrect",
                new List<string> { "Invalid credentials." },
                ErrorType.Unauthorized);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Login failed. Invalid password for UserId={UserId}", user.Id);
            return BaseResponse<AuthResponse>.Fail(
                "Email/Username or password is incorrect",
                new List<string> { "Invalid credentials." },
                ErrorType.Unauthorized);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.CreateToken(user, roles);

        _logger.LogInformation("Login succeeded. UserId={UserId}", user.Id);

        return BaseResponse<AuthResponse>.Ok(token, "Login successful");
    }

    public async Task<BaseResponse> RegisterAsync(string fullName, string userName, string email, string password, CancellationToken ct)
    {
        _logger.LogInformation("Register requested. UserName={UserName}, Email={Email}", userName, email);

        var existingEmail = await _userManager.FindByEmailAsync(email);
        if (existingEmail is not null)
        {
            _logger.LogWarning("Register rejected. Email={Email} already exists", email);
            return BaseResponse.Fail(
                "Email already exists",
                new List<string> { "This email is already in use." },
                ErrorType.Conflict);
        }

        var existingUserName = await _userManager.FindByNameAsync(userName);
        if (existingUserName is not null)
        {
            _logger.LogWarning("Register rejected. UserName={UserName} already exists", userName);
            return BaseResponse.Fail(
                "Username already exists",
                new List<string> { "This username is already in use." },
                ErrorType.Conflict);
        }

        var user = new User
        {
            FullName = fullName,
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            _logger.LogWarning("Register failed for UserName={UserName}. Errors={Errors}", userName, string.Join(" | ", errors));
            return BaseResponse.Fail(
                "Register failed",
                errors,
                ErrorType.Validation);
        }

        _logger.LogInformation("Register succeeded. UserId={UserId}, UserName={UserName}", user.Id, userName);

        return BaseResponse.Ok("Register successful");
    }
}

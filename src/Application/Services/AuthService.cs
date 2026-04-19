using Application.DTOs.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Options;
using Application.Shared.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AuthService> _logger;
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly EmailOptions _emailOptions;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenService jwtTokenService,
        ILogger<AuthService> logger,
        IApplicationDbContext context,
        IEmailService emailService,
        IOptions<EmailOptions> emailOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
        _context = context;
        _emailService = emailService;
        _emailOptions = emailOptions.Value;
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

        if (!user.EmailConfirmed)
        {
            _logger.LogWarning("Login blocked. Email not confirmed for UserId={UserId}", user.Id);
            return BaseResponse<AuthResponse>.Fail(
                "Email is not confirmed",
                new List<string> { "Please confirm your email before logging in." },
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

        var activeRefreshTokens = await _context.RefreshTokens
            .Where(x => x.UserId == user.Id && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(ct);

        foreach (var activeToken in activeRefreshTokens)
        {
            activeToken.IsRevoked = true;
        }

        if (activeRefreshTokens.Count > 0)
        {
            _logger.LogInformation(
                "Revoked {Count} active refresh token(s) for UserId={UserId} during login",
                activeRefreshTokens.Count,
                user.Id);
        }

        var accessToken = _jwtTokenService.CreateAccessToken(user, roles);
        var accessExpire = _jwtTokenService.GetAccessTokenExpireAt();

        var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();
        var refreshExpire = _jwtTokenService.GetRefreshTokenExpireAt();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = refreshExpire,
            IsRevoked = false
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(ct);

        var response = new AuthResponse
        {
            AccessToken = accessToken,
            AccessTokenExpireAt = accessExpire,
            RefreshToken = refreshTokenValue,
            RefreshTokenExpireAt = refreshExpire,
            UserId = user.Id,
            FullName = user.FullName,
            UserName = user.UserName ?? "",
            Email = user.Email ?? ""
        };

        _logger.LogInformation("Login succeeded with refresh token. UserId={UserId}", user.Id);

        return BaseResponse<AuthResponse>.Ok(response, "Login successful");
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

        var addToRoleResult = await _userManager.AddToRoleAsync(user, Domain.Constants.Roles.User);

        if (!addToRoleResult.Succeeded)
        {
            var roleErrors = addToRoleResult.Errors.Select(x => x.Description).ToList();

            _logger.LogWarning("User created but assigning User role failed. UserId={UserId}, Errors={Errors}",
                user.Id,
                string.Join(" | ", roleErrors));

            return BaseResponse.Fail(
                "Register succeeded but role assignment failed",
                roleErrors,
                ErrorType.ServerError);
        }

        await SendConfirmationEmailAsync(user, ct);

        _logger.LogInformation("Register succeeded. UserId={UserId}, UserName={UserName}", user.Id, userName);

        return BaseResponse.Ok("Register successful. Please confirm your email.");
    }

    public async Task<BaseResponse> ConfirmEmailAsync(string userId, string token, CancellationToken ct)
    {
        _logger.LogInformation("Email confirmation requested. UserId={UserId}", userId);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return BaseResponse.Fail(
                "Invalid confirmation request",
                new List<string> { "User was not found." },
                ErrorType.BadRequest);
        }

        if (user.EmailConfirmed)
        {
            return BaseResponse.Ok("Email is already confirmed");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            return BaseResponse.Fail(
                "Email confirmation failed",
                errors,
                ErrorType.BadRequest);
        }

        return BaseResponse.Ok("Email confirmed successfully");
    }

    public async Task<BaseResponse> ResendConfirmationEmailAsync(string email, CancellationToken ct)
    {
        _logger.LogInformation("Resend email confirmation requested. Email={Email}", email);

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return BaseResponse.Ok("If this email exists, a confirmation email has been sent");
        }

        if (user.EmailConfirmed)
        {
            return BaseResponse.Ok("Email is already confirmed");
        }

        await SendConfirmationEmailAsync(user, ct);

        return BaseResponse.Ok("Confirmation email sent");
    }

    public async Task<BaseResponse<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        _logger.LogInformation("Refresh token requested");

        var storedRefreshToken = await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == refreshToken, ct);

        if (storedRefreshToken is null)
        {
            _logger.LogWarning("Refresh token failed. Token not found");
            return BaseResponse<AuthResponse>.Fail(
                "Invalid refresh token",
                new List<string> { "Refresh token is invalid." },
                ErrorType.Unauthorized);
        }

        if (storedRefreshToken.IsRevoked)
        {
            _logger.LogWarning("Refresh token failed. Token is revoked. RefreshTokenId={RefreshTokenId}", storedRefreshToken.Id);
            return BaseResponse<AuthResponse>.Fail(
                "Invalid refresh token",
                new List<string> { "Refresh token is revoked." },
                ErrorType.Unauthorized);
        }

        if (storedRefreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            _logger.LogWarning("Refresh token failed. Token expired. RefreshTokenId={RefreshTokenId}", storedRefreshToken.Id);
            return BaseResponse<AuthResponse>.Fail(
                "Refresh token expired",
                new List<string> { "Refresh token has expired." },
                ErrorType.Unauthorized);
        }

        var user = storedRefreshToken.User;

        var roles = await _userManager.GetRolesAsync(user);

        var newAccessToken = _jwtTokenService.CreateAccessToken(user, roles);
        var newAccessExpireAt = _jwtTokenService.GetAccessTokenExpireAt();

        var newRefreshTokenValue = _jwtTokenService.GenerateRefreshToken();
        var newRefreshExpireAt = _jwtTokenService.GetRefreshTokenExpireAt();

        storedRefreshToken.IsRevoked = true;

        var newRefreshToken = new RefreshToken
        {
            Token = newRefreshTokenValue,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = newRefreshExpireAt,
            IsRevoked = false
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync(ct);

        var response = new AuthResponse
        {
            AccessToken = newAccessToken,
            AccessTokenExpireAt = newAccessExpireAt,
            RefreshToken = newRefreshTokenValue,
            RefreshTokenExpireAt = newRefreshExpireAt,
            UserId = user.Id,
            FullName = user.FullName,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty
        };

        _logger.LogInformation("Refresh token succeeded. UserId={UserId}", user.Id);

        return BaseResponse<AuthResponse>.Ok(response, "Token refreshed successfully");
    }

    public async Task<BaseResponse> LogoutAsync(string refreshToken, CancellationToken ct)
    {
        _logger.LogInformation("Logout requested");

        var storedRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken, ct);

        if (storedRefreshToken is null)
        {
            _logger.LogWarning("Logout failed. Token not found");
            return BaseResponse.Fail(
                "Invalid refresh token",
                new List<string> { "Refresh token not found." },
                ErrorType.Unauthorized);
        }

        if (storedRefreshToken.IsRevoked)
        {
            _logger.LogWarning("Logout failed. Token already revoked. TokenId={TokenId}", storedRefreshToken.Id);
            return BaseResponse.Fail(
                "Token already revoked",
                new List<string> { "Refresh token already revoked." },
                ErrorType.BadRequest);
        }

        storedRefreshToken.IsRevoked = true;

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Logout succeeded. TokenId={TokenId}", storedRefreshToken.Id);

        return BaseResponse.Ok("Logged out successfully");
    }

    private async Task SendConfirmationEmailAsync(User user, CancellationToken ct)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebUtility.UrlEncode(token);
        var confirmationLink = $"{_emailOptions.ConfirmationBaseUrl}?userId={user.Id}&token={encodedToken}";

        var body = $"Please confirm your email by clicking this link: <a href=\"{confirmationLink}\">Confirm Email</a>";
        await _emailService.SendEmailAsync(user.Email!, "Confirm your email", body, ct);
    }
}

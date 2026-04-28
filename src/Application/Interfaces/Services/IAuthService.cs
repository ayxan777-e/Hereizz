using Application.DTOs.Auth;
using Application.Shared.Responses;

namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task<BaseResponse<AuthResponse>> LoginAsync(string emailOrUserName, string password, CancellationToken ct);
    Task<BaseResponse> RegisterAsync(string fullName, string userName, string email, string password, CancellationToken ct);
    Task<BaseResponse<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct);
    Task<BaseResponse> LogoutAsync(string refreshToken, CancellationToken ct);
    Task<BaseResponse> ConfirmEmailAsync(string userId, string token, CancellationToken ct);
    Task<BaseResponse> ResendConfirmationEmailAsync(string email, CancellationToken ct);
    Task<bool> IsEmailConfirmedAsync(string email, CancellationToken cancellationToken);
}

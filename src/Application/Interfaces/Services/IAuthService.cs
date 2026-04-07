using Application.DTOs.Auth;
using Application.Shared.Responses;

namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task<BaseResponse<AuthResponse>> LoginAsync(string emailOrUserName, string password, CancellationToken ct);
    Task<BaseResponse> RegisterAsync(string fullName, string userName, string email, string password, CancellationToken ct);
}

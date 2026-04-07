using Application.DTOs.Auth;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommand:IRequest<BaseResponse<AuthResponse>>
{
    public string RefreshToken { get; set; } = null!;
}

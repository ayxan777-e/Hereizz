using Application.DTOs.Auth;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Auth.Login;

public class LoginCommand : IRequest<BaseResponse<AuthResponse>>
{
    public string EmailOrUserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Auth.Logout;

public class LogoutCommand : IRequest<BaseResponse>
{
    public string RefreshToken { get; set; } = null!;
}
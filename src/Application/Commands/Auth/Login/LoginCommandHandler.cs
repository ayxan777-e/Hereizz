using Application.DTOs.Auth;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse<AuthResponse>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<BaseResponse<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        return _authService.LoginAsync(request.EmailOrUserName, request.Password, ct);
    }
}

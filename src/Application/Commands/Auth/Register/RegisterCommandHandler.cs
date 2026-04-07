using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseResponse>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<BaseResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        return _authService.RegisterAsync(request.FullName, request.UserName, request.Email, request.Password, ct);
    }
}

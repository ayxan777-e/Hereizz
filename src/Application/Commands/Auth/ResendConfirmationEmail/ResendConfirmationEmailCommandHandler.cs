using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Auth.ResendConfirmationEmail;

public class ResendConfirmationEmailCommandHandler : IRequestHandler<ResendConfirmationEmailCommand, BaseResponse>
{
    private readonly IAuthService _authService;

    public ResendConfirmationEmailCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<BaseResponse> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ResendConfirmationEmailAsync(request.Email, cancellationToken);
    }
}

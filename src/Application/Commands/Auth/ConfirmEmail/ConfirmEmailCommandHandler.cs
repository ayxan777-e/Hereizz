using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Auth.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, BaseResponse>
{
    private readonly IAuthService _authService;

    public ConfirmEmailCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<BaseResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ConfirmEmailAsync(request.UserId, request.Token, cancellationToken);
    }
}

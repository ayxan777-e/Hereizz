using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Auth.ResendConfirmationEmail;

public class ResendConfirmationEmailCommand : IRequest<BaseResponse>
{
    public string Email { get; set; } = null!;
}

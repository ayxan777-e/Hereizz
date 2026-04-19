using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Auth.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<BaseResponse>
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
}

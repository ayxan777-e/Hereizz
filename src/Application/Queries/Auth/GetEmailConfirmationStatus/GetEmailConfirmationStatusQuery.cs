using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Auth.GetEmailConfirmationStatus;

public class GetEmailConfirmationStatusQuery : IRequest<BaseResponse<bool>>
{
    public string Email { get; set; } = null!;
}
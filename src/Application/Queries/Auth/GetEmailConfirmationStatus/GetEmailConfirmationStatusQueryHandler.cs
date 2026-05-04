using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Auth.GetEmailConfirmationStatus;

public class GetEmailConfirmationStatusQueryHandler
    : IRequestHandler<GetEmailConfirmationStatusQuery, BaseResponse<bool>>
{
    private readonly IAuthService _authService;

    public GetEmailConfirmationStatusQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<BaseResponse<bool>> Handle(
        GetEmailConfirmationStatusQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BaseResponse<bool>.Fail(
                "Email is required",
                new List<string> { "Email cannot be empty." },
                ErrorType.Validation);
        }

        var isConfirmed = await _authService.IsEmailConfirmedAsync(
            request.Email,
            cancellationToken);

        return BaseResponse<bool>.Ok(
            isConfirmed,
            "Email confirmation status fetched successfully");
    }
}
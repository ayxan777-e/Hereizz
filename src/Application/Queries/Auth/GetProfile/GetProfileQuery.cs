using Application.DTOs.Auth;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Auth.GetProfile;

public class GetProfileQuery : IRequest<BaseResponse<UserProfileDto>>
{
}
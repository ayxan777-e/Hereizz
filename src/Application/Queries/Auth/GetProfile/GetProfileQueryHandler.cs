using Application.DTOs.Auth;
using Application.Shared.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Queries.Auth.GetProfile;

public class GetProfileQueryHandler
    : IRequestHandler<GetProfileQuery, BaseResponse<UserProfileDto>>
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetProfileQueryHandler(
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<UserProfileDto>> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            return BaseResponse<UserProfileDto>.Fail(
                "Unauthorized",
                new List<string> { "User context is missing." },
                ErrorType.Unauthorized);
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return BaseResponse<UserProfileDto>.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
        }

        var dbUser = await _userManager.FindByIdAsync(userId);

        if (dbUser is null)
        {
            return BaseResponse<UserProfileDto>.Fail(
                "User not found",
                new List<string> { "Authenticated user does not exist." },
                ErrorType.NotFound);
        }

        var dto = new UserProfileDto
        {
            Id = dbUser.Id,
            FullName = dbUser.FullName,
            UserName = dbUser.UserName!,
            Email = dbUser.Email!
        };

        return BaseResponse<UserProfileDto>.Ok(dto, "User fetched successfully");
    }
}
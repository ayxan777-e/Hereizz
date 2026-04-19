using Application.Shared.Responses;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Helpers;

public static class UserContextHelper
{
    public static bool TryGetUserId(
        IHttpContextAccessor httpContextAccessor,
        out string userId,
        out BaseResponse? errorResponse)
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            userId = string.Empty;
            errorResponse = BaseResponse.Fail(
                "Unauthorized",
                new List<string> { "User context is missing." },
                ErrorType.Unauthorized);
            return false;
        }

        userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        if (string.IsNullOrWhiteSpace(userId))
        {
            errorResponse = BaseResponse.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
            return false;
        }

        errorResponse = null;
        return true;
    }

    public static bool TryGetUserId<T>(
        IHttpContextAccessor httpContextAccessor,
        out string userId,
        out BaseResponse<T>? errorResponse)
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            userId = string.Empty;
            errorResponse = BaseResponse<T>.Fail(
                "Unauthorized",
                new List<string> { "User context is missing." },
                ErrorType.Unauthorized);
            return false;
        }

        userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        if (string.IsNullOrWhiteSpace(userId))
        {
            errorResponse = BaseResponse<T>.Fail(
                "Unauthorized",
                new List<string> { "User ID not found in token." },
                ErrorType.Unauthorized);
            return false;
        }

        errorResponse = null;
        return true;
    }
}

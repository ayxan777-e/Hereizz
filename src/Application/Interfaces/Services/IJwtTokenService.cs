using Application.DTOs.Auth;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IJwtTokenService
{
    string CreateAccessToken(User user, IList<string> roles);
    DateTime GetAccessTokenExpireAt();
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpireAt();
}
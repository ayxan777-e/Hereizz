using Application.DTOs.Auth;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IJwtTokenService
{
    AuthResponse CreateToken(User user, IList<string> roles);
}
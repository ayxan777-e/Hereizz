namespace Application.DTOs.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; } = null!;
    public DateTime AccessTokenExpireAt { get; set; }

    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpireAt { get; set; }

    public string UserId { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}
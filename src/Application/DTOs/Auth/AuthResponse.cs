namespace Application.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = null!;
    public DateTime ExpireAt { get; set; }
    public string UserId { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}
namespace Application.DTOs.Auth;

public class LoginRequest
{
    public string EmailOrUserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
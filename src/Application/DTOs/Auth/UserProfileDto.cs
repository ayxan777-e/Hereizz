namespace Application.DTOs.Auth;

public class UserProfileDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}
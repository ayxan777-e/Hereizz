using Application.DTOs.Auth;
using Application.Shared.Responses;
using MediatR;

public class RegisterCommand : IRequest<BaseResponse>
{
    public string FullName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
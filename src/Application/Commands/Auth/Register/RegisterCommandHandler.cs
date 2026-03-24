using Application.Shared.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, BaseResponse>
{
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<BaseResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail is not null)
        {
            return BaseResponse.Fail(
                "Email already exists",
                new List<string> { "This email is already in use." },
                ErrorType.Conflict);
        }

        var existingUserName = await _userManager.FindByNameAsync(request.UserName);
        if (existingUserName is not null)
        {
            return BaseResponse.Fail(
                "Username already exists",
                new List<string> { "This username is already in use." },
                ErrorType.Conflict);
        }

        var user = new User
        {
            FullName = request.FullName,
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();

            return BaseResponse.Fail(
                "Register failed",
                errors,
                ErrorType.Validation);
        }

        return BaseResponse.Ok("Register successful");
    }
}
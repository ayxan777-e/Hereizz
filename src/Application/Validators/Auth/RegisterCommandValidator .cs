using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Validators.Auth;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly UserManager<User> _userManager;

    public RegisterCommandValidator(UserManager<User> userManager)
    {
        _userManager = userManager;

        // FullName
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

        // Username
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
            .MustAsync(BeUniqueUserName).WithMessage("Username is already taken.");

        // Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(BeUniqueEmail).WithMessage("Email is already in use.");

        // Password
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user is null;
    }

    private async Task<bool> BeUniqueUserName(string userName, CancellationToken ct)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user is null;
    }
}
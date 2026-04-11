using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");

        await SeedRolesAsync(roleManager, logger);
        await SeedAdminAsync(userManager, logger);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
    {
        var roles = new[] { Roles.Admin, Roles.User };

        foreach (var role in roles)
        {
            var exists = await roleManager.RoleExistsAsync(role);
            if (exists)
            {
                logger.LogInformation("Role already exists: {RoleName}", role);
                continue;
            }

            var result = await roleManager.CreateAsync(new IdentityRole(role));

            if (result.Succeeded)
            {
                logger.LogInformation("Role created: {RoleName}", role);
            }
            else
            {
                logger.LogError("Failed to create role: {RoleName}. Errors: {Errors}",
                    role,
                    string.Join(" | ", result.Errors.Select(x => x.Description)));
            }
        }
    }

    private static async Task SeedAdminAsync(UserManager<User> userManager, ILogger logger)
    {
        const string adminEmail = "admin@hereizzz.com";
        const string adminUserName = "admin";
        const string adminPassword = "Admin123";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin is null)
        {
            var admin = new User
            {
                FullName = "System Admin",
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(admin, adminPassword);

            if (!createResult.Succeeded)
            {
                logger.LogError("Default admin creation failed. Errors: {Errors}",
                    string.Join(" | ", createResult.Errors.Select(x => x.Description)));
                return;
            }

            existingAdmin = admin;
            logger.LogInformation("Default admin user created. Email={Email}", adminEmail);
        }
        else
        {
            logger.LogInformation("Default admin already exists. Email={Email}", adminEmail);
        }

        var isInRole = await userManager.IsInRoleAsync(existingAdmin, Roles.Admin);
        if (!isInRole)
        {
            var addToRoleResult = await userManager.AddToRoleAsync(existingAdmin, Roles.Admin);

            if (addToRoleResult.Succeeded)
            {
                logger.LogInformation("Default admin added to Admin role.");
            }
            else
            {
                logger.LogError("Failed to add default admin to Admin role. Errors: {Errors}",
                    string.Join(" | ", addToRoleResult.Errors.Select(x => x.Description)));
            }
        }
    }
}
using API.Middlewares;
using Application.Interfaces.Services;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Seed;
using Infrastructure.Realtime;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseCors("AllowFrontend");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<NotificationHub>("/hubs/notifications");

        return app;
    }

    public static async Task ApplyMigrationsAndSeedAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<HereizzzDbContext>();
        var refreshTokenCleanupService = scope.ServiceProvider.GetRequiredService<IRefreshTokenCleanupService>();

        await context.Database.MigrateAsync();

        await IdentitySeeder.SeedAsync(scope.ServiceProvider);
        await ShippingOptionSeeder.SeedAsync(context);
        await refreshTokenCleanupService.CleanupAsync(CancellationToken.None);
    }
}
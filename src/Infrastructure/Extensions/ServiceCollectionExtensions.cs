using Application.Abstracts.Services;
using Application.Behaviors;
using Application.Interfaces.Realtime;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Options;
using Application.Queries.Routes;
using Application.Services;
using Application.Services.FeeRules;
using Application.Validators.Orders;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Persistence.Context;
using Infrastructure.Realtime;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HereizzzDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<HereizzzDbContext>()
        .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<EmailOptions>(configuration.GetSection("Email"));

        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions!.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddValidatorsFromAssembly(typeof(DeleteOrderCommandValidator).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddSignalR();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(GetBestRoutesQueryHandler).Assembly);
        });

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICartService, CartService>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IFeeCalculator, FeeCalculator>();
        services.AddScoped<IFeeRule, CustomsFeeRule>();
        services.AddScoped<IFeeRule, WarehouseFeeRule>();
        services.AddScoped<IFeeRule, LocalDeliveryFeeRule>();
        services.AddScoped<IShippingOptionRepository, ShippingOptionRepository>();
        services.AddScoped<IShippingOptionService, ShippingOptionService>();
        services.AddScoped<IRouteSelectionService, RouteSelectionService>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductProviderService, MockProductProviderService>();
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IApplicationDbContext, HereizzzDbContext>();
        services.AddScoped<IRefreshTokenCleanupService, RefreshTokenCleanupService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ICurrencyService, CurrencyService>();

        services.AddScoped<INotificationRealtimeService, SignalRNotificationRealtimeService>();

        services.AddHttpContextAccessor();

        return services;
    }
}
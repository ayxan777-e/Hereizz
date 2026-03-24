using Application.Behaviors;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Queries.Routes;
using Application.Services;
using Application.Services.FeeRules;
using Application.Validators.Orders;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Persistence.Context;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HereizzzDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddValidatorsFromAssembly(typeof(CreateOrderCommandValidator).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(GetBestRoutesQueryHandler).Assembly);
        });
        //Scopes
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IFeeCalculator, FeeCalculator>();
        services.AddScoped<IFeeRule, CustomsFeeRule>();
        services.AddScoped<IFeeRule, WarehouseFeeRule>();
        services.AddScoped<IFeeRule, LocalDeliveryFeeRule>();
        services.AddScoped<IShippingOptionRepository, ShippingOptionRepository>();
        services.AddScoped<IRouteSelectionService, RouteSelectionService>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();

        return services;
    }
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
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

        return services;
    }
}
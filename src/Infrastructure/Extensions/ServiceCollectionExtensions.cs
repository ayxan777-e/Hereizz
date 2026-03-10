using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Application.Services.FeeRules;
using Infrastructure.Persistence.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HereizzzDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        //Scopes
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
}
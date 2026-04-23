using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using GoodHamburger.Infrastructure.Persistence.Connection;
using GoodHamburger.Infrastructure.Persistence.Repositories;
using GoodHamburger.Infrastructure.Persistence.UnitOfWork;
using GoodHamburger.Application.Abstractions.Persistence;

namespace GoodHamburger.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Configure(options => options.ConnectionString = ResolveConnectionString(configuration))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString), "Database connection string was not found.")
            .ValidateOnStart();

        services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value);

        services.AddScoped<DapperDbSession>();
        services.AddScoped<IDbConnectionFactory, NpgsqlConnectionFactory>();
        services.AddScoped<IApplicationUnitOfWork, DapperUnitOfWork>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    private static string ResolveConnectionString(IConfiguration configuration)
    {
        return ResolveFirstConfiguredValue(
            configuration.GetConnectionString("Postgres"),
            configuration.GetConnectionString("RodadaNet"),
            configuration["Database:ConnectionString"]);
    }

    private static string ResolveFirstConfiguredValue(params string?[] candidates)
    {
        foreach (var candidate in candidates)
        {
            if (!string.IsNullOrWhiteSpace(candidate))
            {
                return candidate;
            }
        }

        return string.Empty;
    }
}

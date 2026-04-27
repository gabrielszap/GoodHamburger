using FluentValidation;
using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.Services;
using GoodHamburger.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<OrderRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductRequestValidator>();

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService,ProductService>();

        return services;
    }
}

using FluentValidation;
using GoodHamburger.Application.Contracts;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<OrderRequest>();
        services.AddValidatorsFromAssemblyContaining<ProductRequest>();

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService,ProductService>();

        return services;
    }
}

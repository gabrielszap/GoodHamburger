using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Mappers;

public static class ProductMapper
{
    public static ProductResult ToResult(Product product)
    {
        return new ProductResult
        {
            Id = product.Id,
            Description = product.Description,
            Price = product.Price,
            Type = product.Type,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }

    public static List<ProductResult> ToResultList(IEnumerable<Product> products)
    {
        return products.Select(ToResult).ToList();
    }
}
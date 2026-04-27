using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Common;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class Product : BaseEntity
{
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public ProductType Type { get; private set; }

    private Product() { }

    private Product(string description, decimal price, ProductType type)
    {
        Description = description;
        Price = price;
        Type = type;
        IsActive = true;
    }

    private Product(Guid id, string description, decimal price, ProductType type, bool isActive)
    {
        Id = id;
        Description = description;
        Price = price;
        Type = type;
        IsActive = isActive;
    }

    public static Product Create(string description, decimal price, ProductType type)
    {
        return new Product(description, price, type);
    }
    public static Product Create(Guid id, string description, decimal price, ProductType type, bool isActive)
    {
        return new Product(id, description, price, type, isActive);
    }
}

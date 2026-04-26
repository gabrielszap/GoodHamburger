using GoodHamburger.Domain.Abstractions.Exceptions;
using GoodHamburger.Domain.Common;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class Order : BaseEntity
{
    private readonly List<Product> _products = [];
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    public decimal Subtotal => _products.Sum(p => p.Price);
    public decimal DiscountPercentage => CalculateDiscountPercentage();
    public decimal DiscountAmount => Math.Round(Subtotal * DiscountPercentage, 2);
    public decimal Total => Subtotal - DiscountAmount;

    private Order(IEnumerable<Product> products)
    {
        AddProducts(products);
    }

    public static Order Create(Guid id, IEnumerable<Product> products, bool isActive, DateTime createdAt)
    {
        var order = new Order(products);
        order.Id = id;
        order.IsActive = isActive;
        order.CreatedAt = createdAt;

        return order;
    }
    public static Order Create(IEnumerable<Product> products)
    {
        return new Order(products);
    }

    private void AddProducts(IEnumerable<Product> products)
    {
        var duplicatedType = products
            .GroupBy(p => p.Type)
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicatedType is not null)
            throw new DomainException(
                $"O pedido pode conter apenas um produto do tipo {duplicatedType.Key}.");
          
        _products.AddRange(products);
    }

    private decimal CalculateDiscountPercentage()
    {
        var hasSandwich = _products.Any(p => p.Type == ProductType.Sanduiche);
        var hasSide = _products.Any(p => p.Type == ProductType.Acompanhamento);
        var hasDrink = _products.Any(p => p.Type == ProductType.Bebida);

        if (hasSandwich && hasSide && hasDrink)
            return 0.20m;

        if (hasSandwich && hasDrink)
            return 0.15m;

        if (hasSandwich && hasSide)
            return 0.10m;

        return 0m;
    }
}

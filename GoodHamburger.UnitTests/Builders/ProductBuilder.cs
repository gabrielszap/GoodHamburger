using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.UnitTests.Builders;

public static class ProductBuilder
{
    public static Product Sandwich(decimal price = 5.00m)
        => Product.Create("X Burger", price, ProductType.Sanduiche);

    public static Product Side(decimal price = 2.00m)
        => Product.Create("Batata Frita", price, ProductType.Acompanhamento);

    public static Product Drink(decimal price = 2.50m)
        => Product.Create("Refrigerante", price, ProductType.Bebida);
}
using FluentAssertions;
using GoodHamburger.Domain.Abstractions.Exceptions;
using GoodHamburger.Domain.Entities;
using GoodHamburger.UnitTests.Builders;

namespace GoodHamburger.UnitTests.Domain;

public class OrderTests
{
    [Fact]
    public void Should_Create_Order_With_Sandwich_Only_Without_Discount()
    {
        var products = new[]
        {
            ProductBuilder.Sandwich(5.00m)
        };

        var order = Order.Create(products);

        order.Subtotal.Should().Be(5.00m);
        order.DiscountPercentage.Should().Be(0m);
        order.Total.Should().Be(5.00m);
    }

    [Fact]
    public void Should_Create_Order_With_Side_Only_Without_Discount()
    {
        var products = new[]
        {
            ProductBuilder.Side(2.00m)
        };

        var order = Order.Create(products);

        order.Subtotal.Should().Be(2.00m);
        order.DiscountPercentage.Should().Be(0m);
        order.Total.Should().Be(2.00m);
    }

    [Fact]
    public void Should_Create_Order_With_Drink_Only_Without_Discount()
    {
        var products = new[]
        {
            ProductBuilder.Drink(2.50m)
        };

        var order = Order.Create(products);

        order.Subtotal.Should().Be(2.50m);
        order.DiscountPercentage.Should().Be(0m);
        order.Total.Should().Be(2.50m);
    }

    [Fact]
    public void Should_Create_Order_Without_Discount_When_Side_And_Drink()
    {
        var products = new[]
        {
            ProductBuilder.Side(2.00m),
            ProductBuilder.Drink(2.50m)
        };

        var order = Order.Create(products);

        order.Subtotal.Should().Be(4.50m);
        order.DiscountPercentage.Should().Be(0m);
        order.Total.Should().Be(4.50m);
    }

    [Fact]
    public void Should_Apply_10_Percent_Discount_When_Sandwich_And_Side()
    {
        var products = new[]
        {
            ProductBuilder.Sandwich(5.00m),
            ProductBuilder.Side(2.00m)
        };

        var order = Order.Create(products);

        order.Subtotal.Should().Be(7.00m);
        order.DiscountPercentage.Should().Be(0.10m);
        order.Total.Should().Be(6.30m);
    }

    [Fact]
    public void Should_Apply_15_Percent_Discount_When_Sandwich_And_Drink()
    {
        var products = new[]
        {
            ProductBuilder.Sandwich(5.00m),
            ProductBuilder.Drink(2.50m)
        };

        var order = Order.Create(products);

        order.Subtotal.Should().Be(7.50m);
        order.DiscountPercentage.Should().Be(0.15m);
        order.Total.Should().Be(6.38m);
    }

    [Fact]
    public void Should_Apply_20_Percent_Discount_When_Full_Combo()
    {
        var products = new[]
        {
            ProductBuilder.Sandwich(5.00m),
            ProductBuilder.Side(2.00m),
            ProductBuilder.Drink(2.50m)
        };

        var order = Order.Create(products);

        order.Subtotal.Should().Be(9.50m);
        order.DiscountPercentage.Should().Be(0.20m);
        order.Total.Should().Be(7.60m);
    }

    [Fact]
    public void Should_Throw_When_Duplicated_Sandwich()
    {
        var products = new[]
        {
            ProductBuilder.Sandwich(),
            ProductBuilder.Sandwich()
        };

        var act = () => Order.Create(products);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*Sanduiche*");
    }

    [Fact]
    public void Should_Throw_When_Duplicated_Drink()
    {
        var products = new[]
        {
            ProductBuilder.Drink(),
            ProductBuilder.Drink()
        };

        var act = () => Order.Create(products);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*Bebida*");
    }

    [Fact]
    public void Should_Throw_When_Duplicated_Side()
    {
        var products = new[]
        {
            ProductBuilder.Side(),
            ProductBuilder.Side()
        };

        var act = () => Order.Create(products);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*Acompanhamento*");
    }
}
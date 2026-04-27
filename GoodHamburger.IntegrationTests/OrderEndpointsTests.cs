using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GoodHamburger.IntegrationTests.Models;

namespace GoodHamburger.IntegrationTests;

public sealed class OrderEndpointsTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture _fixture;

    public OrderEndpointsTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetProducts_ShouldReturnPagedMenu()
    {
        var response = await _fixture.Client.GetAsync("/api/product?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<ProductResult>>();

        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnCreated_WhenRequestIsValid()
    {
        var products = await _fixture.GetMenuProductsAsync();

        var sandwich = products.First(x => x.Type.ToString() == "Sanduiche");
        var side = products.First(x => x.Type.ToString() == "Acompanhamento");
        var drink = products.First(x => x.Type.ToString() == "Bebida");

        var request = new OrderRequest
        {
            ProductIds = [sandwich.Id, side.Id, drink.Id]
        };

        var response = await _fixture.Client.PostAsJsonAsync("/api/order", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var order = await response.Content.ReadFromJsonAsync<OrderResult>();

        order.Should().NotBeNull();
        order!.Products.Should().HaveCount(3);

        var expectedSubtotal = sandwich.Price + side.Price + drink.Price;
        var expectedDiscountPercentage = 0.20m;
        var expectedDiscountAmount = Math.Round(expectedSubtotal * expectedDiscountPercentage, 2);
        var expectedTotal = expectedSubtotal - expectedDiscountAmount;

        order.Subtotal.Should().Be(expectedSubtotal);
        order.DiscountPercentage.Should().Be(expectedDiscountPercentage);
        order.DiscountAmount.Should().Be(expectedDiscountAmount);
        order.TotalPrice.Should().Be(expectedTotal);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnBadRequest_WhenRequestHasDuplicatedProductType()
    {
        var products = await _fixture.GetMenuProductsAsync();

        var sandwiches = products
            .Where(x => x.Type.ToString() == "Sanduiche")
            .Take(2)
            .ToList();

        sandwiches.Should().HaveCount(2);

        var request = new OrderRequest
        {
            ProductIds = sandwiches.Select(x => x.Id).ToList()
        };

        var response = await _fixture.Client.PostAsJsonAsync("/api/order", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadAsStringAsync();

        error.Should().Contain("Sanduiche");
    }

    [Fact]
    public async Task GetOrders_ShouldReturnPagedOrders()
    {
        var response = await _fixture.Client.GetAsync("/api/order?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<OrderResult>>();

        result.Should().NotBeNull();
        result!.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}
namespace GoodHamburger.IntegrationTests.Models;

public sealed class OrderRequest
{
    public IReadOnlyCollection<Guid> ProductIds { get; init; } = [];
}
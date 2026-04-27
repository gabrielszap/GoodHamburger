namespace GoodHamburger.Blazor.Models;

public sealed class OrderRequest
{
    public IReadOnlyCollection<Guid> ProductIds { get; init; } = Array.Empty<Guid>();
}

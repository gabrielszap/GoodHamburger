namespace GoodHamburger.Blazor.Models;

public sealed class ProductResult
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ProductType Type { get; set; }
    public bool IsActive { get; set; }
}

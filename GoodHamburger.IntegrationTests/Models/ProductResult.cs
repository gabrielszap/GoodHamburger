namespace GoodHamburger.IntegrationTests.Models;

public sealed class ProductResult
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
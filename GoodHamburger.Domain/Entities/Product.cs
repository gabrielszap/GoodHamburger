using GoodHamburger.Domain.Common;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class Product : BaseEntity
{
    public string Description { get; set; }
    public decimal Price { get; set; }
    public ProductType Type { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

using GoodHamburger.Domain.Common;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class Order : BaseEntity
{
    //public Product(
    //    Guid categoryId,
    //    string name,
    //    string? shortName = null,
    //    string? description = null,
    //    Guid? id = null)
    //    : base(id)
    //{
    //    CategoryId = Guard.AgainstEmptyId(categoryId, nameof(categoryId));
    //    Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
    //    ShortName = string.IsNullOrWhiteSpace(shortName) ? null : shortName.Trim();
    //    Description = description?.Trim();
    //    IsActive = true;
    //}

    //public Guid CategoryId { get; private set; }

    //public string Name { get; private set; }

    //public string? ShortName { get; private set; }

    //public string? Description { get; private set; }

    //public bool IsActive { get; private set; }

    //public void UpdateDetails(string name, string? shortName, string? description)
    //{
    //    Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
    //    ShortName = string.IsNullOrWhiteSpace(shortName) ? null : shortName.Trim();
    //    Description = description?.Trim();
    //}

    //public void Activate()
    //{
    //    IsActive = true;
    //}

    //public void Deactivate()
    //{
    //    IsActive = false;
    //}
    public Guid Id { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
}

namespace GoodHamburger.Domain.Common;

public abstract class BaseEntity
{
    protected BaseEntity(Guid? id = null)
    {
        if (id.HasValue && id.Value == Guid.Empty)
        {
            throw new ArgumentException("Entity id cannot be empty.", nameof(id));
        }

        Id = id ?? Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

namespace GoodHamburger.Application.Abstractions.Persistence;

public interface IApplicationUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

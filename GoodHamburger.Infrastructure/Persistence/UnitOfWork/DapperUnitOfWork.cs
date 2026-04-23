using GoodHamburger.Application.Abstractions.Persistence;

namespace GoodHamburger.Infrastructure.Persistence.UnitOfWork;

public sealed class DapperUnitOfWork : IApplicationUnitOfWork, IAsyncDisposable, IDisposable
{
    private readonly DapperDbSession _dbSession;

    public DapperUnitOfWork(DapperDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbSession.CommitAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return _dbSession.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    public ValueTask DisposeAsync()
    {
        return _dbSession.DisposeAsync();
    }
}

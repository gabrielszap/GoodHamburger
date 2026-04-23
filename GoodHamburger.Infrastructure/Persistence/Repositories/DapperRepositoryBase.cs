using System.Data;
using GoodHamburger.Infrastructure.Persistence.UnitOfWork;

namespace GoodHamburger.Infrastructure.Persistence.Repositories;

public abstract class DapperRepositoryBase
{
    private readonly DapperDbSession _dbSession;

    protected DapperRepositoryBase(DapperDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    protected IDbTransaction? Transaction => _dbSession.CurrentTransaction;

    protected Task<IDbConnection> GetOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        return _dbSession.GetOpenConnectionAsync(cancellationToken);
    }

    protected Task EnsureTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _dbSession.EnsureTransactionAsync(cancellationToken);
    }
}

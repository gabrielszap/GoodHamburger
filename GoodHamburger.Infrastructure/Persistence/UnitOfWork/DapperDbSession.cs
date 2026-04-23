using System.Data;
using System.Data.Common;
using GoodHamburger.Infrastructure.Persistence.Connection;

namespace GoodHamburger.Infrastructure.Persistence.UnitOfWork;

public sealed class DapperDbSession : IAsyncDisposable, IDisposable
{
    private readonly IDbConnectionFactory _connectionFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public DapperDbSession(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IDbTransaction? CurrentTransaction => _transaction;

    public async Task<IDbConnection> GetOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        _connection ??= _connectionFactory.CreateConnection();

        if (_connection.State == ConnectionState.Open)
        {
            return _connection;
        }

        if (_connection is DbConnection dbConnection)
        {
            await dbConnection.OpenAsync(cancellationToken);
        }
        else
        {
            _connection.Open();
        }

        return _connection;
    }

    public async Task EnsureTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            return;
        }

        var connection = await GetOpenConnectionAsync(cancellationToken);

        if (connection is DbConnection dbConnection)
        {
            _transaction = await dbConnection.BeginTransactionAsync(cancellationToken);
            return;
        }

        _transaction = connection.BeginTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            return;
        }

        if (_transaction is DbTransaction dbTransaction)
        {
            await dbTransaction.CommitAsync(cancellationToken);
        }
        else
        {
            _transaction.Commit();
        }

        await DisposeTransactionAsync();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            return;
        }

        if (_transaction is DbTransaction dbTransaction)
        {
            await dbTransaction.RollbackAsync(cancellationToken);
        }
        else
        {
            _transaction.Rollback();
        }

        await DisposeTransactionAsync();
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await RollbackAsync();

        if (_connection is IAsyncDisposable asyncDisposableConnection)
        {
            await asyncDisposableConnection.DisposeAsync();
        }
        else
        {
            _connection?.Dispose();
        }

        _connection = null;
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction is IAsyncDisposable asyncDisposableTransaction)
        {
            await asyncDisposableTransaction.DisposeAsync();
        }
        else
        {
            _transaction?.Dispose();
        }

        _transaction = null;
    }
}

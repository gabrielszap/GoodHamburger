using System.Data;
using Npgsql;

namespace GoodHamburger.Infrastructure.Persistence.Connection;

public sealed class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly DatabaseOptions _databaseOptions;

    public NpgsqlConnectionFactory(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public IDbConnection CreateConnection()
    {
        if (string.IsNullOrWhiteSpace(_databaseOptions.ConnectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        return new NpgsqlConnection(_databaseOptions.ConnectionString);
    }
}

using System.Data;

namespace GoodHamburger.Infrastructure.Persistence.Connection;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

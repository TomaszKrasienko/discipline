using System.Data;

namespace discipline.hangfire.shared.abstractions.DataAccess;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
using System.Data;

namespace discipline.hangfire.shared.abstractions.DataAccess;

public interface IDbContext : IDisposable
{
    IDbConnection GetConnection();
}
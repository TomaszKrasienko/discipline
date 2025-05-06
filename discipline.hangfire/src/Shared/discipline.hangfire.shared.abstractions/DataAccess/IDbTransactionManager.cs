using System.Data;

namespace discipline.hangfire.shared.abstractions.DataAccess;

public interface IDbTransactionManager
{
    IDbConnection Connection { get; }
    IDbTransaction Begin();
    void Commit();
    void Rollback();
}
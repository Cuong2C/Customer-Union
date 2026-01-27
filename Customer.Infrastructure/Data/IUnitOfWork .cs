using System.Data;

namespace Customer_Union.Infrastructure.Data;

public interface IUnitOfWork : IDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }
    void Commit();
    void Rollback();
}

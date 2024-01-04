using System.Data;

namespace Subscription.Infraestructure.Abstractions.Database
{
    public interface IUnitOfWork
    {
        IDbConnection BeginConnection();
        void Commit();
        void Rollback();
    }
}

using Subscription.Infraestructure.Database.Configuration;
using System.Data;

namespace Subscription.Infraestructure.Abstractions.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbConnector session;

        public UnitOfWork(DbConnector connector)
        {
            session = connector;
        }
        public IDbConnection BeginConnection()
        {
            session.Transaction = session.Connection.BeginTransaction();
            return session.Transaction.Connection;
        }

        public void Commit()
        {
            session.Transaction.Commit();
            Dispose();
        }

        private void Dispose() => session.Transaction.Dispose();

        public void Rollback()
        {
            session.Transaction.Rollback();
            Dispose();
        }
    }
}

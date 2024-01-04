using Npgsql;
using System.Data;

namespace Subscription.Infraestructure.Database.Configuration
{
    public class DbConnector : IDisposable
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }

        public DbConnector(DbConnection dbConnection)
        {
            var connectionString = dbConnection.GetConnectionString();
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }
        public void Dispose() => Connection?.Dispose();
    }
}
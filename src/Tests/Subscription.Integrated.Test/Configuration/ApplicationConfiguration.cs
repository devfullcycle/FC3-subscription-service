using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Subscription.Application.Adapters.Repository;
using Subscription.Infraestructure.Abstractions.Database;
using Subscription.Infraestructure.Database.Configuration;
using Subscription.Infraestructure.Database.Repository;
using System.Data;

namespace Subscription.Integrated.Test.Configuration
{
    public class ApplicationConfiguration : WebApplicationFactory<Program>
    {
        private DbConnector Connector;
        public IDbConnection Connection;

        public void Init(IServiceScope scope)
        {
            var connection = scope.ServiceProvider.GetRequiredService<DbConnector>();
            Connector = connection;
            Connection = connection.Connection; 
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.IntegratedTest.Json").Build();

            builder.ConfigureServices(service =>
            {
                var connectionData = configuration.GetSection(nameof(DbConnection)).Get<DbConnection>();
                
                service.AddTransient<DbConnection>();
                service.AddTransient<IUnitOfWork, UnitOfWork>();
                service.AddTransient<ISubscriptionRepository, SubscriptionRepository>();

                if (connectionData is not null)
                    service.AddSingleton(connectionData);
                else
                    throw new Exception("database configuration not found");
            });

        }

        public async Task CreateDatabase()
        {
            await DropDatabaseTables();
            await Connection.ExecuteAsync(DbCreateScript.CreateUUID);
            await Connection.ExecuteAsync(DbCreateScript.CreatePlanTable);
            await Connection.ExecuteAsync(DbCreateScript.CreatePlanCostTable);
            await Connection.ExecuteAsync(DbCreateScript.CreateUserTable);
            await Connection.ExecuteAsync(DbCreateScript.CreateUserSubscriptionTable);
            await Connection.ExecuteAsync(DbCreateScript.CreateLogsTable);
            await Connection.ExecuteAsync(DbCreateScript.CreateFunctionsTable);
        }

        public async Task DropDatabaseTables()
        {

            await Connection.ExecuteAsync(DbCreateScript.DropDatabase);
        }
    }
}

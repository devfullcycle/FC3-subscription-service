using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Subscription.Application.Adapters.Repository;
using Subscription.Infraestructure.Abstractions.Database;
using Subscription.Infraestructure.Database.Repository;

namespace Subscription.Infraestructure.Database.Configuration
{
    public static class IoC
    {
        public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetSection(nameof(DbConnection)).Get<DbConnection>();
            services.AddScoped<DbConnector>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();

            if (configuration is not null)
                services.AddSingleton(connection);
            else
                throw new Exception("Database configuration not founded");

            return services;
        }
    }
}

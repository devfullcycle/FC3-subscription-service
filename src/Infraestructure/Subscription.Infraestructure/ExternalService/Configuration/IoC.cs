using Keycloak.AuthServices.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Subscription.Application.Adapters.AuthService;
using Subscription.Application.Adapters.PaymentGateway;
using Subscription.Infraestructure.ExternalService.Keycloak;
using Subscription.Infraestructure.ExternalService.PaymentService;

namespace Subscription.Infraestructure.ExternalService.Configuration
{
    public static class IoC
    {
        public static IServiceCollection AddGateway(this IServiceCollection services, IConfiguration configuration)
        {
            var keycloak = configuration
                                .GetSection(KeycloakAuthenticationOptions.Section)
                                .Get<KeycloakAuthenticationOptions>();
            var toRequest = configuration
                                .GetSection("userAdminToClient")
                                .Get<KeycloakConfig>();

            services.AddSingleton(toRequest);

            services
                .AddRefitClient<IKeycloakApi>()
                .ConfigureHttpClient(config => config.BaseAddress = new Uri(keycloak?.AuthServerUrl ?? ""));
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IPaymentGateway, PaymentGateway>();
            
            return services;
        }
    }
}

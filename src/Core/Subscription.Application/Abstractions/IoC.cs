using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Subscription.Application.Handlers.Comand.CreatePlan;
using Subscription.Application.Handlers.Comand.CreatePlanCost;
using Subscription.Application.Handlers.Comand.CreateUser;
using Subscription.Application.Handlers.Comand.CreateUserPayment;
using Subscription.Application.Handlers.Query.GetPlans;
using Subscription.Application.Handlers.Query.GetSubscriber;
using Subscription.Application.Handlers.Query.GetSusbscribers;

namespace Subscription.Application.Abstractions
{
    public static class IoC
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICreatePlanHandler, CreatePlanHandler>();
            services.AddTransient<ICreatePlanCostHandler, CreatePlanCostHandler>();
            services.AddTransient<ICreateUserHandler, CreateUserHandler>();
            services.AddTransient<ICreateUserPaymentHandler, CreateUserPaymentHandler>();
            services.AddTransient<IGetPlansHandler, GetPlansHandler>();
            services.AddTransient<IGetSubscribersHandler, GetSubscribersHandler>();
            services.AddTransient<IGetSubscriberHandler, GetSubscriberHandler>();

            return services;
        }
    }
}

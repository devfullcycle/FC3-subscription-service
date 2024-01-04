using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Handlers.Query.GetSubscriber;
using Subscription.Application.Handlers.Query.GetSusbscribers;
using Subscription.Domain.Entities;
using Subscription.Domain.Enum;

namespace Subscription.Application.Adapters.Repository
{
    public interface ISubscriptionRepository
    {
        Task<Plan> CreatePlanAsync(Plan plan, CancellationToken cancellationToken);
        Task<PlanCost> CreatePlanCostAsync(PlanCost planCost, CancellationToken cancellationToken);
        Task<User> CreateUserOnDatabaseasync(User user, CancellationToken cancellationToken);
        Task<bool> CheckIfUserHasActiveSubscription(Guid userId, Guid PLanId, CancellationToken cancellationToken);
        Task<UserSubscription> CreateSubscriptionOnDatabaseasync(UserSubscription userSub, CancellationToken cancellationToken);
        Task<User> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task<Plan> GetPlanByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task<Plan> GetPlanByIdAsync(Guid Id, PlanType planType,CancellationToken cancellationToken);
        Task<User> GetUserByDocumentNumber(string documentNumber, CancellationToken cancellationToken);

        Task<List<Plan>> GetPlans(int page, int pagesize, CancellationToken cancellationToken);
        Task<MetaData> GetPlansPaginated(int page, int pagesize, CancellationToken cancellationToken);
        Task<GetSubscriberOutput> GetSubscriberById(Guid id, CancellationToken cancellationToken);
        Task<GetSubscribersOutput> GetSubscribers(int page, int size, CancellationToken cancellationToken);
        Task<MetaData> GetSubscribersPaginated(int page, int size, CancellationToken cancellationToken);
    }
}

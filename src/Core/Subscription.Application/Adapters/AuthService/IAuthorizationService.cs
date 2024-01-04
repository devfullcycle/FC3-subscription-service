namespace Subscription.Application.Adapters.AuthService
{
    public interface IAuthorizationService
    {
        public Task AddSubscriberRoleToUser(string email, CancellationToken cancellationToken);
        public Task RemoveSubscriberRoleToUser(string email, CancellationToken cancellationToken);
    }
}

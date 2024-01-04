namespace Subscription.Domain.Abstractions.Base
{
    public interface IBase
    {
        List<string> Notifications { get; }
        void AddNotification(string message);
    }
}

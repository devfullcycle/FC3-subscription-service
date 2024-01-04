namespace Subscription.Domain.Abstractions.Base
{
    public class ValueObjectBase : IBase
    {
        public bool IsValid { get { return !Notifications.Any(); } }
        public List<string> Notifications { get; private set; } = new();

        public void AddNotification(string message) =>
            Notifications.Add(message);
    }
}

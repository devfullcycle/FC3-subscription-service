namespace Subscription.Domain.Abstractions.Base
{
    public class EntityBase : IBase
    {
        public Guid Id { get; set; }
        public bool IsValid { get { return !Notifications.Any(); } }
        public List<string> Notifications { get; private set; } = new();

        public void AddNotification(string message)
        {
            Notifications.Add(message);
        }

        public void AddNotifications(List<string> messages)
        {
            Notifications.AddRange(messages);
        }

        public void ClearNotifications() => Notifications.Clear();
    }
}

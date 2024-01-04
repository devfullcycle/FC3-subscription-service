using Subscription.Domain.Abstractions.Base;

namespace Subscription.Domain.Entities
{
    public class UserSubscription : EntityBase
    {
        public User User { get; private set; }
        public Plan Plan { get; private set; }
        public DateTime LastBilling { get; private set; }
        public bool Active { get; private set; }
        public bool Cancelled { get; private set; }

        public static UserSubscription Create(User user, Plan plan, DateTime date, bool? active = true, bool? cancelled = false)
        {
            UserSubscription subscription = new();

            subscription.ChangeUser(user);
            subscription.ChangePlan(plan);
            subscription.ChangeLastBillint(date);
            subscription.Active = active!.Value;
            subscription.Cancelled = cancelled!.Value;

            return subscription;
        }

        private void ChangeLastBillint(DateTime date)
        {
            if(DateTime.Now < date)
            {
                AddNotification("Billing Date Invalid");
                return;
            }
            LastBilling = date;
        }

        private void ChangePlan(Plan plan)
        {
            if (!plan.IsValid)
            {
                AddNotifications(plan.Notifications);
                AddNotification("Plan invalid");
                return;
            }
            Plan = plan;
        }

        private void ChangeUser(User user)
        {
            if (!user.IsValid)
            {
                AddNotifications(user.Notifications);
                AddNotification("User invalid");
                return;
            }
            User = user;
        }
    }
}

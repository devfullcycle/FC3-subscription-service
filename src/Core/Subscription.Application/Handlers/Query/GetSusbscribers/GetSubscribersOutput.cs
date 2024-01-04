namespace Subscription.Application.Handlers.Query.GetSusbscribers
{
    public class GetSubscribersOutput
    {
        public static GetSubscribersOutput Create() => new();
        public List<GetSubscribersResumeOutput> Subscribers { get; set; } = new();

        public void AddSubscriber(GetSubscribersResumeOutput data) => Subscribers.Add(data);
        public void AddSubscriber(List<GetSubscribersResumeOutput> data) => Subscribers.AddRange(data);
    }

    public class GetSubscribersResumeOutput
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PlanName { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public bool IsActive { get; set; }

        public static GetSubscribersResumeOutput GetInstance() => new();

        public GetSubscribersResumeOutput AddName(string name, string lastName, bool isActive)
        {
            Name = name;
            LastName = lastName;
            IsActive = isActive;
            return this;
        }

        public GetSubscribersResumeOutput AddPlan(string planName, decimal price)
        {
            PlanName = planName;
            Price = price;
            return this;
        }
    }
}

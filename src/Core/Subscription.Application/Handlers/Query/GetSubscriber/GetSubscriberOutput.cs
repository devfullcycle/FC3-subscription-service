using Subscription.Domain.Enum;

namespace Subscription.Application.Handlers.Query.GetSubscriber
{
    public class GetSubscriberOutput
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get;  set; }
        public string DocumentNumber { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public decimal Price { get; set; }
        public PlanType Period { get; set; }

        public static GetSubscriberOutput Create() => new();

        public GetSubscriberOutput WithUser(string name, string lastName, int age)
        {
            Name = name;
            LastName = lastName;
            Age = age;
            return this;    
        }

        public GetSubscriberOutput WithDocument(string documentNumber)
        {
            DocumentNumber = documentNumber;
            return this;
        }

        public GetSubscriberOutput WithAddress(string zipcode, string city, string country, string state, string street)
        {
            ZipCode = zipcode;
            City = city;
            Country = country;
            State = state;
            PlanName = street;
            return this;
        }

        public GetSubscriberOutput WithPlan(string planName, string planDescription, decimal price, PlanType planType)
        {
            PlanName = planName;
            PlanDescription = planDescription;
            Price = price;
            Period = planType;
            return this;
        }

    }
}

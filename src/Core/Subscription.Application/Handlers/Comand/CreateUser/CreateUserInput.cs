using Subscription.Application.Abstractions.Handlers;
using Subscription.Domain.Entities;
using Subscription.Domain.ValueObject;

namespace Subscription.Application.Handlers.Comand.CreateUser
{
    public class CreateUserInput : IInput
    {
        public CreateUserInput(string name, 
            string lastName, 
            int age, 
            string documentNumber, 
            string zipCode, 
            string street,
            string city, 
            string state, 
            string country, Guid correlationId)
        {
            Name = name;
            LastName = lastName;
            Age = age;
            DocumentNumber = documentNumber;
            ZipCode = zipCode;
            Street = street;
            City = city;
            State = state;
            Country = country;
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }

        public void CreateCorrelation(Guid? correlationId = null)
        {
            CorrelationId = correlationId is not null ? correlationId.Value : Guid.NewGuid();
        }

        public string Name { get; private set; } = "";
        public string LastName { get; private set; } = "";
        public int Age { get; set; } = 0;
        public string DocumentNumber { get; private set; } = "";
        public string ZipCode { get; private set; } = "";
        public string Street { get; private set; } = "";
        public string City { get; private set; } = "";
        public string State { get; private set; } = "";
        public string Country { get; private set; } = "";

        public User CreaUser()
        {
            var address = Address.Create(ZipCode, Street, City, State, Country);
            var doc = Document.Create(DocumentNumber);
            return User.Create(Name, LastName, Age, address, doc);
        }

    }
}

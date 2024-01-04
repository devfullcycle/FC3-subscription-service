using Subscription.Domain.Abstractions.Base;
using Subscription.Domain.ValueObject;

namespace Subscription.Domain.Entities
{
    public class User : EntityBase
    {
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get { return string.Concat(Name, " ", LastName); } }
        public int Age { get; private set; }
        public Address Address { get; private set; }
        public Document Document { get; private set; }

        public static User Create(string name, string lastName, int age, Address address, Document document)
        {
            User user = new();
            user.ChangeName(name);
            user.ChangeLastName(lastName);
            user.ChangeAge(age);
            user.ChangeAddress(address);
            user.ChangeDocument(document);

            return user;
        }

        public static User CreateBlank() => new();

        public void ChangeDocument(Document document)
        {
            if (!document.IsValid)
            {
                AddNotifications(document.Notifications);
                return;
            }
            Document = document;
        }

        public void ChangeAddress(Address address)
        {
            if (!address.IsValid)
            {
                AddNotifications(address.Notifications);
                return;
            }
            Address = address;
        }

        public void ChangeAge(int age)
        {
            if(age < 18 || age.Equals(0))
            {
                AddNotification("Idade incorreta, o usuario deve ser maior de 18 anos");
                return;
            }
            Age = age;
        }

        public void ChangeLastName(string lastName)
        {
            if(string.IsNullOrWhiteSpace(lastName) || lastName.Length < 3)
            {
                AddNotification("Sobrenome incorreto, minimo 3 caracteres");
                return;
            }
            LastName = lastName;
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                AddNotification("Nome incorreto, minimo 3 caracteres");
                return;
            }
            Name = name;
        }
    }
}

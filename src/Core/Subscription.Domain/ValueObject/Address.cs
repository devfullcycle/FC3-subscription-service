using Subscription.Domain.Abstractions.Base;
using System.IO;

namespace Subscription.Domain.ValueObject
{
    public class Address : ValueObjectBase
    {
        public string ZipCode { get; set; } = "";
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";

        public static Address Create(string zipCode,
            string street, 
            string city, 
            string state,
            string country)
        {
            Address address = new();
            address.ChgangeZipCode(zipCode);
            address.ChangeStreed(street);
            address.ChangeCity(city);
            address.ChangeState(state);
            address.ChangeCountry(country);

            return address;
        }

        private void ChangeCountry(string country)
        {
            if(string.IsNullOrWhiteSpace(country) || !country.Length.Equals(2))
            {
                AddNotification("Pais invalido, apenas 2 caracteres");
                return;
            }
            Country = country;
        }


        private void ChangeState(string state)
        {
            if(string.IsNullOrWhiteSpace(state) || !state.Length.Equals(2))
            {
                AddNotification("Estado incompleto apenas 2 caracteres");
                return;
            }
            State = state;  
        }

        private void ChangeCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city) || city.Length < 4)
            {
                AddNotification("Cidade incompleta, 4 caracteres ou mais");
                return;
            }
            City = city;
        }

        private void ChangeStreed(string street)
        {
            if (string.IsNullOrWhiteSpace(street) || street.Length < 3)
            {
                AddNotification("Rua incompleta, 3 caracteres ou mais");
                return;
            }
            Street = street;
        }

        private void ChgangeZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length < 8)
            {
                AddNotification("CEP incompleto, 8 caracteres ou mais");
                return;
            }
            ZipCode = zipCode;
        }
    }
}

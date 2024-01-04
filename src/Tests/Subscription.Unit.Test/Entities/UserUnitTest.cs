using Subscription.Domain.Entities;
using Subscription.Domain.Enum;
using Subscription.Domain.ValueObject;

namespace Subscription.Unit.Test.Entities
{
    public class UserUnitTest
    {
        [Fact]
        public void TestUserSuccess()
        {
            var user = User.Create(
                "FirstName",
                "LastName",
                18,
                Address.Create(
                   "ZipCodee",
                   "Street",
                   "City",
                   "Sp",
                   "BR"),
                Document.Create("11111111111"));

            Assert.True(user.IsValid);
            Assert.False(user.Notifications.Any());

            user.ChangeDocument(Document.Create("12345678912345"));

            Assert.Equal(DocumentType.CNPJ, user.Document.DocumentType);
        }

        [Fact]
        public void TestUSerWithWrongName()
        {
            var user = User.Create(
               "",
               "LastName",
               18,
               Address.Create(
                  "ZipCodee",
                  "Street",
                  "City",
                  "Sp",
                  "BR"),
               Document.Create("11111111111"));

            Assert.False(user.IsValid);
            Assert.Equal(user.Notifications.Count, 1);
            Assert.Equal(user.Notifications.First(), "Nome incorreto, minimo 3 caracteres");
        }

        [Fact]
        public void TestUserWithWrongLastName()
        {
            var user = User.Create(
               "FirstName",
               "",
               18,
               Address.Create(
                  "ZipCodee",
                  "Street",
                  "City",
                  "Sp",
                  "BR"),
               Document.Create("11111111111"));
            Assert.False(user.IsValid);
            Assert.Equal(user.Notifications.Count, 1);
            Assert.Equal(user.Notifications.First(), "Sobrenome incorreto, minimo 3 caracteres");
        }

        [Fact]
        public void TestUserWithWrongAGE()
        {
            var user = User.Create(
               "FirstName",
               "LastName",
               17,
               Address.Create(
                  "ZipCodee",
                  "Street",
                  "City",
                  "Sp",
                  "BR"),
               Document.Create("11111111111"));
            Assert.False(user.IsValid);
            Assert.Equal(user.Notifications.Count, 1);
            Assert.Equal(user.Notifications.First(), "Idade incorreta, o usuario deve ser maior de 18 anos");
        }

        [Fact]
        public void TestUserWithWrongAddressZipCode()
        {
            var user = User.Create(
               "FirstName",
               "LastName",
               18,
               Address.Create(
                  "ZipCode",
                  "Street",
                  "City",
                  "Sp",
                  "BR"),
               Document.Create("11111111111"));
            Assert.False(user.IsValid);
            Assert.Equal(user.Notifications.Count, 1);
            Assert.Equal(user.Notifications.First(), "CEP incompleto, 8 caracteres ou mais");
        }

        [Fact]
        public void TestUserWithWrongAddressStreet()
        {
            var user = User.Create(
               "FirstName",
               "LastName",
               18,
               Address.Create(
                  "ZipCodee",
                  "",
                  "City",
                  "Sp",
                  "BR"),
               Document.Create("11111111111"));
            Assert.False(user.IsValid);
            Assert.Equal(user.Notifications.Count, 1);
            Assert.Equal(user.Notifications.First(), "Rua incompleta, 3 caracteres ou mais");
        }

        [Fact]
        public void TestUserWithWrongAddressCity()
        {
            var user = User.Create(
               "FirstName",
               "LastName",
               18,
               Address.Create(
                  "ZipCodee",
                  "Street",
                  "C",
                  "Sp",
                  "BR"),
               Document.Create("11111111111"));
            Assert.False(user.IsValid);
            Assert.Equal(user.Notifications.Count, 1);
            Assert.Equal(user.Notifications.First(), "Cidade incompleta, 4 caracteres ou mais");
        }

        [Fact]
        public void TestUserWithWrongAddressState()
        {
            var user = User.Create(
               "FirstName",
               "LastName",
               18,
               Address.Create(
                  "ZipCodee",
                  "Street",
                  "City",
                  "Sao paulo",
                  "BR"),
               Document.Create("11111111111"));
            Assert.False(user.IsValid);
            Assert.Equal(user.Notifications.Count, 1);
            Assert.Equal(user.Notifications.First(), "Estado incompleto apenas 2 caracteres");
        }

        [Fact]
        public void TestUserWithWrongAddressCountry()
        {
            var user = User.Create(
               "FirstName",
               "LastName",
               18,
               Address.Create(
                  "ZipCodee",
                  "Street",
                  "City",
                  "Sp",
                  "BRAZIL"),
               Document.Create("11111111111"));
            Assert.False(user.IsValid);
            Assert.Equal(user.Notifications.Count, 1);
            Assert.Equal(user.Notifications.First(), "Pais invalido, apenas 2 caracteres");
        }

        [Fact]
        public void TestUserWithWrongDocument()
        {
            var user = User.Create(
               "FirstName",
               "LastName",
               18,
               Address.Create(
                  "ZipCodee",
                  "Street",
                  "City",
                  "Sp",
                  "BR"),
               Document.Create("111111"));
            Assert.False(user.IsValid);
            Assert.Equal(user.Notifications.Count, 1);
            Assert.Equal(user.Notifications.First(), "Documento invalido, 11 ou 14 caracteres");
        }
    }
}

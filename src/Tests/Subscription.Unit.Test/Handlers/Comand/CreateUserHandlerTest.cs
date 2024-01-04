using Microsoft.Extensions.Logging;
using Moq;
using Subscription.Application.Adapters.Repository;
using Subscription.Application.Handlers.Comand.CreateUser;
using Subscription.Domain.Entities;

namespace Subscription.Unit.Test.Handlers.Comand
{
    public  class CreateUserHandlerTest
    {
        [Fact]
        public async Task CreateUserWithSuccess()
        {
            var input = new CreateUserInput(
                "Anderson",
                "Amaral",
                26,
                "42469323345",
                "15999999",
                "Rua do code", "Cidade", "SP", "BR", Guid.NewGuid());
            var logger = new Mock<ILogger<CreateUserInput>>();
            var repository = new Mock<ISubscriptionRepository>();

            repository
                .Setup(item => item.CreateUserOnDatabaseasync(It.IsAny<User>(), CancellationToken.None))
                .ReturnsAsync(input.CreaUser());
            repository
                .Setup(item => item.GetUserByDocumentNumber(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(input.CreaUser());

            var handler = new CreateUserHandler(logger.Object, repository.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(response);
            Assert.True(response.Meta.IsValid);
        }

        [Fact]
        public async Task CreateUserWithInvalidDocument()
        {
            var input = new CreateUserInput(
                "Anderson",
                "Amaral",
                26,
                "42469323",
                "15999999",
                "Rua do code", "Cidade", "SP", "BR", Guid.NewGuid());
            var logger = new Mock<ILogger<CreateUserInput>>();
            var repository = new Mock<ISubscriptionRepository>();

            repository
                .Setup(item => item.CreateUserOnDatabaseasync(It.IsAny<User>(), CancellationToken.None))
                .ReturnsAsync(input.CreaUser());

            var handler = new CreateUserHandler(logger.Object, repository.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(response);
            Assert.False(response.Meta.IsValid);
        }

        [Fact]
        public async Task CreateUserWithInvalidAddress()
        {
            var input = new CreateUserInput(
                "Anderson",
                "Amaral",
                26,
                "42469323345",
                "15999999",
                "Rua do code", "Cidade", "SPS", "BR", Guid.NewGuid());
            var logger = new Mock<ILogger<CreateUserInput>>();
            var repository = new Mock<ISubscriptionRepository>();

            repository
                .Setup(item => item.CreateUserOnDatabaseasync(It.IsAny<User>(), CancellationToken.None))
                .ReturnsAsync(input.CreaUser());

            var handler = new CreateUserHandler(logger.Object, repository.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(response);
            Assert.False(response.Meta.IsValid);
        }

        [Fact]
        public async Task CreateUserWithInvalidRepository()
        {
            var input = new CreateUserInput(
                "Anderson",
                "Amaral",
                26,
                "42469323345",
                "15999999",
                "Rua do code", "Cidade", "SPS", "BR", Guid.NewGuid());
            var logger = new Mock<ILogger<CreateUserInput>>();
            var repository = new Mock<ISubscriptionRepository>();

            repository
                .Setup(item => item.CreateUserOnDatabaseasync(It.IsAny<User>(), CancellationToken.None))
                .Throws(new Exception("Some Error"));

            var handler = new CreateUserHandler(logger.Object, repository.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.NotNull(response);
            Assert.False(response.Meta.IsValid);
        }
    }
}

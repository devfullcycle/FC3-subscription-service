using Microsoft.Extensions.Logging;
using Moq;
using Subscription.Application.Adapters.AuthService;
using Subscription.Application.Adapters.PaymentGateway;
using Subscription.Application.Adapters.Repository;
using Subscription.Application.Handlers.Comand.CreateUserPayment;
using Subscription.Domain.Entities;
using Subscription.Domain.Enum;
using Subscription.Domain.ValueObject;

namespace Subscription.Unit.Test.Handlers.Comand
{
    public class CreateUserPaymentHandlerTest
    {
        [Fact]
        public async Task CreatePaymentWithSuccess()
        {
            var address = Address.Create("12345678", "rua tres quatro", "cidade", "SP", "BR");
            var doc = Document.Create("12345678911");
            var input = new CreateUserPaymentInput(
                Guid.NewGuid(),
                PlanType.ANNUALLY,
                100,
                Guid.NewGuid(), "user@user.com",PaymentType.CreditCard, Guid.NewGuid());
            var logger = new Mock<ILogger<CreateUserPaymentInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var gateway = new Mock<IPaymentGateway>();
            var keycloak = new Mock<IAuthorizationService>();
            var planResponse = Plan.CreateAnnually("planb1", "plan1 é o plan1");
            var plancost = PlanCost.Create(100, planResponse.Id);
            planResponse.AddPlanCost(plancost);

            repository.Setup(item => item.GetUserByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(User.Create("Jose", "Aparecido", 31, address, doc));
            repository.Setup(item => item.GetPlanByIdAsync(It.IsAny<Guid>(), It.IsAny<PlanType>(), CancellationToken.None))
                .ReturnsAsync(planResponse);
            repository.Setup(item => item.CheckIfUserHasActiveSubscription(It.IsAny<Guid>(), It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(false);
            repository
                .Setup(item => item.CreateSubscriptionOnDatabaseasync(It.IsAny<UserSubscription>(), CancellationToken.None))
                .ReturnsAsync(UserSubscription.Create(User.Create("Jose", "Aparecido", 31, address, doc), planResponse,DateTime.Now));
            gateway.Setup(item => item.MakePaymentAsync(It.IsAny<PaymentInput>(), CancellationToken.None)).
                ReturnsAsync(true);


            var handler = new CreateUserPaymentHandler(logger.Object, repository.Object, gateway.Object, keycloak.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.True(response.Meta.IsValid);
        }

        [Fact]
        public async Task CreatePaymentWithActiveSubscriptionSuccess()
        {
            var address = Address.Create("12345678", "rua tres quatro", "cidade", "SP", "BR");
            var doc = Document.Create("12345678911");
            var input = new CreateUserPaymentInput(
                Guid.NewGuid(),
                PlanType.ANNUALLY,
                100,
                Guid.NewGuid(), "user@user.com", PaymentType.CreditCard, Guid.NewGuid());
            var logger = new Mock<ILogger<CreateUserPaymentInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var gateway = new Mock<IPaymentGateway>();
            var keycloak = new Mock<IAuthorizationService>();
            var planResponse = Plan.CreateAnnually("planb1", "plan1 é o plan1");
            var plancost = PlanCost.Create(100, planResponse.Id);
            planResponse.AddPlanCost(plancost);

            repository.Setup(item => item.GetUserByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(User.Create("Jose", "Aparecido", 31, address, doc));
            repository.Setup(item => item.GetPlanByIdAsync(It.IsAny<Guid>(), It.IsAny<PlanType>(), CancellationToken.None))
                .ReturnsAsync(planResponse);
            repository.Setup(item => item.CheckIfUserHasActiveSubscription(It.IsAny<Guid>(), It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(true);
            repository
                .Setup(item => item.CreateSubscriptionOnDatabaseasync(It.IsAny<UserSubscription>(), CancellationToken.None))
                .ReturnsAsync(UserSubscription.Create(User.Create("Jose", "Aparecido", 31, address, doc), planResponse, DateTime.Now));
            gateway.Setup(item => item.MakePaymentAsync(It.IsAny<PaymentInput>(), CancellationToken.None)).
                ReturnsAsync(true);


            var handler = new CreateUserPaymentHandler(logger.Object, repository.Object, gateway.Object, keycloak.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.False(response.Meta.IsValid);
        }

        [Fact]
        public async Task CreatePaymentWithInvalidInput()
        {
            var address = Address.Create("123456", "rua tres quatro", "cidade", "SP", "BR");
            var doc = Document.Create("12345678911");
            var input = new CreateUserPaymentInput(
                Guid.NewGuid(),
                PlanType.ANNUALLY,
                100,
                Guid.NewGuid(), "user@user.com", PaymentType.CreditCard, Guid.NewGuid());
            var logger = new Mock<ILogger<CreateUserPaymentInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var keycloak = new Mock<IAuthorizationService>();
            var gateway = new Mock<IPaymentGateway>();
            var planResponse = Plan.CreateAnnually("planb1", "plan1 é o plan1");
            var plancost = PlanCost.Create(100, planResponse.Id);
            planResponse.AddPlanCost(plancost);

            repository.Setup(item => item.GetUserByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(User.Create("Jose", "Aparecido", 31, address, doc));
            repository.Setup(item => item.GetPlanByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(planResponse);
            gateway.Setup(item => item.MakePaymentAsync(It.IsAny<PaymentInput>(), CancellationToken.None)).
                ReturnsAsync(true);

            var handler = new CreateUserPaymentHandler(logger.Object, repository.Object, gateway.Object, keycloak.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.False(response.Meta.IsValid);
        }

        [Fact]
        public async Task CreatePaymentWithThrowsInput()
        {
            var address = Address.Create("123456", "rua tres quatro", "cidade", "SP", "BR");
            var doc = Document.Create("12345678911");
            var input = new CreateUserPaymentInput(
                Guid.NewGuid(),
                PlanType.ANNUALLY,
                100,
                Guid.NewGuid(), "user@user.com", PaymentType.CreditCard, Guid.NewGuid());
            var logger = new Mock<ILogger<CreateUserPaymentInput>>();
            var repository = new Mock<ISubscriptionRepository>();
            var gateway = new Mock<IPaymentGateway>();
            var keycloak = new Mock<IAuthorizationService>();
            var planResponse = Plan.CreateAnnually("planb1", "plan1 é o plan1");
            var plancost = PlanCost.Create(100, planResponse.Id);
            planResponse.AddPlanCost(plancost);

            repository.Setup(item => item.GetUserByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ThrowsAsync(new Exception("Some error ocurred"));
            repository.Setup(item => item.GetPlanByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(planResponse);
            gateway.Setup(item => item.MakePaymentAsync(It.IsAny<PaymentInput>(), CancellationToken.None)).
                ReturnsAsync(true);

            var handler = new CreateUserPaymentHandler(logger.Object, repository.Object, gateway.Object, keycloak.Object);

            var response = await handler.HandleExecutionAsync(input, CancellationToken.None);

            Assert.False(response.Meta.IsValid);
        }
    }
}

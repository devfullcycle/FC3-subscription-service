using Subscription.Domain.Entities;
using Subscription.Domain.ValueObject;

namespace Subscription.Unit.Test.Entities
{
    public class UserSubscriptionUnitTest
    {
        [Fact]
        public void CreateWithSuccess()
        {
            var address = Address.Create("15954123", "street", "city", "SP", "BR");
            var document = Document.Create("12345678910");
            var user = User.Create("Name", "LastName", 18, address, document);
            var plan = Plan.CreateMonthly("plan1", "plan 1 is plan 1");
            plan.AddPlanCost(PlanCost.Create(100, plan.Id));

            var subscription = UserSubscription.Create(user, plan, DateTime.Now, true, false);

            Assert.True(subscription.IsValid);
        }

        [Fact]
        public void CreateWithWrongUser()
        {
            var address = Address.Create("15954123", "street", "city", "SP", "BR");
            var document = Document.Create("12345678910");
            var user = User.Create("Na", "LastName", 18, address, document);
            var plan = Plan.CreateMonthly("plan1", "plan 1 is plan 1");
            plan.AddPlanCost(PlanCost.Create(100, plan.Id));

            var subscription = UserSubscription.Create(user, plan, DateTime.Now, true, false);

            Assert.False(subscription.IsValid);
            Assert.Equal("Nome incorreto, minimo 3 caracteres", subscription.Notifications.First());
        }

        [Fact]
        public void CreateWithWrongPlan()
        {
            var address = Address.Create("15954123", "street", "city", "SP", "BR");
            var document = Document.Create("12345678910");
            var user = User.Create("Name", "LastName", 18, address, document);
            var plan = Plan.CreateMonthly("pl", "plan 1 is plan 1");
            plan.AddPlanCost(PlanCost.Create(100, plan.Id));

            var subscription = UserSubscription.Create(user, plan, DateTime.Now, true, false);

            Assert.False(subscription.IsValid);
            Assert.Equal("Nome do plano incompleto, minimo 3 caracteres", subscription.Notifications.First());
        }

        [Fact]
        public void CreateWithWrongBillingDate()
        {
            var address = Address.Create("15954123", "street", "city", "SP", "BR");
            var document = Document.Create("12345678910");
            var user = User.Create("Name", "LastName", 18, address, document);
            var plan = Plan.CreateMonthly("plan1", "plan 1 is plan 1");
            plan.AddPlanCost(PlanCost.Create(100, plan.Id));

            var subscription = UserSubscription.Create(user, plan, DateTime.Now.AddMinutes(5), true, false);

            Assert.False(subscription.IsValid);
            Assert.Equal("Billing Date Invalid", subscription.Notifications.First());
        }
    }
}

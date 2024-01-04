using Subscription.Domain.Entities;
using Subscription.Domain.Enum;

namespace Subscription.Unit.Test.Entities
{
    public class PlanUnitTest
    {
        [Fact]
        public void CreateWithAnnuallySuccess()
        {
            var plan = Plan.CreateAnnually("plan1", "plan 1 é o plano 1");
            var cost = PlanCost.Create(100, plan.Id);
            plan.AddPlanCost(cost);

            Assert.True(plan.IsValid);
            Assert.Equal(PlanType.ANNUALLY, plan.PlanType);
        }

        [Fact]
        public void CreateWithErrorMessage()
        {
            var plan = Plan.CreateWithErrorMessage("Some error");
            var cost = PlanCost.Create(100, plan.Id);
            plan.AddPlanCost(cost);

            Assert.False(plan.IsValid);
            Assert.Equal("Some error", plan.Notifications.First());
        }

        [Fact]
        public void CreateWithMonthlySuccess()
        {
            var plan = Plan.CreateMonthly("plan1", "plan 1 é o plano 1");
            var cost = PlanCost.Create(100, plan.Id);
            plan.AddPlanCost(cost);

            Assert.True(plan.IsValid);
            Assert.Equal(PlanType.MONTHLY, plan.PlanType);
        }

        [Fact]
        public void CreateWithWrongName()
        {
            var plan = Plan.CreateMonthly("pl", "plan 1 é o plano 1");
            var cost = PlanCost.Create(100, plan.Id);
            plan.AddPlanCost(cost);

            Assert.False(plan.IsValid);
            Assert.Equal("Nome do plano incompleto, minimo 3 caracteres", plan.Notifications.First());
        }

        [Fact]
        public void CreateWithWrongDescription()
        {
            var plan = Plan.CreateMonthly("plas", "pl");
            var cost = PlanCost.Create(100, plan.Id);
            plan.AddPlanCost(cost);

            Assert.False(plan.IsValid);
            Assert.Equal("Descricao do plano incompleta, minimo 3 caracteres", plan.Notifications.First());
        }

        [Fact]
        public void CreateWithWrongCost()
        {
            var plan = Plan.CreateMonthly("plan", "plana");
            var cost = PlanCost.Create(0, plan.Id);
            plan.AddPlanCost(cost);

            Assert.False(plan.IsValid);
            Assert.Equal("Preço invalido, deve ser maior que zero", plan.Notifications.First());
        }
    }
}

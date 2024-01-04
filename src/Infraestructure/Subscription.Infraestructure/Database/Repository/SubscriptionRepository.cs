using Dapper;
using Microsoft.Extensions.Logging;
using Subscription.Application.Abstractions.Handlers;
using Subscription.Application.Adapters.Repository;
using Subscription.Application.Handlers.Query.GetSubscriber;
using Subscription.Application.Handlers.Query.GetSusbscribers;
using Subscription.Domain.Entities;
using Subscription.Domain.Enum;
using Subscription.Domain.ValueObject;
using Subscription.Infraestructure.Abstractions.Database;
using System.Numerics;

namespace Subscription.Infraestructure.Database.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ILogger<SubscriptionRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionRepository(ILogger<SubscriptionRepository> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckIfUserHasActiveSubscription(Guid userId, Guid PlanId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting create a plan on database");

                var session = _unitOfWork.BeginConnection();

                var response = await session.QueryAsync<dynamic>(
                    Queries.CheckIfSubscriptionExists, param: new
                {
                    userId,
                    PlanId
                });

                _unitOfWork.Commit();

                _logger.LogInformation("Plan created");

                return response.Any();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);
                return true;
            }
        }

        public async Task<Plan> CreatePlanAsync(Plan plan, CancellationToken cancellationToken)
        {
            Plan response = new();
            try
            {
                _logger.LogInformation("Starting create a plan on database");

                var session = _unitOfWork.BeginConnection();

                var planType = plan.PlanType.ToString();

                response = await session.QuerySingleAsync<Plan>(Queries.CreatePlan, param: new
                {
                    plan.Name,
                    plan.Description,
                    plan.IsActive,
                    period = planType
                });

                _unitOfWork.Commit();

                _logger.LogInformation("Plan created");

                return response;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddNotification("Some error ocurred when trying get data");
                return response;
            }
        }

        public async Task<PlanCost> CreatePlanCostAsync(PlanCost planCost, CancellationToken cancellationToken)
        {
            PlanCost? response = new();
            try
            {
                _logger.LogInformation("Starting create a planCost on database");

                var session = _unitOfWork.BeginConnection();


                response = await session.QuerySingleOrDefaultAsync<PlanCost>(Queries.CreatePlanCost, param: new
                {
                    planCost.Price,
                    planCost.PlanId,
                    isactive = true
                });

                _unitOfWork.Commit();

                _logger.LogInformation("PlanCost created");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddNotification("Some error ocurred when trying get data");
                return response;
            }
        }

        public async Task<UserSubscription> CreateSubscriptionOnDatabaseasync(UserSubscription userSub, CancellationToken cancellationToken)
        {
            UserSubscription? response = new();
            try
            {
                _logger.LogInformation("Starting create a user subscription on database");

                var session = _unitOfWork.BeginConnection();


                response = await session.QuerySingleOrDefaultAsync<UserSubscription>(Queries.CreateSubscription, param: new
                {
                    planId = userSub.Plan.Id,
                    userId = userSub.User.Id,
                    price = userSub.Plan.PlanCosts[0].Price,
                    lastbilling = DateTime.Now,
                    isactive = true,
                    cancelled = false

                });

                _unitOfWork.Commit();

                _logger.LogInformation("PlanCost created");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddNotification("Some error ocurred when trying get data");
                return response;
            }
        }

        public async Task<User> CreateUserOnDatabaseasync(User user, CancellationToken cancellationToken)
        {
            User? response = new();
            try
            {
                _logger.LogInformation("Starting creating a user on database");

                var session = _unitOfWork.BeginConnection();


                response = await session.QuerySingleOrDefaultAsync<User>(Queries.CreateUser, param: new
                {
                    user.Name,
                    user.LastName,
                    user.Age,
                    user.Address.ZipCode,
                    user.Address.Street,
                    user.Address.City,
                    user.Address.State,
                    user.Address.Country,
                    user.Document.DocumentNumber,
                    user.Document.DocumentType
                });

                _unitOfWork.Commit();

                _logger.LogInformation("User getted");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddNotification("Some error ocurred when trying get data");
                return response;
            }
        }

        public async Task<Plan> GetPlanByIdAsync(Guid Id, PlanType planType,CancellationToken cancellationToken)
        {
            Plan? response = new();
            try
            {
                _logger.LogInformation("Starting getting a plan on database");

                var session = _unitOfWork.BeginConnection();

                var splanType = planType.ToString();

                var data = await session.QueryAsync<Plan, PlanCost , Plan>(
                Queries.GetPlanById,
                (plan, plancos) =>
                {
                    plan.AddPlanCost(plancos);
                    return plan;
                },
                param: new
                {
                    Id,
                    PlanType = splanType
                },
                splitOn: "planid");

                response = data.FirstOrDefault();

                _unitOfWork.Commit();

                _logger.LogInformation("Plan getted");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddNotification("Some error ocurred when trying get data");
                return response;
            }
        }

        public async Task<Plan> GetPlanByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            Plan? response = new();
            try
            {
                _logger.LogInformation("Starting getting a plan on database");

                var session = _unitOfWork.BeginConnection();


                response = await session.QuerySingleOrDefaultAsync<Plan>(Queries.GetOnlyPlanById, param: new
                {
                    Id
                });

                _unitOfWork.Commit();

                _logger.LogInformation("Plan getted");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddNotification("Some error ocurred when trying get data");
                return response;
            }
        }

        public async Task<List<Plan>> GetPlans(int page, int pagesize, CancellationToken cancellationToken)
        {
            List<Plan>? response = new();
            try
            {
                _logger.LogInformation("Starting getting a plan on database");

                var session = _unitOfWork.BeginConnection();


                var data = await session.QueryAsync<Plan, PlanCost, Plan>(
                Queries.GetPlanPaginated,               
                (plan, plancost) =>
                {
                    if (plancost is not null)
                        plan.AddPlanCost(plancost);
                    return plan;

                },
                param: new
                {
                    page,
                    size = pagesize
                },
                splitOn: "planid");

                _unitOfWork.Commit();

                response = data.ToList();

                _logger.LogInformation("Plan getted");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);
                var data = Plan.CreateWithErrorMessage("Internal error on repository");
                response.Add(data);
                return response;
            }
        }

        public async Task<MetaData> GetPlansPaginated(int page, int pagesize, CancellationToken cancellationToken)
        {
            MetaData response = new();
            try
            {
                _logger.LogInformation("GetPlansPaginated process started");
                var session = _unitOfWork.BeginConnection();

                var data = await session.QueryAsync<int>("SELECT COUNT(planid) FROM plan WHERE isactive = TRUE");

                _unitOfWork.Commit();

                if (!data.Any())
                    return response;
                
                response.AddPagination(pagesize, page, data.First());

                return response;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddMessage("Some error ocurred when trying get data");
                return response;
            }
        }

        public async Task<GetSubscriberOutput> GetSubscriberById(Guid id, CancellationToken cancellationToken)
        {
            GetSubscriberOutput? response = new();
            try
            {
                _logger.LogInformation("Starting getting a plan on database");

                var session = _unitOfWork.BeginConnection();

                response = await session.QuerySingleOrDefaultAsync<GetSubscriberOutput>(
                Queries.GetSubscriberById,
                param: new
                {
                   id
                });

                _unitOfWork.Commit();

                _logger.LogInformation("Plan getted");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);
                return response ?? new();
            }
        }

        public async Task<GetSubscribersOutput> GetSubscribers(int page, int size, CancellationToken cancellationToken)
        {
            GetSubscribersOutput? response = new();
            try
            {
                _logger.LogInformation("Starting getting a plan on database");

                var session = _unitOfWork.BeginConnection();

                var pageinit = page == 1 ? 0 : page;

                var data = await session.QueryAsync<GetSubscribersResumeOutput>(
                Queries.GetSubscribers,
                param: new
                {
                    page = pageinit,
                    size
                });

                _unitOfWork.Commit();

                response.AddSubscriber(data.ToList());

                _logger.LogInformation("Plan getted");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);
                return response ?? new();
            }
        }

        public async Task<MetaData> GetSubscribersPaginated(int page, int size, CancellationToken cancellationToken)
        {
            MetaData response = new();
            try
            {
                _logger.LogInformation("GetPlansPaginated process started");
                var session = _unitOfWork.BeginConnection();

                var data = await session.QueryAsync<int>("SELECT COUNT(userid) FROM usersubscription WHERE isactive = TRUE");

                _unitOfWork.Commit();

                if (!data.Any())
                    return response;

                response.AddPagination(size, page, data.First());

                return response;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddMessage("Some error ocurred when trying get data");
                return response;
            }
        }

        public async Task<User> GetUserByDocumentNumber(string documentNumber, CancellationToken cancellationToken)
        {
            User? response = new();
            try
            {
                _logger.LogInformation("Starting Getting a user on database");

                var session = _unitOfWork.BeginConnection();

                response = await session.QuerySingleOrDefaultAsync<User>(Queries.GetUserByDocumentNumber, param: new
                {
                   documentNumber
                });

                _unitOfWork.Commit();

                _logger.LogInformation("User getted");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddNotification("Some error ocurred when trying get data");
                return response;
            }
        }

        public async Task<User> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            User? response = new();
            try
            {
                _logger.LogInformation("Starting Getting a user on database");

                var session = _unitOfWork.BeginConnection();

                response = await session.QuerySingleOrDefaultAsync<User>(Queries.GetUserById, param: new
                {
                    Id
                });

                var doc = await session.QuerySingleOrDefaultAsync<Document>(Queries.GetUserById, param: new
                {
                    Id
                });

                var address = await session.QuerySingleOrDefaultAsync<Address>(Queries.GetUserById, param: new
                {
                    Id
                });

                response.ChangeAddress(address);
                response.ChangeDocument(doc);

                _unitOfWork.Commit();

                _logger.LogInformation("User getted");

                return response ?? new();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogInformation(ex.Message);

                response.AddNotification("Some error ocurred when trying get data");
                return response;
            }
        }
    }
}

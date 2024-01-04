namespace Subscription.Infraestructure.Database.Repository
{
    public static class Queries
    {
        public static string CreatePlan = @"
            INSERT INTO plan (
                name,
                description,
                isactive,
                period
            ) VALUES(
                @name,
                @description,
                @isactive,
                @period
            ) RETURNING *, planId as Id;
        ";

        public static string GetOnlyPlanById = @" Select *, planid as Id from plan where planId = @Id";

        public static string CheckIfSubscriptionExists = @"
                                select * from subscription 
                                where planId = @planid AND
                                userid = @userid and 
                                isactive = true";

        public static string GetPlanById = @"
               select 
                p.planid as Id,
                p.name as Name, 
                p.description as Description,
                p.isactive as IsActive,
                p.period as Period,
                    pc.planid as planId,
                    pc.price as Price,
                    pc.IsActive as Active
            from plan p
            inner join plancost pc ON
            pc.planid = p.planid AND
            pc.isactive = TRUE
            where p.planid = @id and 
            p.period = @planType
        ";

        public static string GetPlanPaginated = @"
             select 
                p.planid as Id,
                p.name as Name, 
                p.description as Description,
                p.isactive as IsActive,
                p.period as Period,
                    pc.planid as planId,
                    pc.price as Price,
                    pc.IsActive as IsActive
            from plan p
            left join plancost pc ON
            pc.planid = p.planid AND
            pc.isactive = TRUE AND
            p.isactive = TRUE
            ORDER BY pc.price desc
            LIMIT @size
            OFFSET @page";

        public static string CreatePlanCost = @"
        INSERT INTO plancost (
            price,
            planid,
            isactive
        ) VALUES(
            @price,
            @planid,
            @isactive
        ) RETURNING *, planId as Id";

        public static string CreateUser = @"
            INSERT INTO users (
                name,
                lastname,
                age,
                zipcode,
                street,
                city,
                state,
                country,
                documentnumber,
                documenttype
            ) VALUES(
                @name,
                @lastname,
                @age,
                @zipcode,
                @street,
                @city,
                @state,
                @country,
                @documentnumber,
                @documenttype
            ) RETURNING *, userid as Id; ";

        public static string GetUserByDocumentNumber = @"
            select *, userid as Id from users WHERE documentnumber = @documentnumber";
        public static string GetUserById = @"
            select *, userid as Id from users WHERE userId = @Id";

        public static string CreateSubscription = @"
            INSERT INTO subscription (
                userid,
                planid,
                price,
                lastbilling,
                isactive,
                cancelled
            ) VALUES(
                @userid,
                @planid,
                @price,
                @lastbilling,
                @isactive,
                @cancelled
            ) RETURNING *, userid as Id;";

        public static string GetSubscribers = @"
            SELECT
                    u.userid,
                    u.name,
                    u.lastname,
                    p.name as planname,
                    us.price,
                    us.isactive
            FROM users u
            LEFT JOIN usersubscription us ON
            u.userId = us.userId
            LEFT JOIN plan p ON
            p.planid = us.planId
            LIMIT @size
            OFFSET @page
        ";

        public static string GetSubscriberById = @"
             SELECT
                     u.userid,
                     u.name,
                     u.lastname,
                     u.age,
                     u.documentnumber,
                     u.zipcode,
                     u.street,
                     u.city,
                     u.state,
                     u.country,
                     p.name as planname,
                     p.description as plandescription,
                     us.price,
                     p.Period,
                     us.isactive
             FROM users u
             LEFT JOIN usersubscription us ON
             u.userId = us.userId
             LEFT JOIN plan p ON
             p.planid = us.planId
             WHERE u.userid = @id
        ";
    }
}

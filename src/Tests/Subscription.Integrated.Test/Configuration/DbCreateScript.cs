namespace Subscription.Integrated.Test.Configuration
{
    public class DbCreateScript
    {
        public static string DropDatabase = @"
            DO $$
                DECLARE 
                    table_name_ text;
                BEGIN
                    FOR table_name_ IN (
                                           SELECT table_name
                                            FROM information_schema.tables
                                            WHERE table_schema= 'public' AND
                                            table_catalog = 'subscription_test'
                                            AND table_type = 'BASE TABLE'
                                        )
                    LOOP
                        EXECUTE 'DROP TABLE IF EXISTS ' || table_name_ || ' CASCADE';
                    END LOOP;
            END $$
        ";

        public static string CreateUUID = @"
            --Criação da extensão UUID
            CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";
        ";
        public static string CreatePlanTable = @"
            -- Tabela Plan
            CREATE TABLE Plan (
                PlanID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                Name VARCHAR(255),
                Description TEXT,
                IsActive BOOLEAN,
                Period VARCHAR(255),
                CreatedAt TIMESTAMPTZ DEFAULT NOW(),
                UpdatedAt TIMESTAMPTZ DEFAULT NOW()
            );
        ";
        public static string CreatePlanCostTable = @"
         -- Tabela PlanCost
            CREATE TABLE PlanCost (
                Price DECIMAL(10, 2),
                PlanID UUID,
                IsActive BOOLEAN,
                CreatedAt TIMESTAMPTZ DEFAULT NOW(),
                UpdatedAt TIMESTAMPTZ DEFAULT NOW(),
                PRIMARY KEY (PlanID, CreatedAt),
                FOREIGN KEY (PlanID) REFERENCES Plan(PlanID)
            );
        ";
        public static string CreateUserTable = @"
          -- Tabela User
            CREATE TABLE ""users"" (
                UserID UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
                Name VARCHAR(255),
                LastName VARCHAR(255),
                Age INT,
                ZipCode VARCHAR(10),
                Street VARCHAR(255),
                City VARCHAR(255),
                State VARCHAR(255),
                Country VARCHAR(255),
                DocumentNumber VARCHAR(255),
                DocumentType VARCHAR(255),
                CreatedAt TIMESTAMPTZ DEFAULT NOW(),
                UpdatedAt TIMESTAMPTZ DEFAULT NOW()
            );
        ";
        public static string CreateUserSubscriptionTable = @"
         -- Tabela sbscription
            CREATE TABLE subscription (
                UserID UUID,
                PlanID UUID,
                Price DECIMAL(10, 2),
                LastBilling TIMESTAMPTZ,
                IsActive BOOLEAN,
                Cancelled BOOLEAN,
                PRIMARY KEY (UserID, PlanID),
                FOREIGN KEY (UserID) REFERENCES ""users""(UserID),
                FOREIGN KEY (PlanID) REFERENCES Plan(PlanID)
            );
        ";
        public static string CreateLogsTable = @"
            -- Tabela para logs de alterações na tabela User
            CREATE TABLE User_Log (
                LogID SERIAL PRIMARY KEY,
                UserID UUID,
                Name VARCHAR(255),
                LastName VARCHAR(255),
                Age INT,
                ZipCode VARCHAR(10),
                Street VARCHAR(255),
                City VARCHAR(255),
                State VARCHAR(255),
                Country VARCHAR(255),
                DocumentNumber VARCHAR(255),
                DocumentType VARCHAR(255),
                CreatedAt TIMESTAMPTZ,
                UpdatedAt TIMESTAMPTZ,
                Action VARCHAR(10),
                LogDate TIMESTAMPTZ DEFAULT NOW()
            );

            -- Tabela para logs de alterações na tabela Plan
            CREATE TABLE Plan_Log (
                LogID SERIAL PRIMARY KEY,
                PlanID UUID,
                Name VARCHAR(255),
                Description TEXT,
                IsActive BOOLEAN,
                Period VARCHAR(255),
                CreatedAt TIMESTAMPTZ,
                UpdatedAt TIMESTAMPTZ,
                Action VARCHAR(10),
                LogDate TIMESTAMPTZ DEFAULT NOW()
            );

            -- Tabela para logs de alterações na tabela PlanCost
            CREATE TABLE PlanCost_Log (
                LogID SERIAL PRIMARY KEY,
                Price DECIMAL(10, 2),
                PlanID UUID,
                IsActive BOOLEAN,
                CreatedAt TIMESTAMPTZ,
                UpdatedAt TIMESTAMPTZ,
                Action VARCHAR(10),
                LogDate TIMESTAMPTZ DEFAULT NOW()
            );

            -- Tabela para logs de alterações na tabela UserSubscription
            CREATE TABLE UserSubscription_Log (
                LogID SERIAL PRIMARY KEY,
                UserID UUID,
                PlanID UUID,
                Price DECIMAL(10, 2),
                LastBilling TIMESTAMPTZ,
                IsActive BOOLEAN,
                Cancelled BOOLEAN,
                Action VARCHAR(10),
                LogDate TIMESTAMPTZ DEFAULT NOW()
            );
        ";
        public static string CreateFunctionsTable = @"
            -- Gatilho para log de alterações na tabela User
            CREATE OR REPLACE FUNCTION user_log_trigger()
            RETURNS TRIGGER AS $$
            BEGIN
                INSERT INTO User_Log (UserID, Name, LastName, Age, ZipCode, Street, City, State, Country, DocumentNumber, DocumentType, CreatedAt, UpdatedAt, Action, LogDate)
                VALUES (OLD.UserID, OLD.Name, OLD.LastName, OLD.Age, OLD.ZipCode, OLD.Street, OLD.City, OLD.State, OLD.Country, OLD.DocumentNumber, OLD.DocumentType, OLD.CreatedAt, OLD.UpdatedAt, 'DELETE', NOW());
    
                INSERT INTO User_Log (UserID, Name, LastName, Age, ZipCode, Street, City, State, Country, DocumentNumber, DocumentType, CreatedAt, UpdatedAt, Action, LogDate)
                VALUES (NEW.UserID, NEW.Name, NEW.LastName, NEW.Age, NEW.ZipCode, NEW.Street, NEW.City, NEW.State, NEW.Country, NEW.DocumentNumber, NEW.DocumentType, NEW.CreatedAt, NEW.UpdatedAt, 'INSERT', NOW());
    
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;

            -- Gatilho para log de alterações na tabela Plan
            CREATE OR REPLACE FUNCTION plan_log_trigger()
            RETURNS TRIGGER AS $$
            BEGIN
                INSERT INTO Plan_Log (PlanID, Name, Description, IsActive, Period, CreatedAt, UpdatedAt, Action, LogDate)
                VALUES (OLD.PlanID, OLD.Name, OLD.Description, OLD.IsActive, OLD.Period, OLD.CreatedAt, OLD.UpdatedAt, 'DELETE', NOW());
    
                INSERT INTO Plan_Log (PlanID, Name, Description, IsActive, Period, CreatedAt, UpdatedAt, Action, LogDate)
                VALUES (NEW.PlanID, NEW.Name, NEW.Description, NEW.IsActive, NEW.Period, NEW.CreatedAt, NEW.UpdatedAt, 'INSERT', NOW());
    
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;

            -- Gatilho para log de alterações na tabela PlanCost
            CREATE OR REPLACE FUNCTION plancost_log_trigger()
            RETURNS TRIGGER AS $$
            BEGIN
                INSERT INTO PlanCost_Log (Price, PlanID, IsActive, CreatedAt, UpdatedAt, Action, LogDate)
                VALUES (OLD.Price, OLD.PlanID, OLD.IsActive, OLD.CreatedAt, OLD.UpdatedAt, 'DELETE', NOW());
    
                INSERT INTO PlanCost_Log (Price, PlanID, IsActive, CreatedAt, UpdatedAt, Action, LogDate)
                VALUES (NEW.Price, NEW.PlanID, NEW.IsActive, NEW.CreatedAt, NEW.UpdatedAt, 'INSERT', NOW());
    
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;

            -- Gatilho para log de alterações na tabela UserSubscription
            CREATE OR REPLACE FUNCTION usersubscription_log_trigger()
            RETURNS TRIGGER AS $$
            BEGIN
                INSERT INTO UserSubscription_Log (UserID, PlanID, Price, LastBilling, IsActive, Cancelled, Action, LogDate)
                VALUES (OLD.UserID, OLD.PlanID, OLD.Price, OLD.LastBilling, OLD.IsActive, OLD.Cancelled, 'DELETE', NOW());
    
                INSERT INTO UserSubscription_Log (UserID, PlanID, Price, LastBilling, IsActive, Cancelled, Action, LogDate)
                VALUES (NEW.UserID, NEW.PlanID, NEW.Price, NEW.LastBilling, NEW.IsActive, NEW.Cancelled, 'INSERT', NOW());
    
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;

            -- Gatilhos para log de alterações
            CREATE TRIGGER User_Log_Trigger
            AFTER INSERT OR DELETE ON ""users""
            FOR EACH ROW EXECUTE FUNCTION user_log_trigger();

            CREATE TRIGGER Plan_Log_Trigger
            AFTER INSERT OR DELETE ON Plan
            FOR EACH ROW EXECUTE FUNCTION plan_log_trigger();

            CREATE TRIGGER PlanCost_Log_Trigger
            AFTER INSERT OR DELETE ON PlanCost
            FOR EACH ROW EXECUTE FUNCTION plancost_log_trigger();

            CREATE TRIGGER UserSubscription_Log_Trigger
            AFTER INSERT OR DELETE ON subscription
            FOR EACH ROW EXECUTE FUNCTION usersubscription_log_trigger();
        ";

    }
}

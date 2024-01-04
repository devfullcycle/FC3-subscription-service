CREATE DATABASE subscription;
CREATE DATABASE subscription_test;

\c subscription;

--Criação da extensão UUID
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

--TABLE USER
CREATE TABLE "users"(
    UserId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    Name VARCHAR(255),
    LastName VARCHAR(255),
    Age Int,
    ZipCode VARCHAR(10),
    Street VARCHAR(255),
    City VARCHAR(255),
    State VARCHAR(255),
    Country VARCHAR(255),
    DocumentNumber Varchar(255),
    DocumentType VARCHAR(255),
    CreatedAt TIMESTAMPTZ DEFAULT NOW(),
    UpdatedAt TIMESTAMPTZ DEFAULT NOW(),
);

--TABLE PLAN 
CREATE TABLE Plan(
    PlanId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    Name VARCHAR(255),
    Description VARCHAR(255),
    IsActive BOOLEAN,
    Period VARCHAR(255),
    CreatedAt TIMESTAMPTZ DEFAULT NOW(),
    UpdatedAt TIMESTAMPTZ DEFAULT NOW(),
);

-- TABLE PLANCOST
CREATE TABLE PlanCost(
    PlanCostId UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    PlanId UUID,
    IsActive BOOLEAN,
    Price Decimal(10,2),
    CreatedAt TIMESTAMPTZ DEFAULT NOW(),
    UpdatedAt TIMESTAMPTZ DEFAULT NOW(),
    FOREIGN KEY (PlanId) REFERENCES Plan(PlanId)
);

--TABLE SUBSCRIPTION

CREATE TABLE Subscription(
    PlanId UUID,
    UserId UUID,
    Price DECIMAL(10,2),
    LastBilling TIMESTAMPTZ,
    IsActive BOOLEAN,
    Cancelled BOOLEAN,
    CreatedAt TIMESTAMPTZ DEFAULT NOW(),
    UpdatedAt TIMESTAMPTZ DEFAULT NOW(),
    PRIMARY KEY(UserId,PlanId),
    FOREIGN KEY (UserId) REFERENCES "users"(UserId),
    FOREIGN KEY (PlanId) REFERENCES "Plan"(PlanId)
);

--Trigger log na table users
CREATE OR REPLACE FUNCTION user_log_trigger()
Returns TRIGGER AS $$
BEGIN
    INSERT INTO User_Log(UserId, Name, LastName, Age, ZipCode, Street,City,State,Country,DocumentNumber,DocumentType,CreatedAt,UpdatedAt,Action,LogDate)
    Values(OLD.UserId, OLD.Name, OLD.LastName, OLD.Age, OLD.ZipCode, OLD.Street,OLD.City,OLD.State,OLD.Country,OLD.DocumentNumber,OLD.DocumentType,OLD.CreatedAt,OLD.UpdatedAt,'DELETE',NOW());

    INSERT INTO User_Log(UserId, Name, LastName, Age, ZipCode, Street,City,State,Country,DocumentNumber,DocumentType,CreatedAt,UpdatedAt,Action,LogDate)
    Values(NEW.UserId, NEW.Name, NEW.LastName, NEW.Age, NEW.ZipCode, NEW.Street,NEW.City,NEW.State,NEW.Country,NEW.DocumentNumber,NEW.DocumentType,NEW.CreatedAt,NEW.UpdatedAt,'INSERT',NOW());
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION plan_log_trigger()
Returns TRIGGER AS $$
BEGIN
    INSERT INTO Plan_Log(PlanId, Name, Description, IsActive,CreatedAt,UpdatedAt,Action,LogDate)
    Values(OLD.PlanId, OLD.Name, OLD.Description, OLD.IsActive, OLD.CreatedAt, OLD.UpdatedAt,'DELETE',NOW());

    INSERT INTO Plan_Log(PlanId, Name, Description, IsActive,CreatedAt,UpdatedAt,Action,LogDate)
    Values(NEW.PlanId, NEW.Name, NEW.Description, NEW.IsActive, NEW.CreatedAt, NEW.UpdatedAt,'INSERT',NOW());
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION plancost_log_trigger()
Returns TRIGGER AS $$
BEGIN
    INSERT INTO PlanCost_Log(PlanCostId,PlanId , Price,IsActive,CreatedAt,UpdatedAt,Action,LogDate)
    Values(OLD.PlanCostId, OLD.PlanId, OLD.Price, OLD.IsActive, OLD.CreatedAt, OLD.UpdatedAt,'DELETE',NOW());

    INSERT INTO PlanCost_Log(PlanCostId,PlanId , Price,IsActive,CreatedAt,UpdatedAt,Action,LogDate)
    Values(NEW.PlanCostId, NEW.PlanId, NEW.Price, NEW.IsActive, NEW.CreatedAt, NEW.UpdatedAt,'INSERT',NOW();
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION subscription_log_trigger()
Returns TRIGGER AS $$
BEGIN
    INSERT INTO Subscription_Log(PlanId,UserId ,Price,IsActive,Cancelled,CreatedAt,UpdatedAt,Action,LogDate)
    Values(OLD.PlanId, OLD.UserId, OLD.Price, OLD.IsActive, OLD.Cancelled,OLD.CreatedAt, OLD.UpdatedAt,'DELETE',NOW());

    INSERT INTO Subscription_Log(PlanId,UserId ,Price,IsActive,Cancelled,CreatedAt,UpdatedAt,Action,LogDate)
    Values(NEW.PlanId, NEW.UserId, NEW.Price, NEW.IsActive, NEW.Cancelled,NEW.CreatedAt, NEW.UpdatedAt,'INSERT',NOW();
END;
$$ LANGUAGE plpgsql;

CREATE TABLE User_Log(
    LogId SERIAL Primary key,
    UserId UUID,
    Name VARCHAR(255),
    LastName VARCHAR(255),
    Age Int,
    ZipCode VARCHAR(10),
    Street VARCHAR(255),
    City VARCHAR(255),
    State VARCHAR(255),
    Country VARCHAR(255),
    DocumentNumber Varchar(255),
    DocumentType VARCHAR(255),
    CreatedAt TIMESTAMPTZ,
    UpdatedAt TIMESTAMPTZ,
    Action VARCHAR(10),
    LogDate TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE Plan_Log(
    LogId SERIAL Primary key,
    PlanId UUID,
    Name VARCHAR(255),
    Description VARCHAR(255),
    IsActive BOOLEAN,
    Period VARCHAR(255),
    CreatedAt TIMESTAMPTZ,
    UpdatedAt TIMESTAMPTZ,
    LogDate TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE PlanCost_Log(
    LogId SERIAL Primary key,
    PlanCostId UUID,
    PlanId UUID,
    IsActive BOOLEAN,
    Price Decimal(10,2),
    CreatedAt TIMESTAMPTZ,
    UpdatedAt TIMESTAMPTZ,
    LogDate TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE Subscription_Log(
    LogId SERIAL Primary key,
    PlanId UUID,
    UserId UUID,
    Price DECIMAL(10,2),
    LastBilling TIMESTAMPTZ,
    IsActive BOOLEAN,
    Cancelled BOOLEAN,
    CreatedAt TIMESTAMPTZ,
    UpdatedAt TIMESTAMPTZ,
    LogDate TIMESTAMPTZ DEFAULT NOW()
);

CREATE TRIGGER User_log_trigger
AFTER INSERT OR DELETE ON "users"
FOR EACH ROW EXECUTE FUNCTION user_log_trigger();

CREATE TRIGGER Plan_log_trigger
AFTER INSERT OR DELETE ON "Plan"
FOR EACH ROW EXECUTE FUNCTION plan_log_trigger();

CREATE TRIGGER Plancost_log_trigger
AFTER INSERT OR DELETE ON "PlanCost"
FOR EACH ROW EXECUTE FUNCTION plancost_log_trigger();

CREATE TRIGGER Subscription_log_trigger
AFTER INSERT OR DELETE ON "Subscription"
FOR EACH ROW EXECUTE FUNCTION subscription_log_trigger();
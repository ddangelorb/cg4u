CREATE DATABASE CG4UCore
--When configuring server read and follow that: 
--	https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/managing-permissions-with-stored-procedures-in-sql-server

CREATE LOGIN cg4uWebUser WITH PASSWORD = 'b5e2847d-f284-47c0-9e80-e84e052b2e28'
CREATE USER cg4uWebUser FOR LOGIN cg4uWebUser

GRANT SELECT ON "dbo"."UserSystemPreferences" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."UsersSystems" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."Users" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."Languages" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."Systems" TO "cg4uWebUser"

GRANT EXECUTE ON dbo.USP_INS_DonationsGivens TO cg4uWebUser
GRANT EXECUTE ON dbo.USP_SEL_DonationsGivensByDistance TO cg4uWebUser

USE CG4UCore

CREATE TABLE Systems (
	Id int not NULL ,
	Name nvarchar(50) not null,
	DtUpdate datetime not null,
	Active bit not null,
    PRIMARY KEY (Id)
);

CREATE TABLE Languages (
	Id int not null ,
	Name nvarchar(50) not null,
	Code varchar(10) not null,
	DtUpdate datetime not null,
	Active bit not null,
    PRIMARY KEY (Id),
    CONSTRAINT Code_Languages UNIQUE (Code)
);

/*
 * TODO
 * 
CREATE TABLE Customers (
	Id int not null ,
	DocumentRevenue nvarchar(14) null,
	Name nvarchar(256) not null,
	PersonType char(1) not null,
	Active bit not null,
    PRIMARY KEY (Id),
    CONSTRAINT Chk_PersonType CHECK (PersonType IN ('F', 'J'))
); 
 
CREATE TABLE Users (
	Id int not null ,
	IdCustomers int null,
	DocumentID nvarchar(14) not null
	DocumentRevenue nvarchar(14) null,
	FirstName nvarchar(50) not null,
	SurName nvarchar(206) not null,
	PersonType char(1) not null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdCustomers) REFERENCES Customers(Id),
    CONSTRAINT DocumentID_Users UNIQUE (DocumentID),
    CONSTRAINT Chk_PersonType CHECK (PersonType IN ('F', 'J'))
);

CREATE UNIQUE NONCLUSTERED INDEX IDX_DocumentRevenue_Users_NotNull
ON Users(DocumentRevenue)
WHERE DocumentRevenue IS NOT NULL;

CREATE TABLE UsersLogins (
	Id int not null ,
	IdUsers int not null,
	IdUserIdentity nvarchar(450) not null ,
	Email nvarchar(256) not null,
	Avatar varBinary(Max) null,
	Authenticated bit not null,
	DtExpAuth datetime null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdUsers) REFERENCES Users(Id),
    CONSTRAINT IdUserIdentity_Users UNIQUE (IdUserIdentity),
    CONSTRAINT Email_Users UNIQUE (Email)
);

CREATE TABLE UsersAddresses (
	Id int not null,
	IdUsers int not null,
	AddressType char(1) not null,
	Address nvarchar(255) not null,
	City nvarchar(50) not null,
	State nvarchar(50) not null,
	ZipCode nvarchar(20) not null,
	DefaultGet bit not null,
	DefaultLet bit not null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdUsers) REFERENCES Users(Id),
    CONSTRAINT Chk_AddressType CHECK (AddressType IN ('H', 'W'))
);

--Move UsersAddresses.DefaultGet, UsersAddresses.DefaultLet and UserSystemPreferences to CG4UDonate database

CREATE TABLE BillableProcesses (
	Id int not NULL ,
	IdSystems int not null,
	Name nvarchar(50) not null,
	BRLUnitCostPrice decimal(5,2) not null,
	USDUnitCostPrice decimal(5,2) not null,
	BRLUnitChargedPrice decimal(5,2) not null,
	USDUnitChargedPrice decimal(5,2) not null,
	Active bit not null,
	PRIMARY KEY (Id),
	FOREIGN KEY (IdSystems) REFERENCES Systems(Id)
);

CREATE TABLE BillableResponses (
	Id int not NULL ,
	IdBillableProcesses int not null,
	IdUsers int not not null,
	IdCustomers int not null,
	DtResponse datetime not null,
	IPOrigin varchar(20) not null,
	Active bit not null,
	PRIMARY KEY (Id)
	FOREIGN KEY (IdBillableProcesses) REFERENCES BillableProcesses(Id),
	FOREIGN KEY (IdUsers) REFERENCES Users(Id),
	FOREIGN KEY (IdCustomers) REFERENCES Customers(Id)
);

 * */
CREATE TABLE Users (
	Id int not null ,
	IdUserIdentity nvarchar(450) not null ,
	Email nvarchar(256) not null,
	FirstName nvarchar(50) not null,
	SurName nvarchar(206) not null,
	Avatar varBinary(Max) null, --Avatar IMAGE null, --Avatar varBinary(Max) null,
	Authenticated bit not null,
	DtExpAuth datetime null,
	Active bit not null,
    PRIMARY KEY (Id),
    CONSTRAINT IdUserIdentity_Users UNIQUE (IdUserIdentity),
    CONSTRAINT Email_Users UNIQUE (Email)
);

CREATE TABLE UsersSystems (
	Id int not null ,
	IdUsers int not null,
	IdSystems int not null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdUsers) REFERENCES Users(Id),
    FOREIGN KEY (IdSystems) REFERENCES Systems(Id),
);

CREATE TABLE UserSystemPreferences (
	Id int not null ,
	IdUsersSystems int not null,
	MaxGetinMeters int null,
	MaxLetinMeters int null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdUsersSystems) REFERENCES UsersSystems(Id)
);

/*
USE CG4UCore
sp_help Users
sp_helptext USP_INS_Users
sp_helptext USP_UPD_Users
sp_helptext USP_INS_UsersSystems

ALTER TABLE Users
ALTER COLUMN Avatar varbinary(max)

EXEC USP_INS_UsersSystems @IdUsers = 1, @IdSystems = 1

DROP TABLE  UserSystemPreferences
DROP TABLE  UsersSystems
DROP TABLE  Users
DROP TABLE  Languages
DROP TABLE Systems

USE CG4UCore
--SELECT u.*, us.IdSystems FROM Users u INNER JOIN UsersSystems us ON u.Id = us.IdUsers WHERE u.IdUserIdentity=@idUserIdentity AND us.IdSystems = @idSystems AND u.Active=1 AND us.Active = 1
SELECT * FROM Users
SELECT * FROM UsersSystems
SELECT * FROM UserSystemPreferences
SELECT * FROM Languages
SELECT * FROM Systems  

SELECT * FROM Users WHERE IdUserIdentity='3c1aa042-562e-4a6f-b38b-02e7c6b52610' AND Active=1

DROP PROCEDURE USP_INS_DonationsDesired
DROP PROCEDURE USP_INS_DonationsGiven
DROP PROCEDURE USP_INS_TradeEvaluations
DROP PROCEDURE USP_INS_TradeLocations
DROP PROCEDURE USP_INS_Trades
DROP PROCEDURE USP_SEL_DonationsById
DROP PROCEDURE USP_SEL_DonationsDesiredAll
DROP PROCEDURE USP_SEL_DonationsDesiredById
DROP PROCEDURE USP_SEL_DonationsDesiredByPosition
DROP PROCEDURE USP_SEL_DonationsGivenAll
DROP PROCEDURE USP_SEL_DonationsGivenById
DROP PROCEDURE USP_SEL_DonationsGivenByPosition
DROP PROCEDURE USP_SEL_DonationsSystemByLanguageAndName
DROP PROCEDURE USP_SEL_TradesByUserGet
DROP PROCEDURE USP_SEL_TradesByUserLet
DROP PROCEDURE USP_UPD_DonationsDesired
DROP PROCEDURE USP_UPD_DonationsGiven
DROP PROCEDURE USP_UPD_TradeEvaluations
DROP PROCEDURE USP_UPD_TradeLocations
DROP PROCEDURE USP_UPD_Trades
*/

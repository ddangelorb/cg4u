CREATE DATABASE CG4UDonate
--When configuring server read and follow that: 
--	https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/managing-permissions-with-stored-procedures-in-sql-server

CREATE LOGIN cg4uWebUser WITH PASSWORD = 'b5e2847d-f284-47c0-9e80-e84e052b2e28'
CREATE USER cg4uWebUser FOR LOGIN cg4uWebUser

GRANT SELECT ON "dbo"."TradeLocations" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."TradeEvaluations" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."UserSystemPreferences" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."Trades" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."UsersSystems" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."DonationsGivens" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."Users" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."ObjectsLanguages" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."Objects" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."Languages" TO "cg4uWebUser"
GRANT SELECT ON "dbo"."Systems" TO "cg4uWebUser"

GRANT EXECUTE ON dbo.USP_INS_DonationsGivens TO cg4uWebUser
GRANT EXECUTE ON dbo.USP_SEL_DonationsGivensByDistance TO cg4uWebUser

USE CG4UDonate

CREATE TABLE Donations (
	Id int not null ,
	IdSystems int not null,
	IdDonationsDad int null,
	Img varBinary(Max) null, --IMAGE null,
	Active bit not null,
    PRIMARY KEY (Id),
    --FOREIGN KEY (IdSystems) REFERENCES Systems(Id),
    FOREIGN KEY (IdDonationsDad) REFERENCES Donations(Id)
);

CREATE TABLE DonationsNames (
	Id int not null ,
	IdDonations int not null,
	IdLanguages int not null,
	Name nvarchar(50) not null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdDonations) REFERENCES Donations(Id) --,
    --FOREIGN KEY (IdLanguages) REFERENCES Languages(Id)
);

/*
 * TODO
ALTER TABLE DonationsGivens
ADD Qtty int not null
 */
CREATE TABLE DonationsGivens (
	Id int not null ,
	IdDonations int not null,
	IdUserOwner int not null,
	DtUpdate datetime not null,
	Qtty int not null,
	Img varBinary(Max) null,
	DtExp datetime null,
	Address nvarchar(255) not null,
	City nvarchar(50) not null,
	State nvarchar(50) not null,
	ZipCode nvarchar(20) not null,
	Latitude float not null,
	Longitude float not null,
	GeoLocation geography not null,
	MaxLetinMeters float null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdDonations) REFERENCES Donations(Id) --,
    --FOREIGN KEY (IdUserOwner) REFERENCES Users(Id)
);

/*
 * TODO
ALTER TABLE DonationsDesired
ADD (Qtty int not null, Img varBinary(Max) null, ImgBack varBinary(Max) null)
 */
CREATE TABLE DonationsDesired (
	Id int not null ,
	IdDonations int not null,
	IdUserOwner int not null,
	DtUpdate datetime not null,
	Qtty int not null, 
	Img varBinary(Max) null, 
	ImgBack varBinary(Max) null
	DtExp datetime null,
	Address nvarchar(255) not null,
	City nvarchar(50) not null,
	State nvarchar(50) not null,
	ZipCode nvarchar(20) not null,
	Latitude float not null,
	Longitude float not null,
	GeoLocation geography not null,
	MaxGetinMeters float null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdDonations) REFERENCES Donations(Id) --,
    --FOREIGN KEY (IdUserOwner) REFERENCES Users(Id)
);

/*
 * TODO
ALTER TABLE DonationsDesired
ADD (QttyConfirmed int not null, ImgReceipt varBinary(Max) null, UserReceiver nvarchar(256) null, UserReceiverPs nvarchar(50) null)
 */
CREATE TABLE Trades (
	Id int not null ,
	IdUserGet int not null,
	IdUserLet int not null,
	IdDonationsGivens int not null,
	IdDonationsDesired int not null,
	DtTrade datetime not null,
	Commited bit not null,
	Active bit not null,
    PRIMARY KEY (Id),
    --FOREIGN KEY (IdUserGet) REFERENCES Users(Id),
    --FOREIGN KEY (IdUserLet) REFERENCES Users(Id),
    FOREIGN KEY (IdDonationsGivens) REFERENCES DonationsGivens(Id),
    FOREIGN KEY (IdDonationsDesired) REFERENCES DonationsDesired(Id)
);

CREATE TABLE TradeLocations (
	Id int not null ,
	IdTrades int not null,
	Address nvarchar(255) not null,
	City nvarchar(50) not null,
	State nvarchar(50) not null,
	ZipCode nvarchar(20) not null,
	Latitude float not null,
	Longitude float not null,
	GeoLocation geography not null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdTrades) REFERENCES Trades(Id)
);

CREATE TABLE TradeEvaluations (
	Id int not null ,
	IdTrades int not null,
	UserGetGrade TINYINT null,
	UserLetGrade TINYINT null,
	CommentsUserGet nvarchar(25) null,
	CommentsUserLet nvarchar(25) null,
	DtEvaluationGet datetime null,
	DtEvaluationLet datetime null,
	ActiveGet bit null,
	ActiveLet bit null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdTrades) REFERENCES Trades(Id)
);
/*
USE CG4UDonate
sp_help Donations

DROP TABLE TradeEvaluations
DROP TABLE TradeLocations
DROP TABLE Trades
DROP TABLE DonationsDesired
DROP TABLE DonationsGivens
DROP TABLE DonationsNames
DROP TABLE Donations

USE CG4UDonate
SELECT * FROM Donations
SELECT * FROM DonationsNames
SELECT * FROM DonationsDesired
SELECT * FROM DonationsGivens
SELECT * FROM Trades
SELECT * FROM TradeEvaluations
SELECT * FROM TradeLocations

delete from Trades

EXEC USP_SEL_TradesById 1
EXEC USP_SEL_TradesByIdSystemLanguage 1, 1, 1

sp_help

Id |IdDonations |IdUserOwner |DtUpdate            |DtExp |Address                          |City      |State |ZipCode  |Latitude    |Longitude   |GeoLocation            |MaxGetinMeters |Active
---|------------|------------|--------------------|------|---------------------------------|----------|------|---------|------------|------------|-----------------------|---------------|------
1  |1           |2           |2018-04-19 14:35:47 |      |Rua Borba Gato 331 Ap 32 Torre J |São Paulo |SP    |04747030 |-23.6565581 |-46.7001006 |     úl 1 ×7¿zt~ãYG¿ |5000           |1      |
2  |2           |2           |2018-04-19 14:35:48 |      |Rua Borba Gato 331 Ap 32 Torre J |São Paulo |SP    |04747030 |-23.6565581 |-46.7001006 |     úl 1 ×7¿zt~ãYG¿ |12000          |1      |
3  |3           |2           |2018-04-19 14:35:49 |      |Rua Borba Gato 331 Ap 32 Torre J |São Paulo |SP    |04747030 |-23.6565581 |-46.7001006 |     úl 1 ×7¿zt~ãYG¿ |11000          |1      |

SELECT * FROM TradeEvaluations
SELECT * FROM TradeLocations
SELECT * FROM Trades




*/


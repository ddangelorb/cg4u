/*
 * https://docs.microsoft.com/pt-br/sql/linux/quickstart-install-connect-docker?view=sql-server-linux-2017
$ docker exec -it sqlserver "bash"
root@9bd14085a87a:/# /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P '...'
1> CREATE DATABASE CG4USecurity
2> GO
1> exit
*/ 
CREATE DATABASE CG4USecurity
--When configuring server read and follow that: 
--	https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/managing-permissions-with-stored-procedures-in-sql-server

CREATE LOGIN cg4uWebUser WITH PASSWORD = 'b5e2847d-f284-47c0-9e80-e84e052b2e28'
CREATE USER cg4uWebUser FOR LOGIN cg4uWebUser

GRANT SELECT ON "dbo"."TradeLocations" TO "cg4uWebUser"

GRANT EXECUTE ON dbo.USP_INS_DonationsGivens TO cg4uWebUser
GRANT EXECUTE ON dbo.USP_SEL_DonationsGivensByDistance TO cg4uWebUser

USE CG4USecurity

CREATE TABLE AnalyzesRequests (
	Id int not null,
	IdBillableProcesses int not null,
	IdLanguages int NOT NULL,
	AnalyzeOrder tinyint NOT NULL,
	TypeCode tinyint NOT NULL,
	TypeName nvarchar(50) not null,  -- 1 'Scene Change', 2 'Face',  3 'Carplate', 4 'Image Description' [Products], 5 PersonGroupCreate, 6 PersonGroupTrain, 7 PersonGroupPersonCreate, 8 PersonGroupPersonAddFace 
	Location VARCHAR(255) NULL,
	SubscriptionKey VARCHAR(255) NULL,
	Active bit not null,
    PRIMARY KEY (Id),
    CONSTRAINT TypeCode_AnalyzesRequests UNIQUE (TypeCode)
);

CREATE TABLE Alerts (
	Id int NOT NULL,
	IdAnalyzesRequests int NOT NULL,
	TypeCode tinyint NOT NULL, --1 Panic, 2 Critical, 3 Warning
	Message nvarchar(255) NOT NULL,
	ProcessingMethod nvarchar(50) not null, --1 SceneChange, 2 UnkownPeople, 3 UnkownCar 
	ProcessingParam nvarchar(255) NULL, --Regex to carplate ('[A-Z]{3}[0-9]{4}'), Face Threshold 92, etc
	Active bit not null,
	PRIMARY KEY (Id),
	FOREIGN KEY (IdAnalyzesRequests) REFERENCES AnalyzesRequests(Id)
)

CREATE TABLE PersonGroups (
	Id int NOT NULL,
	IdCustomers int not null,
	IdApi uniqueidentifier NOT NULL,
	Name nvarchar(100) not null,
	Active bit not null,
	PRIMARY KEY (Id),
	CONSTRAINT IdApi_PersonGroups UNIQUE (IdApi)
);

CREATE TABLE PersonGroupsAlerts (
	Id int NOT NULL,
	IdPersonGroups int not null,
	IdAlerts int NOT NULL,
	Active bit not null,
	PRIMARY KEY (Id),
	FOREIGN KEY (IdPersonGroups) REFERENCES PersonGroups(Id),
	FOREIGN KEY (IdAlerts) REFERENCES Alerts(Id)
);

CREATE TABLE Persons (
	Id int NOT NULL,
	IdPersonGroups int NOT NULL,
	IdApi uniqueidentifier NOT NULL,
	IdUsers int null,
	Name nvarchar(255) not null,
	Active bit not null,
	PRIMARY KEY (Id),
	FOREIGN KEY (IdPersonGroups) REFERENCES PersonGroups(Id),
	CONSTRAINT IdApi_Persons UNIQUE (IdApi)
);

CREATE UNIQUE NONCLUSTERED INDEX IDX_IdUsers_Persons_NotNull
ON Persons(IdUsers)
WHERE IdUsers IS NOT NULL;

CREATE TABLE PersonFaces (
	Id int NOT NULL,
	IdPersons int NOT NULL,
	FaceId uniqueidentifier NOT NULL,
	Active bit not null,
	PRIMARY KEY (Id),
	FOREIGN KEY (IdPersons) REFERENCES Persons(Id),
	CONSTRAINT FaceId_PersonFaces UNIQUE (FaceId)
);


CREATE TABLE PersonCars (
	Id int NOT NULL,
	IdPersons int NOT NULL,
	PlateCode nvarchar(15) not null,
	Active bit not null,
	PRIMARY KEY (Id),
	FOREIGN KEY (IdPersons) REFERENCES Persons(Id)
);

CREATE UNIQUE NONCLUSTERED INDEX IDX_PlateCode_PersonCars
ON PersonCars(PlateCode);

CREATE TABLE VideoCameras (
	Id int not null,
	IdPersonGroups int not null,
	IdPersonGroupsAPI uniqueidentifier NOT NULL,
	Name nvarchar(100) not null,
	Description nvarchar(100) NOT null,
	IngestStreamingUri nvarchar(255) NOT NULL,
	ImageFile varBinary(Max) null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdPersonGroups) REFERENCES PersonGroups(Id)
);

CREATE TABLE TriggersVideoCameras (
	Id int not null,
	IdVideoCameras int not null,
	TypeCode tinyint NOT NULL,
	TypeName nvarchar(50) not null,  -- 1 'Thumbnail',  2 'SceneChange' 
	CommandLine nvarchar(255) NOT NULL,
	ImagePath nvarchar(255) NOT NULL,
	ImageProcessedFolder nvarchar(255) NOT NULL,
	ThresholdParameter tinyint NOT NULL,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdVideoCameras) REFERENCES VideoCameras(Id)
)

CREATE TABLE AnalyzesRequestsVideoCameras (
	Id int not null,
	IdAnalyzesRequests int NOT NULL,
	IdVideoCameras int not null,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdAnalyzesRequests) REFERENCES AnalyzesRequests(Id),
    FOREIGN KEY (IdVideoCameras) REFERENCES VideoCameras(Id)
);

CREATE TABLE ImageProcesses (
	Id int not null,
	IdReference uniqueidentifier NOT NULL,
	IdVideoCameras int NOT NULL,
	ImageFile varBinary(Max) null, 
	ImageName nvarchar(100) NOT NULL,
	IpUserRequest varchar(255) NOT NULL,
	VideoPath nvarchar(255) NOT NULL,
	SecondsToStart int NOT NULL,
	DtProcess datetime NOT NULL,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdVideoCameras) REFERENCES VideoCameras(Id),
    CONSTRAINT IdReference_ImageProcesses UNIQUE (IdReference)
);

CREATE TABLE ImageProcessAnalyzes (
	Id int not null,
	IdReference uniqueidentifier NOT NULL,
	IdImageProcesses int NOT NULL,
	IdAnalyzesRequestsVideoCameras int NOT NULL,
	DtAnalyze datetime NOT NULL,
	ReturnResponseType varchar(5) NOT NULL, --'JSON'
	ReturnResponse nvarchar(max) NULL, --Fulltext search
	Commited bit not null,
	Active bit not null,
    PRIMARY KEY (Id),
    CONSTRAINT IdReference_ImageProcessAnalyzes UNIQUE (IdReference),
    FOREIGN KEY (IdImageProcesses) REFERENCES ImageProcesses(Id),
    FOREIGN KEY (IdAnalyzesRequestsVideoCameras) REFERENCES AnalyzesRequestsVideoCameras(Id)
);

CREATE FULLTEXT CATALOG FullTextCatalog AS DEFAULT;

--SELECT * FROM VideoCameras
--SELECT SERVERPROPERTY('IsFullTextInstalled')

/*
 * SQL Error [7609] [S0005]: Full-Text Search is not installed, or a full-text component cannot be loaded.
TODO: Create this on server side, re-create the sp USP_SEL_ImageProcessesByResponse accordigngly (removing comments)
https://docs.microsoft.com/pt-br/sql/linux/sql-server-linux-configure-docker?view=sql-server-linux-2017
https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup-full-text-search?view=sql-server-linux-2017

CREATE FULLTEXT INDEX ON ImageProcessAnalisys 
(
    ReturnResponse TYPE COLUMN ReturnResponseType LANGUAGE 0 --1046
) 
KEY INDEX PK_ImageProcessAnalisys 
WITH 
    CHANGE_TRACKING = AUTO, 
    STOPLIST=OFF;
--SELECT * FROM ImageProcessAnalisys WHERE FREETEXT((ReturnResponse), 'glasses')  --https://www.sqlshack.com/hands-full-text-search-sql-server/
*/

-- *** WebHook or service to monitore TABLE ImageProcessAnalisys, insert into this table and send users alerts (when applicable)
--https://stackoverflow.com/questions/5288434/how-to-monitor-sql-server-table-changes-by-using-c
--signal r core
--	https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-2.1&tabs=visual-studio-code
--https://www.dreamincode.net/forums/topic/156991-using-sqldependency-to-monitor-sql-database-changes/  
/*  
 * --Don't need that at the moment. We are using signalR on WebApi to notify clients.
CREATE TABLE ImageProcessingLogs (
	Id int not null,
	IdImageProcessAnalyzes int NOT NULL,
	IdEntityRelated int null,
	EntityRelatedType char(1) NOT NULL,
	DtLog datetime NOT NULL,
	Active bit not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdImageProcessAnalyzes) REFERENCES ImageProcessAnalyzes(Id),
    CONSTRAINT Chk_EntityRelatedType CHECK (EntityRelatedType IN ('P', 'C', 'O')) --People, Car, Other
);
  */

/*
USE CG4USecurity
sp_help Alerts

DROP TABLE ImageProcessAnalyzes 
DROP TABLE ImageProcesses 
DROP TABLE AnalyzesRequestsVideoCameras 
DROP TABLE TriggersVideoCameras
DROP TABLE VideoCameras 
DROP TABLE PersonCars 
DROP TABLE PersonFaces
DROP TABLE Persons 
DROP TABLE PersonGroupsAlerts 
DROP TABLE PersonGroups 
DROP TABLE Alerts 
DROP TABLE AnalyzesRequests 

USE CG4USecurity

DELETE FROM ImageProcessAnalyzes 
DELETE FROM ImageProcesses 
DELETE FROM PersonCars 
DELETE FROM PersonFaces
DELETE FROM Persons 
DELETE FROM AnalyzesRequestsVideoCameras 
DELETE FROM VideoCameras 
DELETE FROM PersonGroupsAlerts
DELETE FROM PersonGroups 
DELETE FROM Alerts 
DELETE FROM AnalyzesRequests 


EXEC USP_SEL_PersonGroupsById 1
SELECT * FROM AnalyzesRequests WHERE Id=1 AND Active=1

EXEC USP_SEL_VideoCamerasById 1
SELECT * FROM VideoCameras WHERE Id=1

USE CG4USecurity

SELECT * FROM AnalyzesRequests 
SELECT * FROM Alerts 
SELECT * FROM PersonGroups 
SELECT * FROM PersonGroupsAlerts
SELECT * FROM VideoCameras 
SELECT * FROM TriggersVideoCameras
SELECT * FROM AnalyzesRequestsVideoCameras 
SELECT * FROM Persons 
SELECT * FROM PersonFaces 
SELECT * FROM PersonCars 
SELECT * FROM ImageProcesses 
SELECT * FROM ImageProcessAnalyzes 
*/
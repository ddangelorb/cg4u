CREATE OR ALTER PROCEDURE USP_INS_PersonGroups
@IdCustomers int,
@IdApi 		uniqueidentifier,
@Name 		nvarchar(100)
AS
	INSERT INTO PersonGroups (Id, IdCustomers, IdApi, Name, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdCustomers, 
		@IdApi, 
		@Name,
		1
	FROM PersonGroups
GO
CREATE OR ALTER PROCEDURE USP_INS_Persons
@IdPersonGroups int,
@IdApi 			uniqueidentifier,
@IdUsers 		int,
@Name 			nvarchar(255)
AS
	INSERT INTO Persons (Id, IdPersonGroups, IdApi, IdUsers, Name, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdPersonGroups, 
		@IdApi, 
		@IdUsers, 
		@Name,
		1
	FROM Persons
GO
CREATE OR ALTER PROCEDURE USP_INS_UsersSystems
@IdUserIdentity nvarchar(450),
@IdSystems 	int
AS
	DECLARE @IdUsers int
	SELECT @IdUsers = Id FROM Users WHERE IdUserIdentity = @IdUserIdentity
	
	INSERT INTO UsersSystems (Id, IdUsers, IdSystems, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdUsers,
		@IdSystems,
		0
	FROM UsersSystems
GO
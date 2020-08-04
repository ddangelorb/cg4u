CREATE OR ALTER PROCEDURE USP_UPD_Persons
@Id				int,
@IdPersonGroups int,
@IdApi 			uniqueidentifier,
@IdUsers 		int,
@Name 			nvarchar(255),
@Active 		bit
AS 
	UPDATE Persons
	SET
		IdPersonGroups = @IdPersonGroups, 
		IdApi = @IdApi, 
		IdUsers = @IdUsers, 
		Name = @Name,
		Active = @Active
	WHERE
		Id = @Id

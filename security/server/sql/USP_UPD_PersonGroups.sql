CREATE OR ALTER PROCEDURE USP_UPD_PersonGroups
@Id				int,
@IdCustomers 	int,
@IdApi 			uniqueidentifier,
@Name 			nvarchar(100),
@Active 		bit
AS 
	UPDATE PersonGroups
	SET
		IdCustomers = @IdCustomers, 
		IdApi = @IdApi, 
		Name = @Name,
		Active = @Active
	WHERE
		Id = @Id

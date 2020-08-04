CREATE OR ALTER PROCEDURE USP_INS_PersonFaces
@IdPersons 	int,
@FaceId 	uniqueidentifier
AS
	INSERT INTO PersonFaces (Id, IdPersons, FaceId, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdPersons, 
		@FaceId,
		1
	FROM PersonFaces
GO
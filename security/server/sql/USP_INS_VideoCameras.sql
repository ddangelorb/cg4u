CREATE OR ALTER PROCEDURE USP_INS_VideoCameras
@IdPersonGroups 	int,
@IdPersonGroupsAPI 	uniqueidentifier,
@Name 				nvarchar(50),
@Description 		nvarchar(100)
AS
	INSERT INTO VideoCameras (Id, IdPersonGroups, IdPersonGroupsAPI, Name, Description, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdPersonGroups,
		@IdPersonGroupsAPI,
		@Name,
		@Description,
		1
	FROM VideoCameras
GO
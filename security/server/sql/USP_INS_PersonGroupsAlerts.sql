CREATE OR ALTER PROCEDURE USP_INS_PersonGroupsAlerts
@IdPersonGroups int,
@IdAlerts 		int
AS
	INSERT INTO PersonGroupsAlerts (Id, IdPersonGroups, IdAlerts, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdPersonGroups,
		@IdAlerts,
		1
	FROM PersonGroupsAlerts
GO
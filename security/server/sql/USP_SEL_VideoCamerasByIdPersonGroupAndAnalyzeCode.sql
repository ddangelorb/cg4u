CREATE OR ALTER PROCEDURE USP_SEL_VideoCamerasByIdPersonGroupAndAnalyzeCode 
@IdPersonGroups	int,
@TypeCode 		tinyint
AS 
	SELECT  
		vc.Id,
		vc.IdPersonGroups,
		CONVERT(varchar(36), vc.IdPersonGroupsAPI) AS IdPersonGroupsAPI,
		vc.Name,
		vc.Description,
		vc.Active,
		ar.Id,
		ar.IdBillableProcesses,
		ar.IdLanguages,
		ar.AnalyzeOrder,
		ar.TypeCode,
		ar.TypeName, 
		ar.Location,
		ar.SubscriptionKey,
		ar.Active,
		arvc.Id AS IdAnalyzesRequestsVideoCameras
	FROM   
		AnalyzesRequestsVideoCameras arvc
		INNER JOIN VideoCameras vc ON arvc.IdVideoCameras = vc.Id    
		INNER JOIN AnalyzesRequests ar ON arvc.IdAnalyzesRequests = ar.Id
	WHERE   
		vc.IdPersonGroups = @IdPersonGroups     
		AND ar.TypeCode = @TypeCode
		AND arvc.Active = 1 AND vc.Active = 1 AND ar.Active = 1


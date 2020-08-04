CREATE OR ALTER PROCEDURE USP_SEL_VideoCamerasById 
@Id	int
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
		VideoCameras vc
		LEFT JOIN AnalyzesRequestsVideoCameras arvc on vc.Id = arvc.IdVideoCameras AND arvc.Active = 1
		LEFT JOIN AnalyzesRequests ar ON ar.Id = arvc.IdAnalyzesRequests AND ar.Active = 1
	WHERE   
		vc.Id = @Id AND vc.Active = 1 


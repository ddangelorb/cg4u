CREATE OR ALTER PROCEDURE USP_INS_AnalyzesRequestsVideoCameras
@IdAnalyzesRequests int,
@IdVideoCameras 	int
AS
	INSERT INTO AnalyzesRequestsVideoCameras (Id, IdAnalyzesRequests, IdVideoCameras, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdAnalyzesRequests, 
		@IdVideoCameras, 
		1
	FROM AnalyzesRequestsVideoCameras
GO
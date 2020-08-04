CREATE OR ALTER PROCEDURE USP_INS_ImageProcessAnalyzes
@IdReference 					uniqueidentifier,
@IdImageProcesses 				int,
@IdAnalyzesRequestsVideoCameras 	int,
@DtAnalyze 						datetime,
@ReturnResponseType 				varchar(5),
@ReturnResponse 					nvarchar(max),
@Commited 						bit
AS
	INSERT INTO ImageProcessAnalyzes (Id, IdReference, IdImageProcesses, IdAnalyzesRequestsVideoCameras, DtAnalyze, ReturnResponseType, ReturnResponse, Commited, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdReference,
		@IdImageProcesses,
		@IdAnalyzesRequestsVideoCameras,
		@DtAnalyze,
		@ReturnResponseType,
		@ReturnResponse,
		@Commited,
		1
	FROM ImageProcessAnalyzes
GO
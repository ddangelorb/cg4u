CREATE OR ALTER PROCEDURE USP_UPD_ImageProcessAnalyzes
@Id								int,
@IdReference 					uniqueidentifier,
@IdImageProcesses 				int,
@IdAnalyzesRequestsVideoCameras 	int,
@DtAnalyze 						datetime,
@ReturnResponseType 				varchar(5),
@ReturnResponse 					nvarchar(max),
@Commited 						bit,
@Active 							bit
AS 
	UPDATE ImageProcessAnalyzes
	SET
		IdReference= @IdReference,
		IdImageProcesses = @IdImageProcesses,
		IdAnalyzesRequestsVideoCameras = @IdAnalyzesRequestsVideoCameras,
		DtAnalyze = @DtAnalyze,
		ReturnResponseType = @ReturnResponseType,
		ReturnResponse = @ReturnResponse,
		Commited = @Commited,
		Active = @Active
	WHERE
		Id = @Id

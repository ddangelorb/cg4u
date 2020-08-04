CREATE OR ALTER PROCEDURE USP_INS_ImageProcesses
@IdReference 	uniqueidentifier,
@IdVideoCameras int,
@ImageFile 		varBinary(Max), 
@ImageName 		nvarchar(100),
@IpUserRequest 	varchar(255),
@VideoPath 		nvarchar(255),
@SecondsToStart int,
@DtProcess 		datetime
AS
	INSERT INTO ImageProcesses (Id, IdReference, IdVideoCameras, ImageFile, ImageName, IpUserRequest, VideoPath, SecondsToStart, DtProcess, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdReference,
		@IdVideoCameras,
		@ImageFile, 
		@ImageName,
		@IpUserRequest,
		@VideoPath,
		@SecondsToStart,
		@DtProcess,
		1
	FROM ImageProcesses
GO
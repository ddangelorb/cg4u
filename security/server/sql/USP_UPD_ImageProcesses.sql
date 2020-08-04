CREATE OR ALTER PROCEDURE USP_UPD_ImageProcesses
@Id				int,
@IdReference 	uniqueidentifier,
@IdVideoCameras int,
@ImageFile 		varBinary(Max), 
@ImageName 		nvarchar(100),
@IpUserRequest 	varchar(255),
@VideoPath 		nvarchar(255),
@SecondsToStart int,
@DtProcess 		datetime,
@Active 			bit
AS 
	UPDATE ImageProcesses
	SET
		IdReference= @IdReference,
		IdVideoCameras = @IdVideoCameras,
		ImageFile = @ImageFile, 
		ImageName = @ImageName,
		IpUserRequest = @IpUserRequest,
		VideoPath = @VideoPath,
		SecondsToStart = @SecondsToStart,
		DtProcess = @DtProcess,
		Active = @Active
	WHERE
		Id = @Id

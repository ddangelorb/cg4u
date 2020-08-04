CREATE OR ALTER PROCEDURE USP_INS_Alerts
@IdAnalyzesRequests int,
@TypeCode 			tinyint,
@Message 			nvarchar(255),
@ProcessingMethod 	nvarchar(50),
@ProcessingParam 	nvarchar(255) NULL
AS
	INSERT INTO Alerts (Id, IdAnalyzesRequests, TypeCode, Message, ProcessingMethod, ProcessingParam, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdAnalyzesRequests ,
		@TypeCode,
		@Message,
		@ProcessingMethod,
		@ProcessingParam,
		1
	FROM Alerts
GO
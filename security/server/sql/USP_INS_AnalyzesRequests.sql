CREATE OR ALTER PROCEDURE USP_INS_AnalyzesRequests
@IdBillableProcesses 	int,
@IdLanguages 			int,
@AnalyzeOrder 			tinyint,
@TypeCode 				tinyint,
@TypeName 				nvarchar(50),
@Location 				VARCHAR(255),
@SubscriptionKey 		VARCHAR(255)
AS
	INSERT INTO AnalyzesRequests (Id, IdBillableProcesses, IdLanguages, AnalyzeOrder, TypeCode, TypeName, Location, SubscriptionKey, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdBillableProcesses,
		@IdLanguages,
		@AnalyzeOrder,
		@TypeCode,
		@TypeName,
		@Location,
		@SubscriptionKey,
		1
	FROM AnalyzesRequests
GO
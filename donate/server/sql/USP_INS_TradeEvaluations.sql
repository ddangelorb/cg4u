CREATE OR ALTER PROCEDURE USP_INS_TradeEvaluations
@IdTrades 			int,
@UserGetGrade 		TINYINT,
@UserLetGrade 		TINYINT,
@CommentsUserGet 	varchar(25),
@CommentsUserLet 	varchar(25),
@DtEvaluationGet 	datetime,
@DtEvaluationLet 	datetime,
@ActiveGet 			bit,
@ActiveLet 			bit
AS
	INSERT INTO TradeEvaluations (Id, IdTrades, UserGetGrade, UserLetGrade, CommentsUserGet, CommentsUserLet, DtEvaluationGet, DtEvaluationLet, ActiveGet, ActiveLet)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdTrades,
		@UserGetGrade,
		@UserLetGrade,
		@CommentsUserGet,
		@CommentsUserLet,
		@DtEvaluationGet,
		@DtEvaluationLet,
		@ActiveGet,
		@ActiveLet
	FROM TradeEvaluations
GO
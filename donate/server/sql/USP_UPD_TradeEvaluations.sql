CREATE OR ALTER PROCEDURE USP_UPD_TradeEvaluations
@Id					int,
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
	UPDATE TradeEvaluations
	SET
		IdTrades = @IdTrades, 
		UserGetGrade = @UserGetGrade, 
		UserLetGrade = @UserLetGrade, 
		CommentsUserGet = @CommentsUserGet, 
		CommentsUserLet = @CommentsUserLet, 
		DtEvaluationGet = @DtEvaluationGet, 
		DtEvaluationLet = @DtEvaluationLet, 
		ActiveGet = @ActiveGet, 
		ActiveLet = @ActiveLet
	WHERE
		Id = @Id
GO
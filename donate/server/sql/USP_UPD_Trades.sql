CREATE OR ALTER PROCEDURE USP_UPD_Trades
@Id				int,
@IdUserGet 			int,
@IdUserLet 			int,
@IdDonationsGivens 	int,
@IdDonationsDesired int,
@DtTrade 			datetime,
@Commited 			bit,
@Active 				bit
AS 
	UPDATE Trades
	SET
		IdUserGet = @IdUserGet, 
		IdUserLet = @IdUserLet, 
		IdDonationsGivens = @IdDonationsGivens, 
		IdDonationsDesired = @IdDonationsDesired, 
		DtTrade = @DtTrade, 
		Commited = @Commited, 
		Active = @Active
	WHERE
		Id = @Id
GO
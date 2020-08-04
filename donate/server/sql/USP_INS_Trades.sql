CREATE OR ALTER PROCEDURE USP_INS_Trades
@IdUserGet 			int,
@IdUserLet 			int,
@IdDonationsGivens 	int,
@IdDonationsDesired int,
@DtTrade 			datetime,
@Commited 			bit
AS
	INSERT INTO Trades (Id, IdUserGet, IdUserLet, IdDonationsGivens, IdDonationsDesired, DtTrade, Commited, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdUserGet,
		@IdUserLet,
		@IdDonationsGivens,
		@IdDonationsDesired,
		@DtTrade,
		@Commited,
		1
	FROM Trades
GO
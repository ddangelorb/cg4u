CREATE OR ALTER PROCEDURE USP_UPD_DonationsGivenImg
@Id 		int,
@Img		varbinary(max)
AS 
	UPDATE DonationsGivens
	SET
		DtUpdate = GETDATE(),
		Img = @Img
	FROM DonationsGivens
	WHERE Id = @Id

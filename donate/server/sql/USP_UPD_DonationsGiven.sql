CREATE OR ALTER PROCEDURE USP_UPD_DonationsGiven
@Id				int,
@IdDonations 	int,
@IdUserOwner 	int,
@DtExp 			datetime,
@Img 			varbinary(max),
@Address 		nvarchar(255),
@City			nvarchar(50),
@State			nvarchar(50),
@ZipCode			nvarchar(20),
@Latitude		float,
@Longitude		float, 
@MaxLetinMeters float,
@Active 			bit
AS 
	UPDATE DonationsGivens
	SET
		IdDonations = @IdDonations, 
		IdUserOwner = @IdUserOwner,	
		DtUpdate = GETDATE(), 
		DtExp = @DtExp, 
		Img = @Img,
		Address = @Address, 
		City = @City, 
		State = @State, 
		ZipCode = @ZipCode, 
		Latitude = @Latitude, 
		Longitude = @Longitude, 
		GeoLocation = GEOGRAPHY::Point(@Latitude , @Longitude , 4326), 
		MaxLetinMeters = @MaxLetinMeters, 
		Active = @Active
	WHERE
		Id = @Id

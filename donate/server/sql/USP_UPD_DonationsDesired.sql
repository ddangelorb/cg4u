CREATE OR ALTER PROCEDURE USP_UPD_DonationsDesired
@Id				int,
@IdDonations 	int,
@IdUserOwner 	int,
@DtExp 			datetime,
@Address 		nvarchar(255),
@City			nvarchar(50),
@State			nvarchar(50),
@ZipCode			nvarchar(20),
@Latitude		float,
@Longitude		float, 
@MaxGetinMeters float,
@Active 			bit
AS 
	UPDATE DonationsDesired
	SET
		IdDonations = @IdDonations, 
		IdUserOwner = @IdUserOwner,	
		DtUpdate = GETDATE(), 
		DtExp = @DtExp, 
		Address = @Address, 
		City = @City, 
		State = @State, 
		ZipCode = @ZipCode, 
		Latitude = @Latitude, 
		Longitude = @Longitude, 
		GeoLocation = GEOGRAPHY::Point(@Latitude , @Longitude , 4326), 
		MaxGetinMeters = @MaxGetinMeters, 
		Active = @Active
	WHERE
		Id = @Id

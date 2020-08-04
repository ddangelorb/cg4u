CREATE OR ALTER PROCEDURE USP_INS_DonationsDesired
@IdDonations 	int,
@IdUserOwner 	int,
@DtExp 			datetime,
@Address 		nvarchar(255),
@City			nvarchar(50),
@State			nvarchar(50),
@ZipCode			nvarchar(20),
@Latitude		float,
@Longitude		float, 
@MaxGetinMeters float
AS 
	INSERT INTO DonationsDesired (Id, IdDonations, IdUserOwner,	DtUpdate, DtExp, Address, City, State, ZipCode, Latitude, Longitude, GeoLocation, MaxGetinMeters, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1, 
		@IdDonations,
		@IdUserOwner,
		GETDATE(), 
		@DtExp,
		@Address, 
		@City, 
		@State, 
		@ZipCode, 
		@Latitude, 
		@Longitude,
		GEOGRAPHY::Point(@Latitude , @Longitude , 4326),
		@MaxGetinMeters,
		1
	FROM DonationsDesired
GO
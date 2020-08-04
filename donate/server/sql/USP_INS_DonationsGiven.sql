CREATE OR ALTER PROCEDURE USP_INS_DonationsGiven
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
@MaxLetinMeters float
AS 
	INSERT INTO DonationsGivens(Id, IdDonations, IdUserOwner, DtUpdate, DtExp, Img, Address, City, State, ZipCode, Latitude, Longitude, GeoLocation, MaxLetinMeters, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1, 
		@IdDonations,
		@IdUserOwner,
		GETDATE(), 
		@DtExp,
		@Img,
		@Address, 
		@City, 
		@State, 
		@ZipCode, 
		@Latitude, 
		@Longitude,
		GEOGRAPHY::Point(@Latitude , @Longitude , 4326),
		@MaxLetinMeters,
		1
	FROM DonationsGivens

CREATE OR ALTER PROCEDURE USP_UPD_TradeLocations
@Id			int,
@IdTrades 	int,
@Address 	varchar(255),
@City 		varchar(50),
@State 		varchar(50),
@ZipCode 	varchar(20),
@Latitude 	float,
@Longitude 	float,
@Active 		bit
AS 
	UPDATE TradeLocations
	SET
		IdTrades = @IdTrades, 
		Address = @Address, 
		City = @City, 
		State = @State, 
		ZipCode = @ZipCode, 
		Latitude = @Latitude, 
		Longitude = @Longitude, 
		GeoLocation = GEOGRAPHY::Point(@Latitude , @Longitude , 4326), 
		Active = @Active
	WHERE
		Id = @Id
GO
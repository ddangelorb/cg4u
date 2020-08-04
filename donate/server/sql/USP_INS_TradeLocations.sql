CREATE OR ALTER PROCEDURE USP_INS_TradeLocations
@IdTrades 	int,
@Address 	varchar(255),
@City 		varchar(50),
@State 		varchar(50),
@ZipCode 	varchar(20),
@Latitude 	float,
@Longitude 	float
AS
	INSERT INTO TradeLocations (Id, IdTrades, Address, City, State, ZipCode, Latitude, Longitude, GeoLocation, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdTrades,
		@Address, 
		@City, 
		@State, 
		@ZipCode, 
		@Latitude, 
		@Longitude,
		GEOGRAPHY::Point(@Latitude , @Longitude , 4326),
		1
	FROM TradeLocations
GO
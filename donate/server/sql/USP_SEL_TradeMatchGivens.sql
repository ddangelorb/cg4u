CREATE OR ALTER PROCEDURE USP_SEL_TradeMatchGivens 
@IdDonationsDesired     int,
@IdSystems 		        int,
@IdLanguages		    		int,
@MaxDistanceInMeters    float
AS 
	DECLARE @IdDonations		int
    DECLARE @FromLatitude	float
    DECLARE @FromLongitude	float
	DECLARE @FromGeoLoc geography

    SELECT @IdDonations = IdDonations, @FromLatitude = Latitude, @FromLongitude = Longitude FROM DonationsDesired WHERE Id = @IdDonationsDesired AND Active = 1
	SELECT @FromGeoLoc = GEOGRAPHY::Point(@FromLatitude , @FromLongitude , 4326)

	SELECT   
		dg.Id,
		dg.Id AS IdDonationsGivens,
		dg.DtUpdate,   
		dg.DtExp,
		dg.Img,
		dg.MaxLetinMeters,		
		dg.Active,
		d.Id,
		d.Id AS IdDonations,
		d.IdSystems,   
		d.IdDonationsDad,   
		d.Img,
		d.Active,
		dn.Id,
		dn.Id as IdDonationsNames,
		dn.IdDonations,
		dn.IdLanguages,   
		dn.Name,
		dn.Active,
		u.Id,
		u.IdUserIdentity,
		u.Email,
		u.FirstName,
		u.SurName,
		u.Avatar,
		u.Active,
		dg.Id,
		dg.Id as IdParent,
		dg.Address,   
		dg.City,
		dg.State,
		dg.ZipCode,
		dg.Latitude,
		dg.Longitude
	FROM   
		DonationsGivens dg   
		INNER JOIN Donations d ON dg.IdDonations = d.Id   
		INNER JOIN DonationsNames dn ON dg.IdDonations = dn.IdDonations   
		INNER JOIN CG4UCore..Users u ON dg.IdUserOwner = u.Id   
	WHERE
        dg.GeoLocation.STDistance(@FromGeoLoc) <= @MaxDistanceInMeters  
        AND dg.MaxLetinMeters  <= @MaxDistanceInMeters
        AND dg.IdDonations = @IdDonations
		AND d.IdSystems = @IdSystems
		AND dn.IdLanguages = @IdLanguages      
		AND dg.Active = 1 AND d.Active = 1 AND dn.Active = 1 AND u.Active = 1  

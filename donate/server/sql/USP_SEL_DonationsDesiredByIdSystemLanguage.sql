CREATE OR ALTER PROCEDURE USP_SEL_DonationsDesiredByIdSystemLanguage 
@Id				int,
@IdSystems 		int,
@IdLanguages 	int
AS 
	SELECT   
		dd.Id,
		dd.Id AS IdDonationsDesired,
		dd.DtUpdate,   
		dd.DtExp,
		dd.MaxGetinMeters,		
		dd.Active,
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
		dd.Id,
		dd.Id as IdParent,
		dd.Address,   
		dd.City,
		dd.State,
		dd.ZipCode,
		dd.Latitude,
		dd.Longitude
	FROM   
		DonationsDesired dd   
		INNER JOIN Donations d ON dd.IdDonations = d.Id   
		INNER JOIN DonationsNames dn ON dd.IdDonations = dn.IdDonations   
		INNER JOIN CG4UCore..Users u ON dd.IdUserOwner = u.Id   
	WHERE
		dd.Id = @Id
		AND d.IdSystems = @IdSystems
		AND dn.IdLanguages = @IdLanguages      
		AND dd.Active = 1 AND d.Active = 1 AND dn.Active = 1 AND u.Active = 1  

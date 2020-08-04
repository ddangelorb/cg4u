CREATE OR ALTER PROCEDURE USP_SEL_DonationsDesiredById 
@Id int
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
		INNER JOIN CG4UCore..Users u ON dd.IdUserOwner = u.Id   
	WHERE
		dd.Id = @Id
		AND dd.Active = 1 AND d.Active = 1 AND u.Active = 1  

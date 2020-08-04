CREATE OR ALTER PROCEDURE USP_SEL_DonationsById 
@Id				int,
@IdSystems 		int,
@IdLanguages 	int
AS 
	SELECT  
		d.Id,
		d.Id AS IdDonations,
		d.IdSystems,   
		d.IdDonationsDad,   
		d.Img,   
		dn.Id AS Names_Id,
		dn.Id AS Names_IdDonationsNames,  
		dn.IdDonations AS Names_IdDonations,
		dn.IdLanguages AS Names_IdLanguages,   
		dn.Name AS Names_Name   
	FROM   
		Donations d    
		INNER JOIN DonationsNames dn ON d.Id = dn.IdDonations   
	WHERE   
		d.Id = @Id     
		AND d.IdSystems = @IdSystems
		AND dn.IdLanguages = @IdLanguages
		AND d.Active = 1 AND dn.Active = 1  
GO

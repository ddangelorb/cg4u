CREATE OR ALTER PROCEDURE USP_SEL_DonationsSystemByLanguageAndName
@IdSystems		int,
@IdLanguages 	int,
@Name 			varchar(50)
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
		d.IdSystems = @IdSystems
		AND dn.IdLanguages = @IdLanguages 
		AND dn.Name COLLATE Latin1_General_CI_AI Like '%' + @Name + '%' COLLATE Latin1_General_CI_AI
		AND d.Active = 1 AND dn.Active = 1
GO

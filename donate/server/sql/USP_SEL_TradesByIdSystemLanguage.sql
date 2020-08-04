CREATE OR ALTER PROCEDURE USP_SEL_TradesByIdSystemLanguage 
@Id				int,
@IdSystems 		int,
@IdLanguages 	int
AS 
	SELECT
		t.Id,
		t.Id AS IdTrades,
		t.DtTrade,
		t.Commited,
		t.Active,
		ug.Id,
		ug.IdUserIdentity,
		ug.Email,
		ug.FirstName,
		ug.SurName,
		ug.Avatar,
		ug.Active,
		ul.Id,
		ul.IdUserIdentity,
		ul.Email,
		ul.FirstName,
		ul.SurName,
		ul.Avatar,
		ul.Active,
		dg.Id,
		dg.Id AS IdDonationsGivens,
		dg.DtUpdate,   
		dg.DtExp,
		dg.Img,
		dg.MaxLetinMeters,		
		dg.Active,
		dd.Id,
		dd.Id AS IdDonationsDesired,
		dd.DtUpdate,   
		dd.DtExp,
		dd.MaxGetinMeters,		
		dd.Active,
		dn.Id,
		dn.Id as IdDonationsNames,
		dn.IdDonations,
		dn.IdLanguages,   
		dn.Name,
		dn.Active
	FROM   
		Trades t
		INNER JOIN CG4UCore..Users ug ON t.IdUserGet = ug.Id
		INNER JOIN CG4UCore..Users ul ON t.IdUserLet = ul.Id
		INNER JOIN DonationsGivens dg ON t.IdDonationsGivens = dg.Id  
		INNER JOIN DonationsDesired dd ON t.IdDonationsDesired = dd.Id
		INNER JOIN Donations d ON dg.IdDonations = d.Id AND dd.IdDonations = d.Id
		INNER JOIN DonationsNames dn ON d.Id = dn.IdDonations
	WHERE
		t.Id = @Id
		AND d.IdSystems = @IdSystems
		AND dn.IdLanguages = @IdLanguages      
		AND t.Active = 1 AND ug.Active = 1 AND ul.Active = 1
		AND dg.Active = 1 AND dd.Active = 1 AND d.Active = 1 AND dn.Active = 1

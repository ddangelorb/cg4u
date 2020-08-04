CREATE OR ALTER PROCEDURE USP_SEL_TradesById 
@Id int
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
		dg.MaxLetinMeters,		
		dg.Active,
		dd.Id,
		dd.Id AS IdDonationsDesired,
		dd.DtUpdate,   
		dd.DtExp,
		dd.MaxGetinMeters,		
		dd.Active
	FROM   
		Trades t
		INNER JOIN CG4UCore..Users ug ON t.IdUserGet = ug.Id
		INNER JOIN CG4UCore..Users ul ON t.IdUserLet = ul.Id
		INNER JOIN DonationsGivens dg ON t.IdDonationsGivens = dg.Id  
		INNER JOIN DonationsDesired dd ON t.IdDonationsDesired = dd.Id
	WHERE
		t.Id = @Id
		AND t.Active = 1 AND ug.Active = 1 AND ul.Active = 1
		AND dg.Active = 1 AND dd.Active = 1

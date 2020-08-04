CREATE OR ALTER PROCEDURE USP_SEL_PersonGroupsById 
@Id int
AS 
	SELECT  
		pg.Id,
		pg.IdCustomers,
		pg.IdApi,
		pg.Name,
		pg.Active,
		a.Id,
		a.IdAnalyzesRequests,
		a.TypeCode,
		a.Message,
		a.ProcessingMethod, 
		a.ProcessingParam,
		a.Active
	FROM
		PersonGroups pg
		LEFT JOIN PersonGroupsAlerts pga ON pg.Id = pga.IdPersonGroups AND pga.Active = 1
		LEFT JOIN Alerts a ON a.Id = pga.IdAlerts AND a.Active = 1
	WHERE   
		pg.Id = @Id AND pg.Active = 1



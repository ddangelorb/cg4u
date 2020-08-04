CREATE OR ALTER PROCEDURE USP_SEL_PersonsByIdUser 
@IdUsers int
AS 
	SELECT  
		p.Id,
		p.IdPersonGroups,
		p.IdApi,
		p.IdUsers,
		p.Name,
		p.Active,
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
		a.Active,
		pf.Id,
		pf.IdPersons,
		pf.FaceId,
		pf.Active,
		pc.Id,
		pc.IdPersons,
		pc.PlateCode,
		pc.Active
	FROM
		Persons p
		INNER JOIN PersonGroups pg ON p.IdPersonGroups = pg.Id
		INNER JOIN PersonGroupsAlerts pga ON pg.Id = pga.IdPersonGroups
		INNER JOIN Alerts a ON a.Id = pga.IdAlerts
		LEFT JOIN PersonFaces pf ON p.Id = pf.IdPersons AND pf.Active = 1
		LEFT JOIN PersonCars pc ON  p.Id = pc.IdPersons AND pc.Active = 1
	WHERE   
		p.IdUsers = @IdUsers     
		AND p.Active = 1 AND pg.Active = 1 AND pga.Active = 1 AND a.Active = 1


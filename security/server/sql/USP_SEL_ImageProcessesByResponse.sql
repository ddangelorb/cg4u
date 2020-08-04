CREATE OR ALTER PROCEDURE USP_SEL_ImageProcessesByResponse
@ResponseQuery	nvarchar(255)
AS 
	SELECT
		ip.Id,
		ip.IdReference,
		ip.IdVideoCameras,
		ip.ImageFile, 
		ip.ImageName,
		ip.IpUserRequest,
		ip.VideoPath,
		ip.SecondsToStart,
		ip.DtProcess,
		ip.Active,
		ipa.Id,
		ipa.IdReference,
		ipa.IdImageProcesses,
		ipa.IdAnalyzesRequestsVideoCameras,
		ipa.DtAnalyze,
		ipa.ReturnResponseType,
		ipa.ReturnResponse, 
		ipa.Commited,
		ipa.Active
	FROM   
		ImageProcesses ip
		INNER JOIN ImageProcessAnalyzes ipa ON ip.Id = ipa.IdImageProcesses
	WHERE
		ipa.ReturnResponse like '%' + @ResponseQuery + '%' --FREETEXT((ipa.ReturnResponse), @ResponseQuery)
		AND ip.Active = 1 AND ipa.Active = 1


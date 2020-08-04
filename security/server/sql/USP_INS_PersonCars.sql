CREATE OR ALTER PROCEDURE USP_INS_PersonCars
@IdPersons 	int,
@PlateCode 	nvarchar(15)
AS
	INSERT INTO PersonCars (Id, IdPersons, PlateCode, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdPersons, 
		@PlateCode,
		1
	FROM PersonCars
GO
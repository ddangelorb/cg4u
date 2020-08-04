CREATE OR ALTER PROCEDURE USP_INS_Users
@IdUserIdentity nvarchar(450),
@Email nvarchar(256),
@FirstName nvarchar(50),
@SurName nvarchar(206),
@Avatar varbinary
AS
	INSERT INTO Users (Id, IdUserIdentity, Email, FirstName, SurName, Avatar, Authenticated,	DtExpAuth, Active)
	SELECT 
		ISNULL(MAX(Id), 0) + 1,
		@IdUserIdentity,
		@Email,
		@FirstName,
		@SurName,
		@Avatar,
		0,
		null,
		0
	FROM Users
GO

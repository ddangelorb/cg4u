CREATE OR ALTER PROCEDURE USP_UPD_Users
@Id				int,
@IdUserIdentity nvarchar(450),
@Email 			nvarchar(256),
@FirstName 		nvarchar(50),
@SurName 		nvarchar(206),
@Avatar 			varbinary,
@Active 			bit
AS 
	UPDATE Users
	SET
		IdUserIdentity = @IdUserIdentity, 
		Email = @Email, 
		FirstName = @FirstName, 
		SurName = @SurName, 
		Avatar = @Avatar, 
		Authenticated = 0,	
		DtExpAuth = null,	
		Active = @Active
	WHERE
		Id = @Id
GO
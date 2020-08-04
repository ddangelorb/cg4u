CREATE DATABASE CG4UAuth

CREATE LOGIN cg4uAuthUser WITH PASSWORD = '7d935a04-a719-4ab9-928c-122264e6df12'
CREATE USER cg4uAuthUser FOR LOGIN cg4uAuthUser

sp_help AspNetUsers

use CG4UAuth
select * from AspNetUsers
select * from AspNetUserRoles
select * from AspNetRoles
select * from AspNetRoleClaims
select * from AspNetUserClaims
select * from AspNetUserLogins
select * from AspNetUserTokens


/*
EXEC USP_INS_ObjectLocations
select ISNULL(MAX(Id), 0) + 1 from ObjectLocations
SELECT GEOGRAPHY::Point(47.455 , -122.231 , 4326)

sp_helptext USP_INS_ObjectLocations

select GETDATE()
select * from Systems
select * from Languages
select * from Users
select * from UsersSystems
select * from UserSystemPreferences
select * from Donations 
select * from DonationsNames

select * from DonationsGivens
select * from DonationsDesired

SELECT * FROM Donations d INNER JOIN DonationsNames dn ON d.Id = dn.IdDonations WHERE d.Active = 1 AND dn.Active = 1 AND dn.IdLanguages = 0 AND dn.Name Like '% %'

EXEC USP_SEL_DonationsDesiredByPosition 1,2,3,4

EXEC USP_SEL_DonationsById 1

SELECT * FROM Donations d INNER JOIN DonationsNames dn ON d.Id = dn.IdDonations
SELECT d.Id, d.IdSystems, dn.Id, dn.IdLanguages, dn.Name FROM Donations d INNER JOIN DonationsNames dn ON d.Id = dn.IdDonations

sp_help ObjectLocations

Rua Adolfo Pinheiro
-23.6427125
-46.699038
EXEC USP_SEL_ObjectLocationsByDistance 
	@IdObjects = 3, @IdLanguages = 1, @FromLatitude = -23.6427125, @FromLongitude = -46.699038, @MaxDistanceInMeters = 15000

*/

/*
 * --already did on SQL.Core.Load.sql
USE CG4UCore

insert into Systems(Id, Name, DtUpdate, Active) VALUES (1,'CG4U.Med', getdate(), 1)
insert into Systems(Id, Name, DtUpdate, Active) VALUES (2,'CG4U.Food', getdate(), 1)
insert into Systems(Id, Name, DtUpdate, Active) VALUES (3,'CG4U.Education', getdate(), 1)

insert into Languages(Id, Name, Code, DtUpdate, Active) VALUES (1,'Brazilian Portuguese','pt-BR', getdate(), 1)
insert into Languages(Id, Name, Code, DtUpdate, Active) VALUES (2,'English','en', getdate(), 1)
insert into Languages(Id, Name, Code, DtUpdate, Active) VALUES (3,'Spanish','es', getdate(), 1)
insert into Languages(Id, Name, Code, DtUpdate, Active) VALUES (4,'French','fr', getdate(), 0)
insert into Languages(Id, Name, Code, DtUpdate, Active) VALUES (5,'Italian','it', getdate(), 0)
insert into Languages(Id, Name, Code, DtUpdate, Active) VALUES (6,'German','de', getdate(), 0)
*/

--TODO: change to stored procedure
/* already did on WebAPI.Auth.Startup
 
insert into Users(Id, IdUserIdentity, Email, FirstName, SurName, Avatar, Authenticated, DtExpAuth, Active) 
	VALUES (1, 'ca68ad74-b483-4cad-8acc-dd58b7275325','danieldrb@hotmnail.com', 'Daniel', 'Barros', null, 1, null, 1)
insert into UsersSystems (Id, IdUsers, IdSystems, Active) VALUES (1, 1, 1, 1)
insert into UsersSystems (Id, IdUsers, IdSystems, Active) VALUES (2, 1, 2, 1)
insert into UsersSystems (Id, IdUsers, IdSystems, Active) VALUES (3, 1, 3, 1)
insert into UserSystemPreferences (Id, IdUsersSystems, MaxGetinMeters, MaxLetinMeters, Active) VALUES (1, 1, 10000, null, 1)
insert into UserSystemPreferences (Id, IdUsersSystems, MaxGetinMeters, MaxLetinMeters, Active) VALUES (2, 2, 10000, null, 1)
insert into UserSystemPreferences (Id, IdUsersSystems, MaxGetinMeters, MaxLetinMeters, Active) VALUES (3, 3, 10000, null, 1)
	
--TODO: change to stored procedure
insert into Users(Id, IdUserIdentity, Email, FirstName, SurName, Avatar, Authenticated, DtExpAuth, Active) 
	VALUES (2, '7a5d89e2-8e73-4a9d-8e7f-1de829c82533','fernandadangel@yahoo.com', 'Fernanda', 'Barros', null, 1, null, 1)
insert into UsersSystems (Id, IdUsers, IdSystems, Active) VALUES (4, 2, 1, 1)
insert into UsersSystems (Id, IdUsers, IdSystems, Active) VALUES (5, 2, 2, 1)
insert into UsersSystems (Id, IdUsers, IdSystems, Active) VALUES (6, 2, 3, 1)	
insert into UserSystemPreferences (Id, IdUsersSystems, MaxGetinMeters, MaxLetinMeters, Active) VALUES (4, 4, 15000, 5000, 1)
insert into UserSystemPreferences (Id, IdUsersSystems, MaxGetinMeters, MaxLetinMeters, Active) VALUES (5, 5, 15000, 5000, 1)
insert into UserSystemPreferences (Id, IdUsersSystems, MaxGetinMeters, MaxLetinMeters, Active) VALUES (6, 6, 15000, 5000, 1)
*/

USE CG4UDonate

insert into Donations(Id, IdSystems, IdDonationsDad, Img, Active) VALUES (1, 1, null, null, 1)
insert into DonationsNames(Id, IdDonations, IdLanguages, Name, Active) VALUES (1, 1, 1, N'Analgésicos e Antitérmicos', 1)
insert into DonationsNames(Id, IdDonations, IdLanguages, Name, Active) VALUES (2, 1, 2, N'Painkiller and antipyretics', 1)
insert into DonationsNames(Id, IdDonations, IdLanguages, Name, Active) VALUES (3, 1, 3, N'Analgésicos y antitérmicos', 1)

insert into Donations(Id, IdSystems, IdDonationsDad, Img, Active) VALUES (2, 1, null, null, 1)
insert into DonationsNames(Id, IdDonations, IdLanguages, Name, Active) VALUES (4, 2, 1, N'Antibióticos', 1)
insert into DonationsNames(Id, IdDonations, IdLanguages, Name, Active) VALUES (5, 2, 2, N'Antibiotics', 1)
insert into DonationsNames(Id, IdDonations, IdLanguages, Name, Active) VALUES (6, 2, 3, N'Antibióticos', 1)

insert into Donations(Id, IdSystems, IdDonationsDad, Img, Active) VALUES (3, 1, 1, null, 1)
insert into DonationsNames(Id, IdDonations, IdLanguages, Name, Active) VALUES (7, 3, 1, N'Tylenol Sinus', 1)
insert into DonationsNames(Id, IdDonations, IdLanguages, Name, Active) VALUES (8, 3, 2, N'Tylenol Sinus', 1)
insert into DonationsNames(Id, IdDonations, IdLanguages, Name, Active) VALUES (9, 3, 3, N'Tylenol Sinus', 1)

--EXEC USP_INS_ObjectLocations 3, 1, 'Rua Borba Gato 331 Ap 32 Torre J', 'São Paulo', 'SP', '04747030', -23.6565581, -46.7001006
-- SELECT * FROM CG4UCore..Users WHERE Id = ''
--EXEC USP_INS_ObjectLocations 3, 2, 'Rua Agostinho Gomes 2084 Casa 4 Ipiranga', 'São Paulo', 'SP', '04206900', -23.5860643, -46.6042267
-- sp_helptext USP_SEL_DonationsById
-- EXEC USP_SEL_DonationsById 1,1,1
-- sp_helptext USP_SEL_DonationsSystemByLanguageAndName
-- EXEC USP_SEL_DonationsSystemByLanguageAndName 1,1,'XPTO'
-- SELECT name, collation_name FROM sys.databases 
-- select convert(varchar,SERVERPROPERTY('Collation'))
-- https://stackoverflow.com/questions/16586116/save-accented-characters-in-sql-server?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
/*
 * 
DROP PROCEDURE USP_INS_ObjectLocations 
DROP PROCEDURE USP_SEL_ObjectLocationsByDistance
 
DROP TABLE TradeLocations
DROP TABLE  TradeEvaluations
DROP TABLE  UserSystemPreferences
DROP TABLE  Trades
DROP TABLE  UsersSystems
DROP TABLE  ObjectLocations
DROP TABLE  Users
DROP TABLE  ObjectsLanguages
DROP TABLE  Objects
DROP TABLE  Languages
DROP TABLE Systems 
  
 */

/*
-----
drop proc USP_INS_TTeste1
drop proc USP_SEL_TTeste1Address
DROP TABLE TTeste1Address
DROP TABLE TTeste1

CREATE TABLE TTeste1
(
Id int not null PRIMARY KEY,
nome varchar(50),
Active bit not null,
Img IMAGE null
)

CREATE TABLE TTeste1Address
(
Id int not null PRIMARY KEY,
idTTeste1 int not null,
address varchar(50),
Active bit not null,
FOREIGN KEY (idTTeste1) REFERENCES TTeste1(id),
)


CREATE OR ALTER PROCEDURE dbo.USP_INS_TTeste1 @nome varchar(50)
as 
INSERT INTO TTeste1 (Id, nome, Active) 	SELECT  ISNULL(MAX(id), 0) + 1, @nome, 1 FROM TTeste1

exec USP_INS_TTeste1 @nome ='teste1'
exec USP_INS_TTeste1 @nome ='teste2'
exec USP_INS_TTeste1 @nome ='teste3'

insert into TTeste1Address(Id, idTTeste1, address, Active) 	SELECT  ISNULL(MAX(id), 0) + 1, 1, 'endereco tteste1',  1 FROM TTeste1Address

CREATE OR ALTER PROCEDURE dbo.USP_SEL_TTeste1Address @id int
as 
select * from TTeste1 t inner join TTeste1Address ta on t.Id = ta.idTTeste1 WHERE t.Id = @id

EXEC USP_SEL_TTeste1Address 1



SELECT id, nome FROM TTeste1 WHERE id = (SELECT MAX(id) FROM TTeste1)

*/


/**/












/*
 Latitude and Longitude: http://www.mapcoordinates.net/pt
 * 
  
 * 
 ClassificaçãoTerapêutica - Anvisa - http://www.anvisa.gov.br/hotsite/genericos/profissionais/guia_genericos.pdf
 
Adsorventes e Antifiséticos Intestinais
Agentes Imunossupressores
Agentes Inotrópicos
Amebicidas, Giardicidas e Tricomonicidas
Analgésicos e Antitérmicos 
Anestésicos
Ansiolíticos
Antiácidos
Antiacnes
Antiagregantes Plaquetários 
Antialérgicos e Anti-histamínicos 
Antianêmicos
Antianginosos e Vasodilatadores 
Antiarrítmicos
Antiasmáticos e Broncodilatadores
Antibióticos
Anticonvulsivantes
Antidepressivos
Antidiabéticos
Antieméticos
Antiespasmódicos
Antifúngicos e Antimicóticos Anti-helmínticos . . . . . . . . Anti-hipertensivos . . . . . . . . Antiinflamatórios e Anti-reumáticos Antilipêmicos e Redutores de Colesterol
Antineoplásicos . . . Antiparkinsonianos
. . . . . . . . ....... . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ....... .
. . .. . . . . . . . . . . .. .. . . . . . .
. . . . . .... . . . . . . . . . . . . . . . . . . . . . . . . . . .... . .... . . . . . . . . . . . . . . . . .... .
Anti-retrovirais . .
Antiulcerosos . . .
Antivirais . . . . . .
Diuréticos . . . . .
Expectorantes . . Glicocorticóides
Hiperplasia Prostática Benigna Relaxantes Musculares . . . . . Repositores Eletrolíticos . . . . . Soluções Oftálmicas . . . . . . . Vasoconstritores e Hipertensores
*/

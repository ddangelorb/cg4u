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

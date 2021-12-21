DELETE FROM [dbo].[City];
DBCC CHECKIDENT ('City', RESEED, 0);
GO
DELETE FROM [dbo].[Country];
DBCC CHECKIDENT ('Country', RESEED, 0);
GO
DELETE FROM [dbo].[Continent];
DBCC CHECKIDENT ('Continent', RESEED, 0);
GO
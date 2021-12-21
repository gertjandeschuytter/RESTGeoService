CREATE TABLE [dbo].[Continent] (
    [ContinentId]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [Population] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([ContinentId] ASC)
);
GO
CREATE TABLE [dbo].[Country] (
    [CountryId]               INT             IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (150)  NOT NULL,
    [Population] INT             NOT NULL,
    [Surface]      DECIMAL (18, 2) NOT NULL,
    [ContinentId]      INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([CountryId] ASC),
    CONSTRAINT [FK_Country_Continent] FOREIGN KEY ([ContinentId]) REFERENCES [dbo].[Continent] ([ContinentId])
);
GO
CREATE TABLE [dbo].[City] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (150)  NOT NULL,
    [Population] INT             NOT NULL,
    [CountryId]           INT             NOT NULL,
    [IsCapital]      BIT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_City_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[country] ([CountryId])
);
GO
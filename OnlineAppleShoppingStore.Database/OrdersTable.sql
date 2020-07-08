CREATE TABLE [dbo].[Orders] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [FirstName]        NVARCHAR (160)  NOT NULL,
    [LastName]         NVARCHAR (160)  NOT NULL,
    [Address]          NVARCHAR (70)   NOT NULL,
    [City]             NVARCHAR (40)   NOT NULL,
    [State]            NVARCHAR (40)   NOT NULL,
    [PostalCode]       NVARCHAR (10)   NOT NULL,
    [Country]          NVARCHAR (40)   NOT NULL,
    [Phone]            NVARCHAR (24)   NOT NULL,
    [Email]            NVARCHAR (MAX)  NOT NULL,
    [DateCreated]      DATETIME2 (7)   NOT NULL,
    [Amount]           DECIMAL (18, 2) NOT NULL,
    [CustomerUserName] NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Orders] PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Customers] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [OrderId]   INT            NOT NULL,
    [FirstName] NVARCHAR (160) NOT NULL,
    [LastName]  NVARCHAR (160) NOT NULL,
    [Address]   NVARCHAR (MAX) NOT NULL,
    [Email]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.Customers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Customers_dbo.Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE
);


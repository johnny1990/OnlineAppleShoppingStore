
	CREATE TABLE [dbo].[Categories] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (45) NOT NULL,
    CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED ([Id] ASC)
);



CREATE TABLE [dbo].[Customers] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_dbo.Customers] PRIMARY KEY CLUSTERED ([Id] ASC)
);



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



CREATE TABLE [dbo].[Products] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (45)   NOT NULL,
    [Price]       DECIMAL (18, 2) NOT NULL,
    [Description] NVARCHAR (MAX)  NULL,
    [LastUpdated] DATETIME2 (7)   NOT NULL,
    [CategoryId]  INT             NOT NULL,
    CONSTRAINT [PK_dbo.Products] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Products_dbo.Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE
);


CREATE TABLE [dbo].[Carts] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CartId]      NVARCHAR (MAX) NULL,
    [ProductId]   INT            NOT NULL,
    [Count]       INT            NOT NULL,
    [DateCreated] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_dbo.Carts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Carts_dbo.Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProductId]
    ON [dbo].[Carts]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CategoryId]
    ON [dbo].[Products]([CategoryId] ASC);



	CREATE TABLE [dbo].[ProductsOrdered] (
    [ProductId] INT NOT NULL,
    [OrderId]   INT NOT NULL,
    [Quantity]  INT NOT NULL,
    CONSTRAINT [PK_dbo.ProductsOrdered] PRIMARY KEY CLUSTERED ([ProductId] ASC, [OrderId] ASC),
    CONSTRAINT [FK_dbo.ProductsOrdered_dbo.Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ProductsOrdered_dbo.Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProductId]
    ON [dbo].[ProductsOrdered]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerOrderId]
    ON [dbo].[ProductsOrdered]([OrderId] ASC);


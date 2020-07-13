CREATE TABLE [dbo].[DeliverOrders] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [OrderId]      INT             NOT NULL,
    [FirstName]    NVARCHAR (160)  NOT NULL,
    [LastName]     NVARCHAR (160)  NOT NULL,
    [Address]      NVARCHAR (70)   NOT NULL,
    [Phone]        NVARCHAR (24)   NOT NULL,
    [Email]        NVARCHAR (MAX)  NOT NULL,
    [OrderDate]    DATETIME2 (7)   NOT NULL,
    [Amount]       DECIMAL (18, 2) NOT NULL,
    [DeliveryDate] DATETIME2 (7)   NOT NULL,
    [Status]       NVARCHAR (100)  NOT NULL,
    CONSTRAINT [PK_dbo.DeliverOrders] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.DeliverOrders_dbo.Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE
);


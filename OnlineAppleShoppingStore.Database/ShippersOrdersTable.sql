CREATE TABLE [dbo].[ShippersOrders] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (40) NOT NULL,
    [Phone] NVARCHAR (24) NULL,
    CONSTRAINT [PK_ShippersOrders] PRIMARY KEY CLUSTERED ([Id] ASC)
);

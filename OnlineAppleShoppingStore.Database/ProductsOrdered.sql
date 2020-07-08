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


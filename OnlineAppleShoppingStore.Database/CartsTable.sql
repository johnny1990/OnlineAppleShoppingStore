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


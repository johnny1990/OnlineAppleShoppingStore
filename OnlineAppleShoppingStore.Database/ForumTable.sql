CREATE TABLE [dbo].[Forums] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Title]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Forum] PRIMARY KEY CLUSTERED ([Id] ASC)
);


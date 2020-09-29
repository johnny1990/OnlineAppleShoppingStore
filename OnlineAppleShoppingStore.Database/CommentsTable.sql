CREATE TABLE [dbo].[Comments] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [ForumId] INT            NOT NULL,
    [Body]    NVARCHAR (MAX) NULL,
    [Title]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([Id] ASC)
);


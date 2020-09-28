CREATE TABLE [dbo].[Comments] (
    [CommentId] INT            IDENTITY (1, 1) NOT NULL,
    [Comment]   NVARCHAR (MAX) NULL,
    [TopicId]   INT            NOT NULL,
    CONSTRAINT [PK_dbo.Comments] PRIMARY KEY CLUSTERED ([CommentId] ASC),
    CONSTRAINT [FK_dbo.Comments_dbo.Topics_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[Topics] ([TopicId]) ON DELETE CASCADE
);


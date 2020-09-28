CREATE TABLE [dbo].[Topics] (
    [TopicId]     INT            IDENTITY (1, 1) NOT NULL,
    [TopicName]   NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (120) NOT NULL,
    [Body]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Topics] PRIMARY KEY CLUSTERED ([TopicId] ASC)
);


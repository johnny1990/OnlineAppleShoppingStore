CREATE TABLE [dbo].[Feedback] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [FromName]  NVARCHAR (50)  NULL,
    [FromEmail] NVARCHAR (50)  NULL,
    [FeedBack]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Group]
(
	[Id]								INT IDENTITY (1, 1)				NOT NULL,
	[Uid]								UNIQUEIDENTIFIER				NOT NULL,
	[CreatedOn]							SMALLDATETIME					NOT NULL,
	[DeletedOn]							SMALLDATETIME					NULL,
	[Name]								NVARCHAR (150)					NOT NULL,
	[Description]						NVARCHAR (300)					NULL,
	CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Group_Uid]
	ON [dbo].[Group] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
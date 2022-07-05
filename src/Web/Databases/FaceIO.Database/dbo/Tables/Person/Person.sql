CREATE TABLE [dbo].[Person]
(
	[Id]								INT IDENTITY (1, 1)				NOT NULL,
	[Uid]								UNIQUEIDENTIFIER				NOT NULL,
	[CreatedOn]							SMALLDATETIME					NOT NULL,
	[DeletedOn]							SMALLDATETIME					NULL,
	[Name]								NVARCHAR (150)					NOT NULL,
	CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Person_Uid]
	ON [dbo].[Person] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
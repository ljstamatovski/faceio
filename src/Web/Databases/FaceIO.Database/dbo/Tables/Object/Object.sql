CREATE TABLE [dbo].[Object]
(
	[Id]								INT IDENTITY (1, 1)				NOT NULL,
	[Uid]								UNIQUEIDENTIFIER				NOT NULL,
	[CreatedOn]							SMALLDATETIME					NOT NULL,
	[DeletedOn]							SMALLDATETIME					NULL,
	[Name]								NVARCHAR (150)					NOT NULL,
	[Description]						NVARCHAR (300)					NULL,
	CONSTRAINT [PK_Object] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Object_Uid]
	ON [dbo].[Object] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
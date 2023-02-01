CREATE TABLE [dbo].[Location]
(
	[Id]								INT IDENTITY (1, 1)				NOT NULL,
	[Uid]								UNIQUEIDENTIFIER				NOT NULL,
	[CreatedOn]							SMALLDATETIME					NOT NULL,
	[DeletedOn]							SMALLDATETIME					NULL,
	[CustomerFk]						INT								NOT NULL,
	[Name]								NVARCHAR (150)					NOT NULL,
	[Description]						NVARCHAR (300)					NULL,
	[CollectionId]						NVARCHAR (75)					NOT NULL,
	CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Location_Customer] FOREIGN KEY ([CustomerFk]) REFERENCES [dbo].[Customer] ([Id])
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Location_Uid]
	ON [dbo].[Location] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
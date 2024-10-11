CREATE TABLE [dbo].[Person]
(
	[Id]								INT IDENTITY (1, 1)				NOT NULL,
	[Uid]								UNIQUEIDENTIFIER				NOT NULL,
	[CreatedOn]							SMALLDATETIME					NOT NULL,
	[DeletedOn]							SMALLDATETIME					NULL,
	[CustomerFk]						INT								NOT NULL,
	[Name]								NVARCHAR (150)					NOT NULL,
	[FileName]							NVARCHAR (500)					NULL,
	[Email]								NVARCHAR(150)					NULL, 
    [Phone]								NVARCHAR(20)					NULL, 
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Person_Customer] FOREIGN KEY ([CustomerFk]) REFERENCES [dbo].[Customer] ([Id])
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Person_Uid]
	ON [dbo].[Person] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
CREATE TABLE [dbo].[PersonImage]
(
	[Id]			INT IDENTITY (1, 1)		NOT NULL,
	[Uid]			UNIQUEIDENTIFIER		NOT NULL,
	[CreatedOn]		SMALLDATETIME			NOT NULL,
	[DeletedOn]		SMALLDATETIME			NULL,
	[PersonFk]		INT						NOT NULL,
	[FileName]		NVARCHAR(500)			NOT NULL,
	CONSTRAINT [PK_PersonImage] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_PersonImage_Person] FOREIGN KEY ([PersonFk]) REFERENCES [dbo].[Person] ([Id])
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PersonImage_Uid]
	ON [dbo].[PersonImage] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
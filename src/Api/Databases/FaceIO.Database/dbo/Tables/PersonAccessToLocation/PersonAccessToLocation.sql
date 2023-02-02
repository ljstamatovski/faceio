CREATE TABLE [dbo].[PersonAccessToLocation]
(
	[Id]						INT IDENTITY (1, 1)		NOT NULL,
	[Uid]						UNIQUEIDENTIFIER		NOT NULL,
	[CreatedOn]					SMALLDATETIME			NOT NULL,
	[DeletedOn]					SMALLDATETIME			NULL,
	[PersonFk]					INT						NOT NULL,
	[GroupAccessToLocationFk]	INT						NOT NULL,
	[FaceId]					NVARCHAR(50)			NULL,
	CONSTRAINT [PK_PersonAccessToLocation] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_PersonAccessToLocation_Person] FOREIGN KEY ([PersonFk]) REFERENCES [dbo].[Person] ([Id]),
	CONSTRAINT [FK_PersonAccessToLocation_GroupAccessToLocation] FOREIGN KEY ([GroupAccessToLocationFk]) REFERENCES [dbo].[GroupAccessToLocation] ([Id])
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PersonAccessToLocation_Uid]
	ON [dbo].[PersonAccessToLocation] ([Uid] ASC) WHERE([DeletedOn] IS NULL);

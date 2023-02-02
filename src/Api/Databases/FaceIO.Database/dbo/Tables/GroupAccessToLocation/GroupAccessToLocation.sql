CREATE TABLE [dbo].[GroupAccessToLocation]
(
	[Id]			INT IDENTITY (1, 1)		NOT NULL,
	[Uid]			UNIQUEIDENTIFIER		NOT NULL,
	[CreatedOn]		SMALLDATETIME			NOT NULL,
	[DeletedOn]		SMALLDATETIME			NULL,
	[GroupFk]		INT						NOT NULL,
	[LocationFk]	INT						NOT NULL,
	CONSTRAINT [PK_GroupAccessToLocation] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_GroupAccessToLocation_Group] FOREIGN KEY ([GroupFk]) REFERENCES [dbo].[Group] ([Id]),
	CONSTRAINT [FK_GroupAccessToLocation_Location] FOREIGN KEY ([LocationFk]) REFERENCES [dbo].[Location] ([Id])
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GroupAccessToLocation_Uid]
	ON [dbo].[GroupAccessToLocation] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
CREATE TABLE [dbo].[GroupLocation]
(
	[Id]			INT IDENTITY (1, 1)		NOT NULL,
	[Uid]			UNIQUEIDENTIFIER		NOT NULL,
	[CreatedOn]		SMALLDATETIME			NOT NULL,
	[DeletedOn]		SMALLDATETIME			NULL,
	[GroupFk]		INT						NOT NULL,
	[LocationFk]	INT						NOT NULL,
	CONSTRAINT [PK_GroupLocation] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_GroupLocation_Group] FOREIGN KEY ([GroupFk]) REFERENCES [dbo].[Group] ([Id]),
	CONSTRAINT [FK_GroupLocation_Location] FOREIGN KEY ([LocationFk]) REFERENCES [dbo].[Location] ([Id])
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GroupLocation_Uid]
	ON [dbo].[GroupLocation] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
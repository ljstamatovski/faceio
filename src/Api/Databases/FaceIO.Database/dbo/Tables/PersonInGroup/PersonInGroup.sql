CREATE TABLE [dbo].[PersonInGroup]
(
	[Id]			INT IDENTITY (1, 1)		NOT NULL,
	[Uid]			UNIQUEIDENTIFIER		NOT NULL,
	[CreatedOn]		SMALLDATETIME			NOT NULL,
	[DeletedOn]		SMALLDATETIME			NULL,
	[GroupFk]		INT						NOT NULL,
	[PersonFk]		INT						NOT NULL,
	CONSTRAINT [PK_PersonInGroup] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_PersonInGroup_Group] FOREIGN KEY ([GroupFk]) REFERENCES [dbo].[Group] ([Id]),
	CONSTRAINT [FK_PersonInGroup_Person] FOREIGN KEY ([PersonFk]) REFERENCES [dbo].[Person] ([Id])
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PersonInGroup_Uid]
	ON [dbo].[PersonInGroup] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
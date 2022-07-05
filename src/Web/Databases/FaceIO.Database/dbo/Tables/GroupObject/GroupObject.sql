CREATE TABLE [dbo].[GroupObject]
(
	[Id]			INT IDENTITY (1, 1)		NOT NULL,
	[Uid]			UNIQUEIDENTIFIER		NOT NULL,
	[CreatedOn]		SMALLDATETIME			NOT NULL,
	[DeletedOn]		SMALLDATETIME			NULL,
	[GroupFk]		INT						NOT NULL,
	[ObjectFk]		INT						NOT NULL,
	CONSTRAINT [PK_GroupObject] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_GroupObject_Group] FOREIGN KEY ([GroupFk]) REFERENCES [dbo].[Group] ([Id]),
	CONSTRAINT [FK_GroupObject_Object] FOREIGN KEY ([ObjectFk]) REFERENCES [dbo].[Object] ([Id])
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GroupObject_Uid]
	ON [dbo].[GroupObject] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
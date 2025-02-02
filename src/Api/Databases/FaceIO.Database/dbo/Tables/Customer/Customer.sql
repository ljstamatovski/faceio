﻿CREATE TABLE [dbo].[Customer]
(
	[Id]								INT IDENTITY (1, 1)				NOT NULL,
	[Uid]								UNIQUEIDENTIFIER				NOT NULL,
	[CreatedOn]							SMALLDATETIME					NOT NULL,
	[DeletedOn]							SMALLDATETIME					NULL,
	[Name]								NVARCHAR (150)					NOT NULL,
	CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Customer_Uid]
	ON [dbo].[Customer] ([Uid] ASC) WHERE([DeletedOn] IS NULL);
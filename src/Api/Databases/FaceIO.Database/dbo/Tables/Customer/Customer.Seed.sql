SET IDENTITY_INSERT [dbo].[Customer] ON

INSERT INTO [dbo].[Customer] (Id, Uid, CreatedOn, DeletedOn, Name)
VALUES
    (1, '316f1c85-8e01-4749-845e-768b22219244', GETUTCDATE(), NULL, 'FaceIO')

SET IDENTITY_INSERT [dbo].[Customer] OFF
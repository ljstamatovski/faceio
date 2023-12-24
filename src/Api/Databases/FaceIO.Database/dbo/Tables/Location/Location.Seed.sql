SET IDENTITY_INSERT [dbo].[Location] ON

INSERT INTO [dbo].[Location] (Id, Uid, CreatedOn, DeletedOn, CustomerFk, Name, Description, CollectionId)
VALUES
    (1 ,'919f1e4f-025d-4ce2-b4ee-8223d23b278c', GETUTCDATE(), NULL, 1, 'Workplace 1', 'Main Office', 'Office'),
    (2 ,NEWID(), GETUTCDATE(), NULL, 1, 'Home 1', 'Primary Residence', 'Residential'),
    (3 ,NEWID(), GETUTCDATE(), NULL, 1, 'Warehouse 1', 'Distribution Center', 'Warehouse'),
    (4 ,NEWID(), GETUTCDATE(), NULL, 1, 'Workplace 2', 'Branch Office', 'Office'),
    (5 ,NEWID(), GETUTCDATE(), NULL, 1, 'Home 2', 'Secondary Residence', 'Residential'),
    (6 ,NEWID(), GETUTCDATE(), NULL, 1, 'Warehouse 2', 'Storage Facility', 'Warehouse'),
    (7 ,NEWID(), GETUTCDATE(), NULL, 1, 'Workplace 3', 'Headquarters', 'Office'),
    (8 ,NEWID(), GETUTCDATE(), NULL, 1, 'Home 3', 'Vacation Home', 'Residential'),
    (9 ,NEWID(), GETUTCDATE(), NULL, 1, 'Warehouse 3', 'Logistics Center', 'Warehouse'),
    (10 ,NEWID(), GETUTCDATE(), NULL, 1, 'Workplace 4', 'Remote Office', 'Office'),
    (11 ,NEWID(), GETUTCDATE(), NULL, 1, 'Home 4', 'City Apartment', 'Residential'),
    (12 ,NEWID(), GETUTCDATE(), NULL, 1, 'Warehouse 4', 'Manufacturing Plant', 'Warehouse'),
    (13 ,NEWID(), GETUTCDATE(), NULL, 1, 'Workplace 5', 'Co-Working Space', 'Office'),
    (14 ,NEWID(), GETUTCDATE(), NULL, 1, 'Home 5', 'Suburban House', 'Residential'),
    (15 ,NEWID(), GETUTCDATE(), NULL, 1, 'Warehouse 5', 'Cold Storage', 'Warehouse')

SET IDENTITY_INSERT [dbo].[Location] OFF
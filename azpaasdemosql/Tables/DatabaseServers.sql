CREATE TABLE [dbo].[DatabaseServers]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [DatabaseServer] VARCHAR(MAX) NULL, 
    [DatabaseName] VARCHAR(50) NULL,
	[Region] VARCHAR(50) NULL

)

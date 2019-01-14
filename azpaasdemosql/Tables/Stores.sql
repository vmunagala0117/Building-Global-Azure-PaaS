﻿CREATE TABLE [dbo].[Stores]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NULL, 
    [Country] VARCHAR(50) NULL, 
    [DatabaseServerId] INT NULL, 
    CONSTRAINT [FK_Stores_DatabaseServers] FOREIGN KEY ([DatabaseServerId]) REFERENCES [DatabaseServers](Id)
)

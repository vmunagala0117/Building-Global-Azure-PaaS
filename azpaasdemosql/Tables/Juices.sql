﻿CREATE TABLE [dbo].[Juices]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NULL, 
    [ImageUrl] NVARCHAR(MAX) NULL, 
    [Price] FLOAT NULL
)

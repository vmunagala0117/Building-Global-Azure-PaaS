CREATE TABLE [dbo].[OrderLines]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Quantity] INT NULL, 
    [JuiceId] INT NULL, 
    [OrderId] INT NULL, 
    CONSTRAINT [JuiceForeignKey] FOREIGN KEY (JuiceId) REFERENCES Juices(Id), 
    CONSTRAINT [OrdersForeignKey] FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
)

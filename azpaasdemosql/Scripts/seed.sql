IF NOT EXISTS(SELECT 1 FROM dbo.Stores)
BEGIN

	INSERT INTO dbo.Juices ([ImageUrl],[Name],[Price]) VALUES ('https://paasdemo.azureedge.net/picsblob/beet_juice.jpg', 'Bloody Beets', 10.50)
	INSERT INTO dbo.Juices ([ImageUrl],[Name],[Price]) VALUES ('https://paasdemo.azureedge.net/picsblob/carrot-juice.jpg', 'Red Carrot', 20.0)
	INSERT INTO dbo.Juices ([ImageUrl],[Name],[Price]) VALUES ('https://paasdemo.azureedge.net/picsblob/apple-juice.jpg', 'Golden Apple', 6.0)

	INSERT INTO dbo.Stores ([Name], [Country]) VALUES ('VV Stores', 'USA')
	INSERT INTO dbo.Stores ([Name], [Country]) VALUES ('Sippy Juice Stores', 'India')
	INSERT INTO dbo.Stores ([Name], [Country]) VALUES ('Green Juice Stores', 'Australia')

	--INSERT INTO dbo.Orders([Date],[Price],[StoreId]) VALUES('2018-01-12', 20, 1)

	--INSERT INTO dbo.OrderLines([JuiceId], [OrderId], [Quantity]) VALUES(1,1,3)

END

IF NOT EXISTS(SELECT 1 FROM dbo.DatabaseServers)
BEGIN
	
	INSERT INTO dbo.DatabaseServers ([DatabaseServer],[DatabaseName],[Region]) VALUES ('azpaasdemosqlservereastus.database.windows.net','azpaasdemo-db-eastus','US')
	UPDATE dbo.Stores SET DatabaseServerId = @@IDENTITY WHERE Country = 'USA' 
		
	INSERT INTO dbo.DatabaseServers ([DatabaseServer],[DatabaseName],[Region]) VALUES ('azpaasdemosqlserversouthindia.database.windows.net','azpaasdemo-db-india','India')
	UPDATE dbo.Stores SET DatabaseServerId = @@IDENTITY WHERE Country = 'India'

	INSERT INTO dbo.DatabaseServers ([DatabaseServer],[DatabaseName],[Region]) VALUES ('azpaasdemosqlserveraustraliaeast.database.windows.net','azpaasdemo-db-aus','Australia')
	UPDATE dbo.Stores SET DatabaseServerId = @@IDENTITY WHERE Country = 'Australia'
END
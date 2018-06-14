USE [master]
GO
EXEC sp_addlinkedserver 
   @server = 'spatialite', -- the name you give the server in SSMS 
   @srvproduct = '', -- Can be blank but not NULL
   @provider = 'MSDASQL', 
   @datasrc = 'spatialite-template' -- the name of the system dsn connection you created
GO


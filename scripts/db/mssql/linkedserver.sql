USE [master]
GO
EXEC master.dbo.sp_addlinkedserver @server = N'POSTGIS', @srvproduct=N'Postgresql', @provider=N'MSDASQL', @datasrc=N'PostgreSQL95'

GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'collation compatible', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'data access', @optvalue=N'true'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'dist', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'pub', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'rpc', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'rpc out', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'sub', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'connect timeout', @optvalue=N'0'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'collation name', @optvalue=null
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'lazy schema validation', @optvalue=N'false'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'query timeout', @optvalue=N'0'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'use remote collation', @optvalue=N'true'
GO
EXEC master.dbo.sp_serveroption @server=N'POSTGIS', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO
USE [master]
GO
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname = N'POSTGIS', @locallogin = NULL , @useself = N'False'
GO

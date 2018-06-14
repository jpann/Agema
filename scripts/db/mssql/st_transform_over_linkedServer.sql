

USE [BMPTracking]
GO
 

/****** Object:  UserDefinedFunction [dbo].[ST_Transform]    Script Date: 5/9/2018 12:45:43 PM ******/
DROP FUNCTION [dbo].[ST_Transform]
GO



CREATE FUNCTION dbo.ST_Transform(@sourceWKT nvarchar(max), @sourceSrid int, @targetSrid int ) RETURNS nvarchar(max)
AS BEGIN

	DECLARE @pgsql varchar(8000);
	DECLARE @projectedWkt nvarchar(max);

	SET @pgsql  = CONCAT( 'select ST_AsText(ST_Transform(ST_GeomFromText( '
		,''''
		,@sourceWKT
		,''''
		,','
		,@sourceSrid
		,'),'
		,@targetSrid
		,')) as wkt'
	);

	SELECT  @projectedWkt = wkt   FROM OPENQUERY(POSTGIS,@pgsql) ;
	

	 
	RETURN @projectedWkt;

END


GO



SELECT dbo.ST_Transform('POINT(-71.160281 42.258729)',4269,3857)


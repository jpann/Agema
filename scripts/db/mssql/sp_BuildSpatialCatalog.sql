-- =============================================
-- Author:		Jay Cummins (cumminsjp@gmail.com)
-- Create date: 2018-04-20
-- Description:	Does a quick and dirty inventory of tables w/ Geometry/Geography columns 
--              and gets a spatial reference ID from a single row.
-- =============================================

-- Drop the type and rebuild. Might add onto to the table later.
DROP TYPE dbo.SpatialCatalogTableType;
GO

IF TYPE_ID('dbo.SpatialCatalogTableType') IS NULL -- Check if type exists
 
	 CREATE TYPE dbo.SpatialCatalogTableType AS TABLE(
				TABLE_CATALOG nvarchar(128),
				TABLE_SCHEMA nvarchar(128),
				TABLE_NAME nvarchar(128),
				COLUMN_NAME nvarchar(128),
				SRID int
	);

GO

-- dbo.sp_BuildSpatialCatalog

DECLARE @cur CURSOR;

DECLARE @cat nvarchar(128);
DECLARE @schema nvarchar(128);
DECLARE @tableName nvarchar(128);
DECLARE @columnName nvarchar(128);

DECLARE @srid int;

DECLARE @table dbo.SpatialCatalogTableType;
		
BEGIN
	SET @cur = CURSOR FOR

		SELECT 
			--TOP 10 
			c.TABLE_CATALOG
			,c.TABLE_SCHEMA
			,c.TABLE_NAME
			,c.COLUMN_NAME
		FROM INFORMATION_SCHEMA.TABLES t
			INNER JOIN INFORMATION_SCHEMA.COLUMNS c ON
			t.TABLE_CATALOG = c.TABLE_CATALOG
			AND t.TABLE_SCHEMA = c.TABLE_SCHEMA
			AND t.TABLE_NAME = c.TABLE_NAME	
		WHERE t.TABLE_TYPE = 'BASE TABLE'
			AND DATA_TYPE in ('geometry','geography')
			AND t.TABLE_CATALOG = 'BMPTracking'
		ORDER BY ORDINAL_POSITION

	OPEN @cur 
	FETCH NEXT FROM @cur 
	INTO @cat,@schema,@tableName,@columnName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @sridParam NVARCHAR(20) = '@o INT OUTPUT'
		DECLARE @statement nvarchar(500) = CONCAT('SELECT  TOP 1 @o=',@columnName,'.STSrid FROM ',@schema,'.',@tableName,';');

		EXECUTE sp_executesql @statement, @sridParam, @o = @srid OUTPUT;

		INSERT INTO @table VALUES(@cat,@schema,@tableName,@columnName,@srid);
		SET @srid = 0;
	FETCH NEXT FROM @cur 
		INTO @cat,@schema,@tableName,@columnName
	END; 

	CLOSE @cur ;
	DEALLOCATE @cur;
END;	
		
SELECT  * FROM @table;
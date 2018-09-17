-- =============================================
-- Author:		Jay Cummins (cumminsjp@gmail.com)
-- Create date: 2018-04-20
-- Description:	Performs a text search of sql modules, tables, and columns (e.g. stored procudures) for string matches.
-- =============================================

IF EXISTS ( SELECT  1
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'dbo.Search')
                    AND type IN ( N'F',N'TF') 
		) 
	BEGIN
		DROP FUNCTION dbo.Search;
		Print 'Procedure dbo.Search dropped.';
	END
	ELSE
		Print 'Procedure dbo.Search does not exist.';
GO

Print 'Creating procedure dbo.Search.';
GO 

CREATE FUNCTION dbo.Search
(
	@search varchar(255)
)
RETURNS 
 @t TABLE (
	object_Name sysname
	,type_desc nvarchar(120)
	,definition nvarchar(max)
	,schema_name nvarchar(256)
)

AS
BEGIN
	INSERT INTO @t
			SELECT DISTINCT o.name AS object_Name
			,o.type_desc
			,cast(m.DEFINITION COLLATE Latin1_General_CI_AS_KS_WS AS NVARCHAR(max)) AS DEFINITION
			,OBJECT_SCHEMA_NAME(o.object_id) AS schema_name
		FROM sys.sql_modules m
		INNER JOIN sys.objects o ON m.object_id = o.object_id
		WHERE m.DEFINITION LIKE '%' + @Search + '%'

		UNION

		SELECT cast(CONCAT (
					TABLE_CATALOG
					,'.'
					,TABLE_SCHEMA
					,'.'
					,TABLE_NAME
					) COLLATE Latin1_General_CI_AS_KS_WS AS NVARCHAR(max)) AS DEFINITION
			,cast(TABLE_TYPE COLLATE Latin1_General_CI_AS_KS_WS AS NVARCHAR(max))
			,cast('' COLLATE Latin1_General_CI_AS_KS_WS AS NVARCHAR(max))
			,TABLE_SCHEMA
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_NAME LIKE '%' + @Search + '%'

		UNION

		SELECT CONCAT (
				TABLE_CATALOG
				,'.'
				,TABLE_SCHEMA
				,'.'
				,TABLE_NAME
				,'.'
				,COLUMN_NAME
				)
			,'COLUMN'
			,NULL
			,TABLE_SCHEMA
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE COLUMN_NAME LIKE '%' + @Search + '%'
		ORDER BY o.name
			,o.type_desc;



		RETURN;
END;
GO

--sp_help 'INFORMATION_SCHEMA.TABLES'
--sp_help 'sys.sql_modules '
-- SELECT * FROM dbo.search('Category')

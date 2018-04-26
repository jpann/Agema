-- =============================================
-- Author:		Jay Cummins (cumminsjp@gmail.com)
-- Create date: 2018-04-20
-- Description:	Does a quick and dirty inventory of tables w/ Geometry/Geography columns 
--              and gets a spatial reference ID from a single row.
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
	SELECT DISTINCT
		o.name AS object_Name,o.type_desc,m.definition,OBJECT_SCHEMA_NAME (o.object_id) as schema_name
		
		FROM sys.sql_modules        m 
			INNER JOIN sys.objects  o ON m.object_id=o.object_id
		
		WHERE m.definition Like '%'+@Search+'%'
		ORDER BY o.name,o.type_desc;

		RETURN;
END;
GO

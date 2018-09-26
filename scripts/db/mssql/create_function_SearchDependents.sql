


IF EXISTS ( SELECT  1
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'dbo.SearchDependents')
                    AND type IN ( N'F',N'TF') 
		) 
	BEGIN
		DROP FUNCTION dbo.SearchDependents;
		Print 'Procedure dbo.SearchDependents dropped.';
	END
	ELSE
		Print 'Procedure dbo.SearchDependents does not exist.';
GO

Print 'Creating procedure dbo.SearchDependents.';
GO 

CREATE FUNCTION dbo.SearchDependents
(
	@search varchar(255)
)
RETURNS 
 @t TABLE (
	[ReferencingSchemaTableName] [nvarchar](257) NULL,
	[ReferencedSchemaTableName] [nvarchar](257) NULL,
	[ReferencingType] [nvarchar](60) NULL,
	[ReferencingObjectId] [int] NULL,
	[ReferencedObjectId] [int] NULL,
	[ReferencedSchemaName] [nvarchar](128) NULL,
	[Level] [int] NULL,
	[ObjectFullPath] [nvarchar](4000) NOT NULL
)
AS
BEGIN
	
with cte as (
	SELECT      
		DISTINCT
		--SP, View, or Function
		CONCAT(SCHEMA_NAME(schema_id),'.', o.name) as ReferencingSchemaTableName
		,CONCAT(ref.referenced_schema_name,'.', ref.referenced_entity_name) as ReferencedSchemaTableName
		--,SCHEMA_NAME(schema_id) as ReferencingSchema
		--, ReferencingName = o.name
		, ReferencingType = o.type_desc
		, o.object_id  as ReferencingObjectId

		--Referenced Field
		,ref.referenced_id as ReferencedObjectId
		--,ref.referenced_database_name --will be null if the DB is not explicitly called out
		,ref.referenced_schema_name  as ReferencedSchemaName --will be null or blank if the DB is not explicitly called out
		--,ref.referenced_entity_name
		--,ref.referenced_minor_name
	

	FROM sys.objects AS o 
	cross apply sys.dm_sql_referenced_entities(CONCAT(SCHEMA_NAME(schema_id),'.' + o.name), 'Object') ref
	where o.type in ('FN','IF','V','P','TF')
	and SCHEMA_NAME(schema_id) IS NOT NULL
	AND 		
		/* Exclude broken entities */
		o.name NOT IN ('sp_upgraddiagrams','BMPVerification_RunSelection','BulkUpdatePlanSpatiallyRelatedBMPs','SpatiallyIdentifyRankedHydrologicUnits'
		,'v_LogiReportAnalysisFolders'
		,'v_LogiReportAnalysisRoles'
		,'v_CB_BMPInstance_PY2013'
		,'v_LogiReportAnalysis')
	-- AND o.Name = 'CropToNmpFieldSeason'
		
	-- for other database object types use below line 
		--AND  o.type in ('FN','IF','V','P','TF')
)
, ObjectHierarchy ( ReferencingSchemaTableName	,ReferencedSchemaTableName,	ReferencingType,	ReferencingObjectId,	ReferencedObjectId,	ReferencedSchemaName , Level , Obj_Path) as 
(
	SELECT 
		ReferencingSchemaTableName,	ReferencedSchemaTableName,	ReferencingType,	ReferencingObjectId	,ReferencedObjectId	,ReferencedSchemaName
			,0 as Level 
			,convert ( nvarchar ( 4000  ) , N'/' + ReferencingSchemaTableName ) as Obj_Path 
		FROM cte
		WHERE 
		--cte.ReferencingSchemaTableName = 'nmp.GetNutrientBalance'
		ReferencingSchemaTableName in (@search)		

		UNION ALL 

		SELECT 
				cte.ReferencingSchemaTableName,	cte.ReferencedSchemaTableName,	cte.ReferencingType,	ref.ReferencingObjectId	,cte.ReferencedObjectId	,cte.ReferencedSchemaName
				, Level + 1 as Level 			
				, convert ( nvarchar ( 4000 ) , ref.Obj_Path + N'/' + cte.ReferencingSchemaTableName ) as Obj_Path 
			FROM cte
			JOIN  ObjectHierarchy ref on cte.ReferencingSchemaTableName = ref.ReferencedSchemaTableName
			
			WHERE ref.Obj_Path not like '%/' + cte.ReferencedSchemaTableName + '/%' 
		
)
INSERT INTO @t
SELECT 
	ReferencingSchemaTableName,ReferencedSchemaTableName,ReferencingType,ReferencingObjectId,ReferencedObjectId,ReferencedSchemaName,Level
	,CONCAT(Obj_Path,'/',ReferencedSchemaTableName) as ObjectFullPath

FROM ObjectHierarchy
ORDER BY Obj_Path;

		RETURN;
END;
GO
 
/*
 

*/




















 
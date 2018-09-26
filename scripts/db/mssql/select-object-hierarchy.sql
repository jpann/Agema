/*
Started with this answer on  https://stackoverflow.com/a/41678117/386619

Added a SELECT for dm_sql_referenced_entities.

But the big //TODO is obtain the hierarchy below the object with a recursive query.

*/

DECLARE @schemaAndObjectName VARCHAR(28) = 'nmp.GetNutrientBalance';


;with ObjectHierarchy ( Base_Object_Id , Base_Cchema_Id , Base_Object_Name , Base_Object_Type, object_id , Schema_Id , Name , Type_Desc , Level , Obj_Path) 
as 
    ( select  so.object_id as Base_Object_Id 
        , so.schema_id as Base_Cchema_Id 
        , so.name as Base_Object_Name 
        , so.type_desc as Base_Object_Type
        , so.object_id as object_id 
        , so.schema_id as Schema_Id 
        , so.name 
        , so.type_desc 
        , 0 as Level 
        , convert ( nvarchar ( 1000 ) , N'/' + so.name ) as Obj_Path 
    from sys.objects so 
        left join sys.sql_expression_dependencies ed on ed.referenced_id = so.object_id 
        left join sys.objects rso on rso.object_id = ed.referencing_id 
    where rso.type is null 
        and so.type in ( 'P', 'V', 'IF', 'FN', 'TF' ) 

    union all 
    select   cp.Base_Object_Id as Base_Object_Id 
        , cp.Base_Cchema_Id 
        , cp.Base_Object_Name 
        , cp.Base_Object_Type
        , so.object_id as object_id 
        , so.schema_id as ID_Schema 
        , so.name 
        , so.type_desc 
        , Level + 1 as Level 
        , convert ( nvarchar ( 1000 ) , cp.Obj_Path + N'/' + so.name ) as Obj_Path 
    from sys.objects so 
        inner join sys.sql_expression_dependencies ed on ed.referenced_id = so.object_id 
        inner join sys.objects rso on rso.object_id = ed.referencing_id 
        inner join ObjectHierarchy as cp on rso.object_id = cp.object_id and rso.object_id <> so.object_id 
    where so.type in ( 'P', 'V', 'IF', 'FN', 'TF', 'U') 
        and ( rso.type is null or rso.type in ( 'P', 'V', 'IF', 'FN', 'TF', 'U' ) ) 
        and cp.Obj_Path not like '%/' + so.name + '/%' )   -- prevent cycles n hierarcy
select  
	SCHEMA_NAME ( Schema_Id ) + '.' + Name as SchemaTableName
	,Base_Object_Name 
    , Base_Object_Type
    , REPLICATE ( '   ' , Level ) + Name as Indented_Name 
    , Type_Desc as Object_Type 
    , Level 
    , Obj_Path 
from ObjectHierarchy as p 
WHERE  SCHEMA_NAME ( Schema_Id ) + '.' + Name  = @schemaAndObjectName
--order by Obj_Path;

UNION


SELECT
	CONCAT(r.referenced_schema_name,'.',r.referenced_entity_name) as SchemaTableName
	,r.referenced_entity_name
	,o.type_desc    
	,null
	,o.type_desc 
	,null
	,null
	--,r.*
FROM
    sys.dm_sql_referenced_entities(@schemaAndObjectName, 'OBJECT') r
	JOIN sys.objects AS o ON o.object_id = r.referenced_id

	--SELECT TYPE_NAME (207092274)

	-- sELECT *,TYPE_NAME(object_id) FROM  sys.objects AS o WHERE o.object_id = 207092274


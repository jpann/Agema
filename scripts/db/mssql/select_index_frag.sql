SELECT OBJECT_NAME(ind.OBJECT_ID) AS TableName
	,OBJECT_SCHEMA_NAME(ind.OBJECT_ID) AS SchemaName
	,ind.name AS IndexName
	,indexstats.index_type_desc AS IndexType
	,indexstats.avg_fragmentation_in_percent
	,CASE 
		WHEN indexstats.avg_fragmentation_in_percent > 30
			THEN CONCAT (
					'ALTER INDEX ['
					,ind.name
					,'] ON '
					,OBJECT_SCHEMA_NAME(ind.OBJECT_ID)
					,'.['
					,OBJECT_NAME(ind.OBJECT_ID)
					,'] REBUILD ;'
					)
		WHEN indexstats.avg_fragmentation_in_percent <= 30
			AND indexstats.avg_fragmentation_in_percent > 5
			THEN CONCAT (
					'ALTER INDEX ['
					,ind.name
					,'] ON '
					,OBJECT_SCHEMA_NAME(ind.OBJECT_ID)
					,'.['
					,OBJECT_NAME(ind.OBJECT_ID)
					,'] REORGANIZE ;'
					)
		ELSE NULL
		END CorrectiveStatement
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, NULL) indexstats
INNER JOIN sys.indexes ind ON ind.object_id = indexstats.object_id
	AND ind.index_id = indexstats.index_id
WHERE indexstats.avg_fragmentation_in_percent > 5
	AND ind.name IS NOT NULL
ORDER BY indexstats.avg_fragmentation_in_percent DESC


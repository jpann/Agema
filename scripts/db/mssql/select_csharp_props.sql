
SELECT 
CONCAT('public ',
CASE 
	WHEN DATA_TYPE IN ('nvarchar','varchar') THEN 'string'
	WHEN DATA_TYPE IN ('smallint') THEN 'short'
	WHEN DATA_TYPE IN ('bigint') THEN 'long'
	WHEN DATA_TYPE IN ('int') THEN 'int'
	WHEN DATA_TYPE IN ('smallint') THEN 'int'
	WHEN DATA_TYPE IN ('tinyint') THEN 'byte'	
	WHEN DATA_TYPE IN ('bit') THEN 'bool'
	WHEN DATA_TYPE IN ('numeric') THEN 'decimal'	
	WHEN DATA_TYPE IN ('datetime','datetime2') THEN 'DateTime'	
	ELSE 'string'
END
,' ',REPLACE(COLUMN_NAME,' ',''),' { get; set; }')
,*

FROM INFORMATION_SCHEMA.COLUMNS


SELECT 
CONCAT('public ',
CASE 
	WHEN DATA_TYPE IN ('nvarchar','varchar') THEN 'string'
	WHEN DATA_TYPE IN ('smallint') THEN 'short'
	WHEN DATA_TYPE IN ('int') THEN 'int'
	WHEN DATA_TYPE IN ('tinyint') THEN 'byte'
	
	ELSE 'string'
END
,' ',COLUMN_NAME,' { get; set; }')
,*

FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'v_WorkflowStep';

-- SELECT SDE tables in the database and create a drop statement
SELECT 
* 
,CONCAT('DROP TABLE ',TABLE_NAME,';') as sql
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME like 'SDE%' and TABLE_TYPE <> 'VIEW';


/*
Borrowed from Kendra Little: https://littlekendra.com/2016/01/28/how-to-check-if-an-index-exists-on-a-table-in-sql-server
*/
IF 1 = (SELECT COUNT(*) as index_count
    FROM sys.indexes AS si
    JOIN sys.objects AS so on si.object_id=so.object_id
    JOIN sys.schemas AS sc on so.schema_id=sc.schema_id
    WHERE 
        sc.name='dbo' /* Schema */
        AND so.name ='FirstNameByYear' /* Table */
        AND si.name='ix_halp' /* Index */)
PRINT 'it exists!'
ELSE PRINT 'nope';
GO

/* Or... */
IF 0 = (SELECT COUNT(*) as index_count
    FROM sys.indexes AS si
    JOIN sys.objects AS so on si.object_id=so.object_id
    JOIN sys.schemas AS sc on so.schema_id=sc.schema_id
    WHERE 
        sc.name='dbo' /* Schema */
        AND so.name ='ttttt' /* Table */
        AND si.name='xxxxx' /* Index */)
BEGIN
	CREATE NONCLUSTERED INDEX xxxxx 
	ON dbo.ttttt (BmpInstance_ID);
END
ELSE PRINT 'Index xxxxx already exists.';

GO



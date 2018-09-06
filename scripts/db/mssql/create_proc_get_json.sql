-- From https://stackoverflow.com/a/17882333/386619
CREATE PROCEDURE dbo.GetJSON @ObjectName VARCHAR(255), @registries_per_request smallint = null
AS
BEGIN
    IF OBJECT_ID(@ObjectName) IS NULL
        BEGIN
            SELECT Json = '';
            RETURN
        END;

    DECLARE @Top NVARCHAR(20) = CASE WHEN @registries_per_request IS NOT NULL 
                                    THEN 'TOP (' + CAST(@registries_per_request AS NVARCHAR) + ') ' 
                                    ELSE '' 
                                END;

    DECLARE @SQL NVARCHAR(MAX) = N'SELECT ' + @Top + '* INTO ##T ' + 
                                'FROM ' + @ObjectName;

    EXECUTE SP_EXECUTESQL @SQL;

    DECLARE @X NVARCHAR(MAX) = '[' + (SELECT * FROM ##T FOR XML PATH('')) + ']';


    SELECT  @X = REPLACE(@X, '<' + Name + '>', 
                    CASE WHEN ROW_NUMBER() OVER(ORDER BY Column_ID) = 1 THEN '{'
                         ELSE '' END + Name + ':'),
            @X = REPLACE(@X, '</' + Name + '>', ','),
            @X = REPLACE(@X, ',{', '}, {'),
            @X = REPLACE(@X, ',]', '}]')
    FROM    sys.columns
    WHERE   [Object_ID] = OBJECT_ID(@ObjectName)
    ORDER BY Column_ID;

    DROP TABLE ##T;

    SELECT  Json = @X;

END
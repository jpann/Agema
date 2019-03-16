-- =============================================
-- Author:		Jay Cummins (cumminsjp@gmail.com)
-- Create date: 2019-02-07
-- Description:	Splits a string and returns a table.  This
-- 				is a modified version of: https://sqlperformance.com/2012/07/t-sql-queries/split-strings
-- =============================================

IF object_id(N'dbo.SplitStrings', N'TF') IS NOT NULL
	BEGIN
		DROP FUNCTION dbo.SplitStrings;
		print 'dbo.SplitStrings dropped.';
	END
ELSE
	BEGIN
		print 'dbo.SplitStrings does not exist. Skipping.';
	END;
GO


CREATE FUNCTION dbo.SplitStrings
(
   @List       NVARCHAR(MAX),
   @Delimiter  NVARCHAR(255)
)
RETURNS @Items TABLE (Item NVARCHAR(4000))
WITH SCHEMABINDING
AS
BEGIN
   DECLARE @ll INT = LEN(@List) + 1, @ld INT = LEN(@Delimiter);
 
   WITH a AS
   (
       SELECT
           [start] = 1,
           [end]   = COALESCE(NULLIF(CHARINDEX(@Delimiter, 
                       @List, 1), 0), @ll),
           [value] = SUBSTRING(@List, 1, 
                     COALESCE(NULLIF(CHARINDEX(@Delimiter, 
                       @List, 1), 0), @ll) - 1)
       UNION ALL
       SELECT
           [start] = CONVERT(INT, [end]) + @ld,
           [end]   = COALESCE(NULLIF(CHARINDEX(@Delimiter, 
                       @List, [end] + @ld), 0), @ll),
           [value] = SUBSTRING(@List, [end] + @ld, 
                     COALESCE(NULLIF(CHARINDEX(@Delimiter, 
                       @List, [end] + @ld), 0), @ll)-[end]-@ld)
       FROM a
       WHERE [end] < @ll
   )
   INSERT @Items SELECT [value]
   FROM a
   WHERE LEN([value]) > 0
   OPTION (MAXRECURSION 0);
 
   RETURN;
END
GO

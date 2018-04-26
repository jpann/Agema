-- =============================================
-- Author:		Jay Cummins (cumminsjp@gmail.com)
-- Create date: 2018-04-20
-- Description:	 Creates a table between 2 integer values, similar to the PostgreSQL generate_series function (but with way less options!).
-- =============================================


IF EXISTS ( SELECT  1
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'dbo.generate_series')
                    AND type IN ( N'F',N'TF') 
		) 
	BEGIN
		DROP FUNCTION dbo.generate_series;
		Print 'Procedure dbo.generate_series dropped.';
	END
	ELSE
		Print 'Procedure dbo.generate_series does not exist.';


GO

Print 'Creating procedure dbo.generate_series.';
GO 

CREATE FUNCTION dbo.generate_series ( @p_start INT, @p_end INT)
RETURNS @Integers TABLE ( [IntValue] INT )
AS
BEGIN
    
  WITH interval(V) AS (
		SELECT @p_start
		UNION ALL
		SELECT V + 1 FROM interval
		WHERE V < @p_end
	)
	INSERT INTO @Integers
	SELECT * FROM interval;
    
    RETURN
END
GO  

--select * FROM generate_series(1,4)
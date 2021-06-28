 
DROP FUNCTION IF EXISTS [dbo].fn_ReverseCantorPair;
GO

CREATE FUNCTION [dbo].fn_ReverseCantorPair
(
	@z BIGINT
)
RETURNS @t TABLE (
	N1  BIGINT NOT NULL,
	N2  BIGINT NOT NULL)
AS
BEGIN	
	DECLARE @v BIGINT;


	SET @v =floor((-1.0 + sqrt(1.0 + 8.0 * @z))/2.0);

	
	INSERT INTO @t
	    SELECT  @v * (@v + 3) / 2 - @z  -- N1 Value
			,  @z - @v * (@v + 1) / 2 -- N2 value
    ;
	 
	RETURN;
END;
GO


SELECT * FROM dbo.fn_ReverseCantorPair(786885);
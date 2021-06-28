IF object_id(N'fn_createCantorPair', N'FN') IS NOT NULL
    DROP FUNCTION fn_createCantorPair;
GO


CREATE FUNCTION dbo.fn_createCantorPair
(
	@x bigint,
	@y bigint
)
RETURNS bigint
AS
BEGIN
	DECLARE @returnValue bigint; 
	SELECT @returnValue=((@x + @y) * (@x + @y + 1)) / 2 + @y;
	RETURN(@returnValue)
END
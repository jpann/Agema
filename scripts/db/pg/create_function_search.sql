-- =============================================
-- Author:		Jay Cummins (cumminsjp@gmail.com)
-- Create date: 2018-04-30
-- =============================================

DROP FUNCTION IF EXISTS public.search(text);

CREATE OR REPLACE FUNCTION public.search(
    IN p_search text
    )
  RETURNS TABLE(
	specific_schema text
	,routine_name text
	,routine_type text
	,return_type text
	,data_type text
	,ordinal_position integer
  ) AS
$BODY$
 
	SELECT 
		routines.specific_schema
		,routines.routine_name
		,routines.routine_type
		,routines.data_type as return_type
		, parameters.data_type
		, parameters.ordinal_position
		--,routines.*
	FROM information_schema.routines
	    LEFT JOIN information_schema.parameters ON routines.specific_name=parameters.specific_name
	WHERE routine_name ilike p_search;
	 
$BODY$
  LANGUAGE sql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION public.search(text)  OWNER TO postgres;



SELECT * FROM search('st_area');
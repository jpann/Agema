with inputs as (
	select 'sens_coll'::text as table_name
)
, cte as (
select
	column_name
	, case 
		when data_type = 'uuid' then 'Guid'
		when data_type = 'character varying' then 'string'
		when data_type = 'timestamp without time zone' then 'DateTime'
		when data_type = 'boolean' then 'bool'
		
	  else 'Object'
	  
	  end as csharpType
	, case
		when is_nullable::boolean = true and data_type <> 'character varying'  then '?' else '' end as nullablechar
--,*

from 
information_schema.columns
where	
	table_name = (select table_name from inputs)
--order by
--	ordinal_position
)
select '[Column("'||column_name||'")]'||E'\n'||'public '||csharptype||nullablechar||' '||compasskey_util.camel_caps(column_name)||' { get; set; }' from CTE;
 
 


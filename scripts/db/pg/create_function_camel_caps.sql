CREATE schema if not exists compasskey_util AUTHORIZATION postgres;

// TODO: add a jsonb parameter to handle special cases, including the one that is currently hard-coded
CREATE OR REPLACE FUNCTION compasskey_util.camel_caps(p_text text)
  RETURNS  Text  AS 
$BODY$

with word_list as(
	select case when t = 'ts' then 'On'
	else initcap(t)
	end as word
	from 
	unnest( 
		(select string_to_array(p_text,'_'))
	) t	  
)
select string_agg(word,''::text) 
from word_list;

$BODY$
  LANGUAGE sql VOLATILE
  COST 100;
 
 
 
 
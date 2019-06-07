

SELECT --id_0, id, pk, statefp10, countyfp10, tractce10, blockce, blockid10, partflg, housing10, pop10, pop_den, acreage
 	 SUM("POP10") as tract_sum_pop10
	 
	  ,sum(case when "POP10" is null then 0 else 1 end) as tract_count_pop10_notnull
      ,sum(case when "POP10" is null then 1 else 0 end) as tract_count_pop10_null
       
	 ,SUM("HOUSING10") as tract_sum_housing
 	 ,COUNT("HOUSING10") as tract_count_housing
 	 
	  ,sum(case when "HOUSING10" is null then 0 else 1 end) as tract_count_housing_notnull
      ,sum(case when "HOUSING10" is null then 1 else 0 end) as tract_count_housing_null
      
      ,array_agg("BLOCKID10") as block_ids
      
 	 ,"TRACTCE10" as tractce10
 	 ,st_union(geom) as geom
 	 into us_census.tract2010_pophu_usa 
FROM us_census.tabblock2010_pophu_usa 
group by tractce10;


-- Drop table

-- DROP TABLE us_census.tabblock2010_pophu_usa

ALTER TABLE us_census.tract2010_pophu_usa add column id serial;  
ALTER TABLE us_census.tract2010_pophu_usa ADD CONSTRAINT pk_tract2010_pophu_usa PRIMARY KEY (id);
CREATE INDEX sidx_tract2010_pophu_usa ON us_census.tract2010_pophu_usa USING gist (geom);
CREATE UNIQUE INDEX tract2010_pophu_usa_tractce10_idx ON us_census.tract2010_pophu_usa (tractce10,id);














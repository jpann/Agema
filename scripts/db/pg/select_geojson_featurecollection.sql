-- Queries a list of features as a geojson feature collection
SELECT row_to_json(fc)FROM (
        SELECT 'FeatureCollection' AS type
                ,array_to_json(array_agg(f)) AS features
        FROM (
                SELECT 'Feature' AS type
            /* Change this to name of the actual geometry column.  Geom for native PostGIS data. Shape if it came from Esri.
            You can also st_transform to a another coordinate system (e.g ST_AsGeoJSON(ST_Transform(lg.shape,4326))::json AS geometry)
             */
                        ,ST_AsGeoJSON(lg.geom)::json AS geometry  
                        ,row_to_json((
                                        SELECT l
                                        FROM (
                        -- Put Attribute select here (but don't put the geometry field)
                                                SELECT id,  csa2010, shomes10, shomes11, shomes12, shomes13, shomes14, 
       salepr10, salepr11, salepr12, salepr13, salepr14, dom10, dom11, 
       dom12, dom13, dom14, reosa11, reosa12, reosa13, reosa14, cashsa11, 
       cashsa12, cashsa13, cashsa14, fore10, fore11, fore12, fore13, 
       fore14, ownroc10, ownroc11, ownroc12, ownroc13, ownroc14, nomail10, 
       nomail11, nomail12, nomail13, nomail14, totalres10, totalres11, 
       totalres12, totalres13, totalres14, affordm14, affordr14, homtax11, 
       homtax12, homtax13, homtax14, owntax11, owntax12, owntax13, owntax14, 
       resrehab10, resrehab11, resrehab12, resrehab13, resrehab14, constper11, 
       constper12, constper13, constper14, vacant10, vacant11, vacant12, 
       vacant13, vacant14, baltvac11, baltvac12, baltvac13, vio10, vio11, 
       vio12, vio13, demper11, demper12, demper13, demper14, hcvhouse14
                                                 
                                                ) AS l
                                        )) AS properties
                FROM public.vs14_housing  AS lg  /* <-- You need to specify your source table (or view)  */
                limit 5  /* Put a where clause or limit here to filter the query */
                ) AS f
 
        ) AS fc;


/*
Note: If you are testing your query in PGADMIN, your results may be too big to copy/paste Change your Max. characters per column to -1.
PGADMIN may still display blank data, but you can copy/paste the GeoJSON to a text editor.
*/

     
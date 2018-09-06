
CREATE OR REPLACE FUNCTION nrand(p_number_of_random_distances integer, p_radius numeric, p_geom geometry)
RETURNS geometry AS $$
 DECLARE 
   tmp_dist numeric;
   max_dist numeric;
 BEGIN
   max_dist := 0;
   FOR i IN 1..p_number_of_random_distances
   LOOP
     tmp_dist := random()*p_radius;
  IF max_dist < tmp_dist THEN
    max_dist := tmp_dist;
  END IF;
   END LOOP;
   RETURN ST_Project(p_geom, max_dist, radians(random()*360));
 END;
$$
LANGUAGE plpgsql;

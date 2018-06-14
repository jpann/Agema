
Select *
from openquery(spatialite , 'select pk,st_area(geom) FROM EPSG_Polygons ');
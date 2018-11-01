

REM curl -k -o C:\tmp\uscensus_population_housingtabblock2010_04_pophu_Arizona.zip     https://www2.census.gov/geo/tiger/TIGER2010BLKPOPHU/tabblock2010_04_pophu.zip

REM %1 = FIPS Code
REM %2 = StateName


curl -k -o "C:\tmp\uscensus_population_housing\tabblock2010_%~1_pophu_%~2.zip"     https://www2.census.gov/geo/tiger/TIGER2010BLKPOPHU/tabblock2010_%1_pophu.zip

7za x -oC:\tmp\uscensus_population_housing "C:\tmp\uscensus_population_housing\tabblock2010_%~1_pophu_%~2.zip" 

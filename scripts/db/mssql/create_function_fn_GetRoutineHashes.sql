CREATE function dbo.fn_GetRoutineHashes()
RETURNS @t TABLE 
	(
		SPECIFIC_SCHEMA nvarchar(256),
		SPECIFIC_NAME nvarchar(256),
		SPECIFIC_CATALOG nvarchar(256),
		FULL_OBJECT_NAME nvarchar(256),
		ROUTINE_HASH varchar(max)
	) 
AS BEGIN
		DECLARE @objectName nvarchar(200)
		DECLARE @specificName nvarchar(200)
		DECLARE @specificSchema nvarchar(200)
		DECLARE @databaseName nvarchar(200)
		declare @spdefinition nvarchar(4000) 
		declare @hashedVal varbinary(4000) 
		DECLARE cur CURSOR LOCAL FAST_FORWARD FOR select  CONCAT(specific_schema ,'.',specific_name),SPECIFIC_CATALOG as databaseName, SPECIFIC_NAME, SPECIFIC_SCHEMA from information_schema.routines;

		OPEN cur

		FETCH NEXT FROM cur INTO @objectName, @databaseName, @specificName, @specificSchema
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @spdefinition = (SELECT OBJECT_DEFINITION (OBJECT_ID(@objectName)))     
			set @hashedVal = (select HashBytes('SHA1', @spdefinition)) 
			/**Here's the hashed value of the stored procedure**********/
			-- print CONVERT(varchar(max),@hashedVal ,2) 

			INSERT INTO @t VALUES (@specificSchema,@specificName,@databaseName,@objectName,CONVERT(varchar(max),@hashedVal ,2));


		
			
			FETCH NEXT FROM cur INTO @objectName,  @databaseName, @specificName, @specificSchema
		END

		CLOSE cur
		DEALLOCATE cur
 
 

RETURN;

END



GO


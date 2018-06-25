BEGIN 
	DECLARE @msg varchar(255);
	-- DECLARE @routineName sysname = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))+'.'+QUOTENAME(OBJECT_NAME(@@PROCID));

	BEGIN TRY	
		-- PRINT CONCAT('@routineName=',@routineName);
	

		SET @msg = CONCAT('Example',' Error');

		--RAISERROR (@msg,
		--	16, -- Severity.
		--	1 -- State.
		--	);

			/*
			THROW [ { error_number | @local_variable },  
					{ message | @local_variable },  
					{ state | @local_variable } ]   
			[ ; ]  
			*/
			THROW 51000, 'Throwing an example exception', 1;  


			--RETURN 0;
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT 
			ERROR_NUMBER() AS ErrorNumber,
			ERROR_SEVERITY() AS ErrorSeverity,
			ERROR_STATE() as ErrorState,
			ERROR_PROCEDURE() as ErrorProcedure,
			ERROR_LINE() as ErrorLine,
			ERROR_MESSAGE() as ErrorMessage; 

	SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();
	-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH;
END;
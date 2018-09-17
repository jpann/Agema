
:: Note: there is no recyling bin.  This is a permanent delete.
 
:: use a variable for the log file path
SET TLOGFILEPATH=C:\Windows\Temp
 
:: make a folder for temporarily holding the files to delete
mkdir %TLOGFILEPATH%\delfolder
 
:: robocopy anything older than 30 days (/MINAGE) and use /MOV to 
:: to purge it from the source directory after the file is copied to 
:: the destination
robocopy %TLOGFILEPATH% %TLOGFILEPATH%\delfolder /S /MOV /MINAGE:30 /R:1 /W:1

REM robocopy w/ same src and dest with a /S /MOVE will clear the empty directories 
REM and leave directories that have files
:: robocopy %TLOGFILEPATH% %TLOGFILEPATH% /S /MOVE
 
 
:: remove the entire temporary folder that is holding the old files
:: comment out this line if you wish to inspect the files before deleting the folder.
::rd /s /q %TLOGFILEPATH%\delfolder
 
:: comment out the next line, if you don't want the DOS window to remain open.
:: pause

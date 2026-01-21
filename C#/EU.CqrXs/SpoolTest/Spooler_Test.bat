@echo off 

SET BatchStartDir=%cd%
Set KEYS=%cd%\..\..\..\docu\keys.txt 

rmdir %cd%\Spooler\In
rmdir %cd%\Spooler\Out
del /sex %cd%\Spooler
rmdir %cd%\Spooler


if exist %cd%\..\..\..\docu\keys.txt (	
	mkdir %cd%\Spooler	
	mkdir %cd%\Spooler\In
	mkdir %cd%\Spooler\Out
	copy %cd%\..\..\..\docu\keys.txt %cd%\Spooler\keys.txt /Y
	rem cd %cd%\..\..\..\docu\
	REM PUSHD .\..\..\..\docu
	SET SpoolerIn=%cd%\Spooler\In\
	SET KEYS=%cd%\Spooler\keys.txt
	echo %SpoolerIn%
	copy %cd%\..\..\..\docu\*.* %cd%\Spooler\In\. /Y
	Set SpoolerOut=%cd%\Spooler\Out\
	REM POPD
) else (
	echo %cd%\..\..\..\docu\keys.txt doen't exist"
)

cd %BatchStartDir%

echo "Starting EU.CqrXs.Spooler.exe  -i=%SpoolerIn% -o=%SpoolerOut% -r -k=%KEYS%"

EU.CqrXs.SpoolTest.exe -i=%SpoolerIn% -o=%SpoolerOut% -k=%KEYS%

pause

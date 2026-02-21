@echo off 

echo "deleting old files in Encrypt\* Out\*"
del /s /q .\Encrypt\*.*
del /s /q .\Out\*.*

:initial
if "%1"=="-S" goto secpipe
if "%1"=="-s" goto secpipe

echo "EU.CqrXs.Spooler.exe -V -k=bar@ba.area23.at -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\In\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\
EU.CqrXs.Spooler.exe -V -k=bar@ba.area23.at -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\In\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\ 
echo "EU.CqrXs.Spooler.exe -V -D -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Out\ "
EU.CqrXs.Spooler.exe -V -D -k=bar@ba.area23.at -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Out\ 
goto end


:secpipe
EU.CqrXs.Spooler.exe -V -S -k=jo@io.cqrxs.eu -i=.\In\ -o=.\Encrypt\ 
echo "Decompressing from directory!"
EU.CqrXs.Spooler.exe -D -V -S -k=jo@io.cqrxs.eu -i=.\Encrypt\ -o=.\Out\ 
goto end


:end



REM exit

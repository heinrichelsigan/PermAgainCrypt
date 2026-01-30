@echo off 

echo "EU.CqrXs.Spooler.exe -V -k=bar@ba.area23.at -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\In\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\
EU.CqrXs.Spooler.exe -V -k=bar@ba.area23.at -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\In\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\ 
echo "EU.CqrXs.Spooler.exe -V -D -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Out\ "
EU.CqrXs.Spooler.exe -V -D -k=bar@ba.area23.at -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Out\ 
del /s /v /q S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\*
pause
del /s /v /q S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Out\*


EU.CqrXs.Spooler.exe -V -S -k=jo@io.cqrxs.eu -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\In\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\ 
echo "Decompressing from directory!"
EU.CqrXs.Spooler.exe -D -V -S -k=jo@io.cqrxs.eu -i=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\ -o=S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Out\ 
del /s /v /q S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Encrypt\*
pause
del /s /v /q S:\PermAgainCrypt\Deploy\EU.CqrXs\EU.CqrXs.Spooler\Out\*

REM exit

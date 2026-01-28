@echo off 

echo "EU.CqrXs.SpoolTest.exe -V -i=S:\PermAgainCrypt\Deploy\SpoolTest\In\ -o=S:\PermAgainCrypt\Deploy\SpoolTest\Encrypt\ -k=S:\PermAgainCrypt\Deploy\SpoolTest\In\keys.txt"
EU.CqrXs.SpoolTest.exe -V -i=S:\PermAgainCrypt\Deploy\SpoolTest\In\ -o=S:\PermAgainCrypt\Deploy\SpoolTest\Encrypt\ -k=S:\PermAgainCrypt\Deploy\SpoolTest\In\keys.txt
echo "EU.CqrXs.SpoolTest.exe -V -D -i=S:\PermAgainCrypt\Deploy\SpoolTest\Encrypt\ -o=S:\PermAgainCrypt\Deploy\SpoolTest\Out\ -k=S:\PermAgainCrypt\Deploy\SpoolTest\In\keys.txt"
EU.CqrXs.SpoolTest.exe -V -D -i=S:\PermAgainCrypt\Deploy\SpoolTest\Encrypt\ -o=S:\PermAgainCrypt\Deploy\SpoolTest\Out\ -k=S:\PermAgainCrypt\Deploy\SpoolTest\In\keys.txt
del /s /v /q S:\PermAgainCrypt\Deploy\SpoolTest\Encrypt\*
pause
del /s /v /q S:\PermAgainCrypt\Deploy\SpoolTest\Out\*


EU.CqrXs.SpoolTest.exe -V -S -i=S:\PermAgainCrypt\Deploy\SpoolTest\In\ -o=S:\PermAgainCrypt\Deploy\SpoolTest\Encrypt\ -k=S:\PermAgainCrypt\Deploy\SpoolTest\In\keys.txt
echo "Decompressing from directory!"
EU.CqrXs.SpoolTest.exe -D -V -S -i=S:\PermAgainCrypt\Deploy\SpoolTest\Encrypt\ -o=S:\PermAgainCrypt\Deploy\SpoolTest\Out\ -k=S:\PermAgainCrypt\Deploy\SpoolTest\In\keys.txt
del /s /v /q S:\PermAgainCrypt\Deploy\SpoolTest\Encrypt\*
pause
del /s /v /q S:\PermAgainCrypt\Deploy\SpoolTest\Out\*

REM exit

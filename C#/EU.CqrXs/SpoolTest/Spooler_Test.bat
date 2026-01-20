@echo off

echo Starting Spooler EU.CqrXs.Spooler.exe tests
mkdir -p H:\Spooler\
mkdir -p H:\Spooler\InFiles\
mkdir -p H:\Spooler\OutFiles\
copy /s /e /v S:\PermAgainCrypt\docu\*.* H:\Spooler\InFiles\.
copy S:\PermAgainCrypt\docu\keys.txt H:\Spooler\keys.txt

EU.CqrXs.SpoolTest.exe -i=H:\Spooler\InFiles\ -o=H:\Spooler\OutFiles\ -r -k=H:\Spooler\keys.txt


pause

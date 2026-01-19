@echo off

echo Staring console EU.CqrXs.Console.exe tests
echo deleting README_MD.base16 README.MD.gz.BfF.base64 README.MD.Whirlpool.bz.Base32 README.MD.SCrypt.zip.uu README.MD.BCrypt.zip.xx READ_MD.txt READ_GUNZIP.txt READ_UNZIP.txt READ_BUNZIP.txt README_SYM_BCRYPT_UNZIP.txt

del /q README_MD.base16 README.MD.gz.BfF.base64 README.MD.Whirlpool.bz.Base32 README.MD.SCrypt.zip.uu README.MD.BCrypt.zip.xx READ_MD.txt READ_GUNZIP.txt READ_UNZIP.txt READ_BUNZIP.txt README_SYM_BCRYPT_UNZIP.txt
@echo on

EU.CqrXs.Console.exe -i=.\README.MD -e=base16 -o=.\README_MD.base16
EU.CqrXs.Console.exe -i=.\README_MD.base16 -d=base16 -o=.\READ_MD.txt

EU.CqrXs.Console.exe -i=.\README.MD -z=gzip  -c=BlowFish,Fish2,Fish3 -p=Hallo -e=base64 -o=.\README.MD.gz.BfF.base64
EU.CqrXs.Console.exe -i=.\README.MD.gz.BfF.base64 -d=base64 -D=BlowFish,Fish2,Fish3 -p=Hallo -u=gzip -o=.\READ_GUNZIP.txt

EU.CqrXs.Console.exe -i=.\README.MD -z=bz -k=heinrichelsigan.area23.at -H=Whirlpool -e=hex32 -o=.\README.MD.Whirlpool.bz.Hex32
EU.CqrXs.Console.exe -i=.\README.MD.Whirlpool.bz.Hex32 -d=hex32 -q=heinrichelsigan.area23.at -H=Whirlpool -u=bz -o=.\READ_BUNZIP.txt

EU.CqrXs.Console.exe -i=.\README.MD -z=zip -k=io.cqrxs.eu -H=SCrypt -e=uu -o=.\README.MD.SCrypt.zip.uu
EU.CqrXs.Console.exe -i=.\README.MD.SCrypt.zip.uu -d=uu -q=io.cqrxs.eu -H=SCrypt -u=zip -o=.\READ_UNZIP.txt

EU.CqrXs.Console.exe -i=.\README.MD -S -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\README.MD.BCrypt.zip.xx
EU.CqrXs.Console.exe -i=.\README.MD.BCrypt.zip.xx -S -d=xx -q=io.cqrxs.eu -H=BCrypt -u=zip -o=.\README_SYM_BCRYPT_UNZIP.txt

start notepad READ_MD.txt
start notepad READ_GUNZIP.txt
start notepad READ_BUNZIP.txt
start notepad READ_UNZIP.txt
start notepad README_SYM_BCRYPT_UNZIP.txt

echo finished, waiting 30 seconds to close
timeout 30 > NUL
REM pause

@echo off

echo Staring console EU.CqrXs.Console.Core.exe tests
echo deleting README.MD.BlowFish.Fish2.Fish3.base64 README.MD.Whirlpool.bz.Base32 README.MD.SCrypt.zip.uu README_UNZIP.txt README_GUNZIP.txt README_BUNZIP.txt README.MD.BCrypt.zip.xx README_SYM_BCRYPT_UNZIP.txt

del /q README.MD.BlowFish.Fish2.Fish3.base64 README.MD.Whirlpool.bz.Base32 README.MD.SCrypt.zip.uu README_UNZIP.txt README_GUNZIP.txt README_BUNZIP.txt README.MD.BCrypt.zip.xx README_SYM_BCRYPT_UNZIP.txt
@echo on
EU.CqrXs.Console.Core.exe -i=.\README.MD -z=gzip  -c=BlowFish,Fish2,Fish3 -p=Hallo -e=base64 -o=.\README.MD.BlowFish.Fish2.Fish3.base64
EU.CqrXs.Console.Core.exe -i=.\README.MD.BlowFish.Fish2.Fish3.base64  -d=base64 -D=BlowFish,Fish2,Fish3 -p=Hallo -u=gzip -o=.\README_GUNZIP.txt

EU.CqrXs.Console.Core.exe -i=.\README.MD -z=bz -k=heinrichelsigan.area23.at -H=Whirlpool -e=hex32 -o=.\README.MD.Whirlpool.bz.Hex32
EU.CqrXs.Console.Core.exe -i=.\README.MD.Whirlpool.bz.Hex32 -d=hex32 -q=heinrichelsigan.area23.at -H=Whirlpool -u=bz -o=.\README_BUNZIP.txt

EU.CqrXs.Console.Core.exe -i=.\README.MD -z=zip -k=io.cqrxs.eu -H=SCrypt -e=uu -o=.\README.MD.SCrypt.zip.uu
EU.CqrXs.Console.Core.exe -i=.\README.MD.SCrypt.zip.uu -d=uu -q=io.cqrxs.eu -H=SCrypt -u=zip -o=.\README_UNZIP.txt

EU.CqrXs.Console.Core.exe -i=.\README.MD -S -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\README.MD.BCrypt.zip.xx
EU.CqrXs.Console.Core.exe -i=.\README.MD.BCrypt.zip.xx -S -d=xx -q=io.cqrxs.eu -H=BCrypt -u=zip -o=.\README_SYM_BCRYPT_UNZIP.txt

start notepad README_UNZIP.txt
echo Finished

pause
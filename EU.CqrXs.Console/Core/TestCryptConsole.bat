@echo off

del README.MD.BlowFish.Fish2.Fish3.base64 README_MD.txt
mkdir log
EU.CqrXs.Console.Core.exe -i=.\README.MD -z=gzip  -c=BlowFish,Fish2,Fish3 -k=Hallo -e=base64 -o=.\README.MD.BlowFish.Fish2.Fish3.base64
EU.CqrXs.Console.Core.exe -i=.\README.MD.BlowFish.Fish2.Fish3.base64  -d=base64 -D=BlowFish,Fish2,Fish3 -k=Hallo -u=gzip -o=.\README_MD.txt

pause
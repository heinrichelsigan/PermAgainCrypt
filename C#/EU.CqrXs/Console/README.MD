https://cqrxs.eu/download/EU.CqrXs.Console/README.MD

S:\>EU.CqrXs.Console.exe -?
Usage:  EU.CqrXs.Console.exe
    -i | --inFile= | --inText={string|EnviromentVariable} | --inStd
    -o | --outFile= | --outText=EnviromentVariable | --outStd
    -u | --unzip={gzip|bzip2}
    -z | --zip={gzip|bzip2}
    -d | --decode={raw|hex16|hex32|base32|base64|uu}
    -e | --encode={raw|hex16|hex32|base32|base64|uu}
    -c | --crypt={algo1,algo2,...}
         algo:
            Aes,AesLight,Rijndael,Des,Des3,Dstu7624,
            Aria,Camellia,CamelliaLight,Cast5,Cast6,
            BlowFish,Fish2,Fish3,
            Gost28147,Idea,Noekeon,
            RC2,RC532,RC564,RC6,
            Seed,SkipJack,Serpent,SM4,
            Tea,Tnepres,XTea,
            ZenMatrix,ZenMatrix2
        symmAlgo: 
            Aes,BlowFish,Camellia,Cast6,Des3,Fish2,Fish3,Gost28147,Idea,RC532,Seed,SkipJack,Serpent,Tea,XTea,SM4        
      -p --pass=Passphrase
    -D | --decrypt=={algo1,algo2,...}
      -p --pass=Passphrase    
    -k | --key=passKey encrypt
    -q | --qey=passKey decrypt
    -h | --hash={Oct|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|TupleHash}
    -S | --SymmCipher 
    -? | --gethelp

Examples:
    EU.CqrXs.Console.exe -i=test.jpg -z=bzip2 -e=base32 -o=test.jpg.bz2.base32
    EU.CqrXs.Console.exe -i=test.jpg.bz2.base32 -d=base32 -u=bzip2 -o=test1.jpg
        
    EU.CqrXs.Console.exe --inFile=test.jpg --zip=gzip --crypt=AesLight,Fish3 -k=MySecretKey -e=base64 -o=test.jpg.gz.aeslight.fish3.base64
    EU.CqrXs.Console.exe -i=test.jpg.gz.aeslight.fish3.base64 -d=base64  -D=AesLight,Fish3 -k=MySecretKey -e=base64  --unzip=gzip  -o=test2.jpg
        
    EU.CqrXs.Console.exe -i=.\README.MD -z=zip -k=io.cqrxs.eu -H=SCrypt -e=uu -o=.\README.MD.SCrypt.zip.uu
    EU.CqrXs.Console.exe -i=.\README.MD.SCrypt.zip.uu -d=uu -q=io.cqrxs.eu -H=SCrypt -u=zip -o=.\README_UNZIP.txt
        
    EU.CqrXs.Console.exe -i=.\README.MD -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\README.MD.BCrypt.zip.xx
    EU.CqrXs.Console.exe -i=.\README.MD.BCrypt.zip.xx -d=xx -q=io.cqrxs.eu -H=BCrypt -u=zip -o=.\README_SYM_BCRYPT_UNZIP.txt


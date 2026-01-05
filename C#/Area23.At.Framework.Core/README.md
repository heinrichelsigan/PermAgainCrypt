# 8-staged symmetric block cipher pipeline 

An eight staged symmetric block cipher crypto pipeline to improve advanced encryption standard based on meta DES, 3DES with P-Box S-Box.

The following image shows you an example of a symmetric cipher **8 staged encryption pipe** and the corresponding **decryption** _inverse_ **pipe**.

<img src="https://raw.githubusercontent.com/heinrichelsigan/PermAgainCrypt/refs/heads/main/docu/Symmetric_Cipher_Pipeline.png" />

Before entering the encryption pipe, the file can be zipped to avoid huge amount of symmetric cipher blocks 
and after exiting the encryption pipe the file can be ascii encoded with base64 mime, uuencode, xxencode or hex16, because symmetric chiphered binary files might lose their block padding.

Implementation is based on my blog article: [Making symmetric cipher encryption meta permutating again](https://area23-at.blogspot.com/2024/04/making-symetric-chiffer-encryption.html),
including the follwing symmetric cipher algorithms:
- [Aes](https://en.wikipedia.org/wiki/Advanced_Encryption_Standard), AesLight, AesNet, [Rijndael](https://en.wikipedia.org/wiki/Advanced_Encryption_Standard)
- [Bruce Schneier's](https://www.schneier.com/) BlowFish, 2-Fish, 3-Fish, 3-Fish-256	
- [Camellia](https://en.wikipedia.org/wiki/Camellia_(cipher)), CamelliaLight
- [Cast5](https://en.wikipedia.org/wiki/CAST-128), Cast6
- [National security agency's](https://nsa.gov) Des, 3-Des, Triple-Des.Net,  [SkipJack](https://en.wikipedia.org/wiki/Skipjack_(cipher))
- [Dstu7624](https://en.wikipedia.org/wiki/Kupyna)
- [Gost28147](https://en.wikipedia.org/wiki/GOST_(block_cipher)), [Idea](https://en.wikipedia.org/wiki/International_Data_Encryption_Algorithm), [Noekeon](https://en.wikipedia.org/wiki/NOEKEON)
- [RC2](https://de.wikipedia.org/wiki/RC2_(Blockchiffre)), [RC5](https://en.wikipedia.org/wiki/RC5)32, RC564, [RC6](https://en.wikipedia.org/wiki/RC6)
- [Seed](https://en.wikipedia.org/wiki/SEED), [SM4](https://en.wikipedia.org/wiki/SM4_(cipher))
- [Serpent](https://en.wikipedia.org/wiki/Serpent_(cipher)), Tnepres
- [Tea](https://de.wikipedia.org/wiki/Tiny_Encryption_Algorithm), [XTea](https://en.wikipedia.org/wiki/XTEA)
- and my own simplest below average symmetric block cipher alogrithms: ZenMatrix, ZenMatrix2	

## C# solution and all projects ported to .NetCore 10
You need since 2025-11-15 Visual Studio 2026 and C# .NetCore 10.0 to compile C# csproj.

## Download

### Releases
https://github.com/heinrichelsigan/PermAgainCrypt/releases/

### [cqrxs.eu/download](https://cqrxs.eu/download/)
- [PermAgainCrypt_WinForm_NetCore10_x86+x64](https://cqrxs.eu/download/PermAgainCrypt_WinForm_NetCore10_x64.7z)
- Rendundant website is [io.cqrxs.eu/download](https://io.cqrxs.eu/download/)

## WebForm online test:
- [area23.at/net/Crypt/CoolCrypt.aspx](https://area23.at/net/Crypt/CoolCrypt.aspx)
- [cqrxs.eu/net/Crypt/CoolCrypt.aspx](https://cqrxs.eu/net/Crypt/CoolCrypt.aspx)

## Videos
- walk through: https://youtu.be/GX9q1sRx3nE
- https://youtu.be/5J3R1gg-jjA
- https://youtu.be/xpnnrxc2znA
- https://youtu.be/tY2DPsZjbVQ

## Credits
- Great Thanks to [the Legion of the Bouncy Castle](https://www.bouncycastle.org/) 
- Github: [github.com/bcgit](https://github.com/bcgit) 

## WinFormCore:
<img width="800" height="726" alt="image" src="https://raw.githubusercontent.com/heinrichelsigan/PermAgainCrypt/refs/heads/main/docu/PermAgainCrypt_WinForm.gif" />

## Console Application

<img width="1132" height="818" alt="image" src="https://github.com/user-attachments/assets/bdda016b-5daa-436a-a9f0-4f981e54b688" />

Console application let you execute cipherpipe as standard console programm. 
options can be set by argument parameters.

```
U:\source\PermAgainCrypt\Deploy\console\x86>EU.CqrXs.Console.Core.exe -?
Usage:  EU.CqrXs.Console.Core.exe
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
    -h | --hash={Blake2xs|BCrypt|CShake|Dstu7564|MD5|Oct|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|TupleHash|TupleHash|Whirlpool}
    -S | --SymmCipher
    -? | --gethelp

Examples:
        EU.CqrXs.Console.Core.exe -i=test.jpg -z=bzip2 -e=base32 -o=test.jpg.bz2.base32
        EU.CqrXs.Console.Core.exe -i=test.jpg.bz2.base32 -d=base32 -u=bzip2 -o=test1.jpg

        EU.CqrXs.Console.Core.exe --inFile=test.jpg --zip=gzip --crypt=AesLight,Fish3 -k=MySecretKey -e=base64 -o=test.jpg.gz.aeslight.fish3.base64
        EU.CqrXs.Console.Core.exe -i=test.jpg.gz.aeslight.fish3.base64 -d=base64  -D=AesLight,Fish3 -k=MySecretKey -e=base64  --unzip=gzip  -o=test2.jpg

        EU.CqrXs.Console.Core.exe -i=README.MD -z=zip -k=io.cqrxs.eu -H=SCrypt -e=uu -o=README.MD.SCrypt.zip.uu
        EU.CqrXs.Console.Core.exe -i=README.MD.SCrypt.zip.uu -d=uu -q=io.cqrxs.eu -H=SCrypt -u=zip -o=README_UNZIP.txt

U:\source\PermAgainCrypt\Deploy\console\x86>
```

# [Java](https://github.com/heinrichelsigan/PermAgainCrypt/tree/main/java)

<img src="https://github.com/heinrichelsigan/PermAgainCrypt/blob/main/docu/2025-12-28_javaProtorype.gif?raw=true" />

# [Android](https://github.com/heinrichelsigan/PermAgainCrypt/tree/main/android)



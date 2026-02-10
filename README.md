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

## What are advantages and disadvantages of Symmetric Block Cipher

### Advantages
Since symmetric block cipher ciphers each block in the same encrypting way
<img width="1021" height="443" alt="image" src="https://github.com/user-attachments/assets/27210f93-31d8-4002-851e-95748c5ad4d9" />

parallel processing can be implemented quiet easy with average performance bust on huge multiprocessore machines.
<img width="1065" height="727" alt="image" src="https://github.com/user-attachments/assets/bddbc429-7d1d-4c4e-a55b-40c3b0cbf50b" />


### Disadvantages

<img width="1049" height="709" alt="image" src="https://github.com/user-attachments/assets/90052c90-0fe1-477d-a505-5f92fb7506d5" />


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
- LibTom: [github.com/libtom/libtomcrypt](https://github.com/libtom/libtomcrypt) [libtom.net/](https://www.libtom.net/)

## WinFormCore:
<img width="800" height="726" alt="image" src="https://raw.githubusercontent.com/heinrichelsigan/PermAgainCrypt/refs/heads/main/docu/PermAgainCrypt_WinForm.gif" />

## Console Application

![2026-01-27_EU_CqrXs_Console](https://github.com/user-attachments/assets/e5b885c3-971b-43d3-b16e-8c610cea1ba4)

Console application let you execute cipherpipe as standard console programm. 
options can be set by argument parameters.

```
U:\source\PermAgainCrypt\Deploy\console\x86>EU.CqrXs.Console.exe
Usage:  EU.CqrXs.Console.exe
    -i  ├─ --inFile= | --inText={string|EnviromentVariable} | --inStd
        |
    -k  ├─ --key=passKey encrypt
    -H  ├─ --Hash={Blake2xs|BCrypt|CShake|Dstu7564|Hey|MD5|Oct|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|TupleHash}
        |      default: Hex
    -z  ├─ --zip={gzip|bzip2|zip}
        |     default: none
    -C  ├─ --CipherAlgost={algo1,algo2,...}
        | └ algo:
        │     Aes,AesLight,Rijndael,Des,Des3,Dstu7624,
        │       Aria,Camellia,CamelliaLight,Cast5,Cast6,
        │       BlowFish,Fish2,Fish3,
        │       Gost28147,Idea,Noekeon,
        │       RC2,RC532,RC564,RC6,
        │       Seed,SkipJack,Serpent,SM4,
        │       Tea,Tnepres,XTea,
        │       ZenMatrix,ZenMatrix2
        │   symmAlgo:
        │        Aes,BlowFish,Camellia,Cast6,Des3,Fish2,Fish3,Gost28147,Idea,RC532,Seed,SkipJack,Serpent,Tea,XTea,SM4
    -S  ├─ --SymmCipher
    -e  ├─ --encode={raw|hex16|hex32|base32|base64|uu}
        |   default: base64
    -D  ├─ --Decrypt [ = Inverse_Pipe_Direction ]
        |
    -o  ├─ --outFile= | --outText=EnviromentVariable | --outStd
        |
    -V  ├─ --verbose
    -?  ├─ --gethelp

Examples:

    EU.CqrXs.Console.exe -V -i=.\README.MD -e=base16 -o=.\README_MD.base16
    EU.CqrXs.Console.exe -V -D  -i=.\README_MD.base16 -e=base16 -o=.\READ_MD.txt

    EU.CqrXs.Console.exe -V -i=.\README.MD -k=Hallo -z=gzip  -C=BlowFish,Fish2,Fish3 -e=base64 -o=.\README.MD.gz.BfF.base64
    EU.CqrXs.Console.exe -V -D -i=.\README.MD.gz.BfF.base64 -e=base64 -C=BlowFish,Fish2,Fish3 -p=Hallo -z=gzip -o=.\READ_GUNZIP.txt

    EU.CqrXs.Console.exe -V -i=.\README.MD -z=bz -k=heinrichelsigan.area23.at -H=Whirlpool -e=hex32 -o=.\README.MD.Whirlpool.bz.Hex32
    EU.CqrXs.Console.exe -V -D -i=.\README.MD.Whirlpool.bz.Hex32 -e=hex32 -k=heinrichelsigan.area23.at -H=Whirlpool -z=bz -o=.\READ_BUNZIP.txt

    EU.CqrXs.Console.exe -V -i=.\README.MD -z=zip -k=io.cqrxs.eu -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4 -H=SCrypt -e=uu -o=.\README.MD.SCrypt.zip.uu
    EU.CqrXs.Console.exe -V -D -i=.\README.MD.SCrypt.zip.uu -e=uu -k=io.cqrxs.eu -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4 -H=SCrypt -z=zip -o=.\READ_UNZIP.txt

    EU.CqrXs.Console.exe -V -i=.\README.MD -S -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\README.MD.BCrypt.zip.xx
    EU.CqrXs.Console.exe -V -D -i=.\README.MD.BCrypt.zip.xx -S -e=xx -k=io.cqrxs.eu -H=BCrypt -z=zip -o=.\README_SYM_BCRYPT_UNZIP.txt

U:\source\PermAgainCrypt\Deploy\console\x86>
```

# [Java](https://github.com/heinrichelsigan/PermAgainCrypt/tree/main/java)

<img width="1007" height="764" alt="2026-01-25_javax_swing_JFrame_java" src="https://github.com/user-attachments/assets/fb449450-0dcd-481e-a75f-572efbf8d5ee" />

## Java C# compare encoding / decoding

- RC564 is not well implemented by me in java.
- ZenMatrix has **now** *been already* ported by me to java.
- ZenMatrix2, BZip2 and Zip aren't already ported by me to java.
- Ascon256 and Xoodyak have currently every replaced by *Oct* and *TupleHash*

<img width="2017" height="932" alt="2026-01-27_Screenshot_Java_CSharp" src="https://github.com/user-attachments/assets/411827b4-cb98-48c2-ade1-4692c3fb858f" />

![Peek_2026_01_28_0340](https://github.com/user-attachments/assets/28231520-d5e9-4eac-8673-f41c227d4870)


# [Android](https://github.com/heinrichelsigan/PermAgainCrypt/tree/main/android)

<img width="720" height="1600" alt="Screenshot_20260202-081909" src="https://github.com/user-attachments/assets/69402018-f67e-40ad-987c-61a6ca925361" />


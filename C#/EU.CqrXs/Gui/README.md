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


### Online [latex help](https://cqrxs.eu/help/Making_symmetric_BlockCipher_meta_permutating_again.pdf)


## C# solution and all projects ported to .NetCore 10
You need since 2025-11-15 Visual Studio 2026 and C# .NetCore 10.0 to compile C# csproj.

## Download

### Releases
https://github.com/heinrichelsigan/PermAgainCrypt/releases/

### [cqrxs.eu/download](https://cqrxs.eu/download/)
- [PermAgainCrypt_WinForm_NetCore10_x86+x64](https://cqrxs.eu/download/PermAgainCrypt_WinForm_NetCore10_x64.7z)
- Rendundant website is [io.cqrxs.eu/download](https://io.cqrxs.eu/download/)

## WinFormCore:
<img width="800" height="726" alt="image" src="https://raw.githubusercontent.com/heinrichelsigan/PermAgainCrypt/refs/heads/main/docu/PermAgainCrypt_WinForm.gif" />

## Videos
- walk through: https://youtu.be/GX9q1sRx3nE
- https://youtu.be/5J3R1gg-jjA
- https://youtu.be/xpnnrxc2znA
- https://youtu.be/tY2DPsZjbVQ

## Credits
- Great Thanks to [the Legion of the Bouncy Castle](https://www.bouncycastle.org/), git [bcgit](https://github.com/bcgit)
- [libtom.net](https://www.libtom.net/), git [libtom/libtomcrypt](https://github.com/libtom/libtomcrypt) 
- [cryptopp.com](https://cryptopp.com/), git [weidai11/cryptopp](https://github.com/libtom/libtomcrypt) 

# **Java**  CipherPipe *Prototype* as JFrame

## Requirements

- JDK installed
- openjdk-21
- openjdk-25

## Windows 

- change following lines in *winmake.bat* depdendent on your JDK installation and bouncy-castle version:
```
echo Setting bouncy-castle jar
SET BCJAR=bcprov-lts8on-2.73.10.jar
echo Setting Path and CLASSPATH
SET Path=%Path%;%USERPROFILE%\.jdks\openjdk-25\bin

SET CLASSPATH=%CLASSPATH%;%USERPROFILE%\.jdks\openjdk-25\lib
SET MYCLASSPATH=%CLASSPATH%;.\;.\%BCJAR%;.\eu\cqrxs\;.\eu\cqrxs\gui\;.\eu\cqrxs\fw\net\;.\eu\cqrxs\fw\util\;.\eu\cqrxs\fw\crypt\;.\eu\cqrxs\fw\crypt\encoding\;.\eu\cqrxs\fw\crypt\cipher\;.\eu\cqrxs\fw\crypt\hash\;
```
- run: *winmake.bat*

## Linux

### classic Makefile
- change *Makefile* dependent on your JDK installation and bouncy-castle version
- then run make
```
make clean; make all; make run
```

### fast jmake.sh shell script 

- change these linews in *jmake.sh*  depdendent on your JDK installation and bouncy-castle version:
  remember: classpath seperator under unix is <b>:</b> and **NOT semicolon ;**
```
echo "setting bouncy castle jar and MYCLASSPATH"
BCJAR=bcprov-lts8on-2.73.10.jar

MYCLASSPATH="$CLASSPATH:./:./$BCJAR:./eu/cqrxs/:./eu/cqrxs/gui/:./eu/cqrxs/fw/net/:./eu/cqrxs/fw/util/:./eu/cqrxs/fw/crypt/:./eu/cqrxs/fw/crypt/encoding/:./eu/cqrxs/fw/crypt/cipher/:./eu/cqrxs/fw/crypt/hash/:"
```
- execute: *sh -x jmake.sh*

<hr />

## java file tree

```
.
├── bcprov-lts8on-2.73.10.jar
├── eu
│   ├── cqrxs
│   │   ├── console
│   │   │   ├── ConsoleMain_java.txt
│   │   │   └── OptEnum.java
│   │   ├── crypt
│   │   │   ├── cipher
│   │   │   │   ├── CipherEnum.java
│   │   │   │   ├── CipherPipe.java
│   │   │   │   ├── CryptBounceCastle.java
│   │   │   │   ├── CryptHelper.java
│   │   │   │   ├── CryptParams.java
│   │   │   │   ├── SymmCipherEnum.java
│   │   │   │   └── ZenMatrix.java
│   │   │   ├── encoding
│   │   │   │   ├── Base16Coder.java
│   │   │   │   ├── Base64Coder.java
│   │   │   │   ├── EncodeEnum.java
│   │   │   │   ├── EnDeCodeHelper.java
│   │   │   │   ├── EnDeCoder.java
│   │   │   │   ├── Hex16Coder.java
│   │   │   │   ├── uu
│   │   │   │   │   ├── CEFormatException.java
│   │   │   │   │   ├── CEStreamExhausted.java
│   │   │   │   │   ├── CharacterDecoder.java
│   │   │   │   │   ├── CharacterEncoder.java
│   │   │   │   │   ├── UUDecoder.java
│   │   │   │   │   └── UUEncoder.java
│   │   │   │   ├── UuCoder.java
│   │   │   │   └── XxEncoder_java.txt
│   │   │   └── hash
│   │   │       ├── BCrypt.java
│   │   │       ├── Blake2xs.java
│   │   │       ├── CShake.java
│   │   │       ├── Dstu7564.java
│   │   │       ├── Hex.java
│   │   │       ├── KeyHash.java
│   │   │       ├── MD5.java
│   │   │       ├── Oct.java
│   │   │       ├── OpenBSDCrypt.java
│   │   │       ├── RipeMD256.java
│   │   │       ├── SCrypt.java
│   │   │       ├── Sha1.java
│   │   │       ├── Sha256.java
│   │   │       ├── Sha384.java
│   │   │       ├── Sha512.java
│   │   │       ├── TupleHash.java
│   │   │       └── Whirlpool.java
│   │   ├── gui
│   │   │   ├── CqrJdFrame.java
│   │   │   ├── CqrJDialog.java
│   │   │   ├── CqrJdPanel_java.txt
│   │   │   ├── cqrxs-eu.jpg
│   │   │   ├── ErrorsBundle.java
│   │   │   ├── ImageTest.java
│   │   │   ├── ImageViewer.java
│   │   │   ├── PropertyChangeSupport.java
│   │   │   └── VetoableChangeSupport.java
│   │   ├── util
│   │   │   ├── CException.java
│   │   │   ├── Constants.java
│   │   │   ├── ContextLazy.java
│   │   │   ├── DbgWriter.java
│   │   │   ├── Fortune.java
│   │   │   └── NotImplementedError.java
│   │   └── zip
│   │       ├── GZ.java
│   │       └── ZipType.java
│   └── net
│       ├── addr
│       │   ├── NetworkAddresses.java
│       │   └── WinMake.bat
│       └── server6
│           ├── EchoClient.java
│           ├── EchoInputStream.java
│           ├── EchoServer.java
│           ├── Makefile
│           └── winmake.bat
├── jmake.sh
├── Makefile
├── PermAgainCrypt.iml
├── README.md
└── winmake.bat
```

## Sceenshot

<img src="https://raw.githubusercontent.com/heinrichelsigan/PermAgainCrypt/refs/heads/main/docu/2025-12-28_javaProtorype.gif" />


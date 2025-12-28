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

- change these linews in *jmake.sh*  depdendent on your JDK installation and bouncy-castle version:
```
echo "setting bouncy castle jar and MYCLASSPATH"
# BCJAR=bcprov-jdk18on-1.79.jar
BCJAR=bcprov-lts8on-2.73.10.jar

# CLASSPATH = %CLASSPATH%;C:/Users/heinrich.elsigan/.jdks/openjdk-25/lib
MYCLASSPATH="$CLASSPATH:./:./$BCJAR:./eu/cqrxs/:./eu/cqrxs/gui/:./eu/cqrxs/fw/net/:./eu/cqrxs/fw/util/:./eu/cqrxs/fw/crypt/:./eu/cqrxs/fw/crypt/encoding/:./eu/cqrxs/fw/crypt/cipher/:./eu/cqrxs/fw/crypt/hash/:"
```
- remember: classpath seperator under unix is <b>:</b> and **NOT semicolon ;**
- execute *sh -x jmake.sh*

<hr />

## Sceenshot

<img src="https://raw.githubusercontent.com/heinrichelsigan/PermAgainCrypt/refs/heads/main/docu/2025-12-28_javaProtorype.gif" />



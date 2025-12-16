@echo off

REM 
REM Path=%Path%;"C:\Program Files\Android\Android Studio\jbr\bin"
REM SET CLASSPATH = %CLASSPATH%;"C:\Program Files\Android\Android Studio\jbr\lib"

echo Setting Path and CLASSPATH
SET Path=%Path%;C:\Users\heinrich.elsigan\.jdks\openjdk-25\bin

SET CLASSPATH = %CLASSPATH%;C:\Users\heinrich.elsigan\.jdks\openjdk-25\lib
set MYCLASSPATH=%CLASSPATH%;.\;.\bcprov-jdk18on-1.79.jar;.\eu\cqrxs\;.\eu\cqrxs\gui\;.\eu\cqrxs\fw\net\;.\eu\cqrxs\fw\util\;.\eu\cqrxs\fw\crypt\;.\eu\cqrxs\fw\crypt\encoding\;.\eu\cqrxs\fw\crypt\cipher\;.\eu\cqrxs\fw\crypt\hash\;

echo "cleaning classes from last build in eu/cqrxs/ eu/cqrxs/gui/ "
echo "del /s /f /q *.class"
del /s /f /q *.class

echo "compiling now with javac CqrXs.Eu "
    

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\fw\util\CException.java eu\cqrxs\fw\util\NotImplementedError.java eu\cqrxs\fw\util\Constants.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\fw\util\CException.java eu\cqrxs\fw\util\NotImplementedError.java eu\cqrxs\fw\util\Constants.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\fw\net\NetworkAddresses.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\net\NetworkAddresses.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\gui\CqrJDialog.java eu\cqrxs\gui\CqrJdFrame.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\gui\CqrJDialog.java eu\cqrxs\gui\CqrJdFrame.java
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\gui\ImageTest.java


echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\fw\crypt\encoding\uu\CEFormatException.java eu\cqrxs\fw\crypt\encoding\uu\CEStreamExhausted.java eu\cqrxs\fw\crypt\encoding\uu\CharacterDecoder.java eu\cqrxs\fw\crypt\encoding\uu\CharacterEncoder.java  eu\cqrxs\fw\crypt\encoding\uu\UUDecoder.java  eu\cqrxs\fw\crypt\encoding\uu\UUEncoder.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\fw\crypt\encoding\uu\CEFormatException.java eu\cqrxs\fw\crypt\encoding\uu\CEStreamExhausted.java eu\cqrxs\fw\crypt\encoding\uu\CharacterDecoder.java eu\cqrxs\fw\crypt\encoding\uu\CharacterEncoder.java  eu\cqrxs\fw\crypt\encoding\uu\UUDecoder.java  eu\cqrxs\fw\crypt\encoding\uu\UUEncoder.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\crypt\encoding\EncodeEnum.java eu\cqrxs\fw\crypt\encoding\EnDeCoder.java  eu\cqrxs\fw\crypt\encoding\Base16Coder.java  eu\cqrxs\fw\crypt\encoding\Hex16Coder.java eu\cqrxs\fw\crypt\encoding\Base64Coder.java  eu\cqrxs\fw\crypt\encoding\UuCoder.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\crypt\encoding\EncodeEnum.java eu\cqrxs\fw\crypt\encoding\EnDeCoder.java  eu\cqrxs\fw\crypt\encoding\Base16Coder.java  eu\cqrxs\fw\crypt\encoding\Hex16Coder.java eu\cqrxs\fw\crypt\encoding\Base64Coder.java  eu\cqrxs\fw\crypt\encoding\UuCoder.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\fw\crypt\cipher\CipherEnum.java eu\cqrxs\fw\crypt\cipher\CipherPipe.java eu\cqrxs\fw\crypt\cipher\CryptBounceCastle.java eu\cqrxs\fw\crypt\cipher\CryptHelper.java eu\cqrxs\fw\crypt\cipher\CryptParams.java eu\cqrxs\fw\crypt\cipher\SymmCipherEnum.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\fw\crypt\cipher\CipherEnum.java eu\cqrxs\fw\crypt\cipher\CipherPipe.java eu\cqrxs\fw\crypt\cipher\CryptBounceCastle.java eu\cqrxs\fw\crypt\cipher\CryptHelper.java eu\cqrxs\fw\crypt\cipher\CryptParams.java eu\cqrxs\fw\crypt\cipher\SymmCipherEnum.java

echo "javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\crypt\hash\Hex.java eu\cqrxs\fw\crypt\hash\KeyHash.java eu\cqrxs\fw\crypt\hash\MD5.java eu\cqrxs\fw\crypt\hash\RipeMD256.java eu\cqrxs\fw\crypt\hash\Sha1.java eu\cqrxs\fw\crypt\hash\Sha256.java eu\cqrxs\fw\crypt\hash\Sha384.java eu\cqrxs\fw\crypt\hash\Sha512.java eu\cqrxs\fw\crypt\hash\Whirlpool.java eu\cqrxs\fw\crypt\hash\Dstu7564.java"
javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\crypt\hash\Hex.java eu\cqrxs\fw\crypt\hash\KeyHash.java eu\cqrxs\fw\crypt\hash\MD5.java eu\cqrxs\fw\crypt\hash\RipeMD256.java eu\cqrxs\fw\crypt\hash\Sha1.java eu\cqrxs\fw\crypt\hash\Sha256.java eu\cqrxs\fw\crypt\hash\Sha384.java eu\cqrxs\fw\crypt\hash\Sha512.java eu\cqrxs\fw\crypt\hash\Whirlpool.java eu\cqrxs\fw\crypt\hash\Dstu7564.java
 
echo "javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\util\CException.java eu\cqrxs\fw\util\Constants.java eu\cqrxs\fw\util\ContextLazy.java"
javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\util\CException.java eu\cqrxs\fw\util\Constants.java eu\cqrxs\fw\util\ContextLazy.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\fw\zip\GZ.java  eu\cqrxs\fw\zip\ZipType.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\fw\zip\GZ.java  eu\cqrxs\fw\zip\ZipType.java


echo "build finished"

pause
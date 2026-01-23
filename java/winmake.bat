@echo off

REM 

REM set Path=%Path%;"C:\Program Files\Android\Android Studio\jbr\bin"
REM set Path=%Path%;%USERPROFILE%\.jdks\graalvm-jdk-21.0.2\bin
REM set Path=%Path%;%USERPROFILE%\.jdks\corretto-23.0.1\bin
REM set Path=%Path%;%USERPROFILE%\.jdks\liberica-full-21.0.2\bin
REM set Path=%Path%;%USERPROFILE%\.jdks\semeru-21.0.2\bin

REM Path=%Path%;"C:\Program Files\Android\Android Studio\jbr\bin"
REM SET CLASSPATH = %CLASSPATH%;"C:\Program Files\Android\Android Studio\jbr\lib"

echo Setting bouncy-castle jar
REM SET BCJAR=bcprov-jdk18on-1.79.jar
SET BCJAR=bcprov-lts8on-2.73.10.jar
echo Setting Path and CLASSPATH
SET Path=%Path%;%USERPROFILE%\.jdks\openjdk-25\bin

SET CLASSPATH=%CLASSPATH%;%USERPROFILE%\.jdks\openjdk-25\lib
SET MYCLASSPATH=%CLASSPATH%;.\;.\%BCJAR%;.\eu\cqrxs\;.\eu\cqrxs\gui\;.\eu\cqrxs\net\;eu\cqrxs\net\addr\;.\eu\cqrxs\net\server6;.\eu\cqrxs\util\;.\eu\cqrxs\crypt\;.\eu\cqrxs\crypt\encoding\;.\eu\cqrxs\crypt\cipher\;.\eu\cqrxs\crypt\hash\;	

echo "cleaning classes from last build in eu/cqrxs/ eu/cqrxs/gui/ "
echo "del /s /f /q *.class"
del /s /f /q *.class

echo "compiling now with javac CqrXs.Eu "
    

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\util\CException.java eu\cqrxs\util\NotImplementedError.java eu\cqrxs\util\Constants.java eu\cqrxs\util\DbgWriter.java "
javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\util\CException.java eu\cqrxs\util\NotImplementedError.java eu\cqrxs\util\Constants.java eu\cqrxs\util\DbgWriter.java 

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\net\NetworkAddresses.java eu\cqrxs\net\server6\EchoInputStream.java eu\cqrxs\net\server6\EchoServer.java eu\cqrxs\net\server6\EchoClient.java "
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\net\NetworkAddresses.java eu\cqrxs\net\server6\EchoInputStream.java eu\cqrxs\net\server6\EchoServer.java eu\cqrxs\net\server6\EchoClient.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\encoding\uu\CEFormatException.java eu\cqrxs\crypt\encoding\uu\CEStreamExhausted.java eu\cqrxs\crypt\encoding\uu\CharacterDecoder.java eu\cqrxs\crypt\encoding\uu\CharacterEncoder.java  eu\cqrxs\crypt\encoding\uu\UUDecoder.java  eu\cqrxs\crypt\encoding\uu\UUEncoder.java eu\cqrxs\crypt\encoding\EnDeCodeHelper.java  eu\cqrxs\crypt\encoding\EncodeEnum.java eu\cqrxs\crypt\encoding\IEncodable.java  eu\cqrxs\crypt\encoding\Base16Coder.java  eu\cqrxs\crypt\encoding\Hex16Coder.java eu\cqrxs\crypt\encoding\uu\Hex64Coder.java eu\cqrxs\crypt\encoding\Base64Coder.java  eu\cqrxs\crypt\encoding\UuCoder.java eu\cqrxs\crypt\encoding\XxEncoder.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\encoding\uu\CEFormatException.java eu\cqrxs\crypt\encoding\uu\CEStreamExhausted.java eu\cqrxs\crypt\encoding\uu\CharacterDecoder.java eu\cqrxs\crypt\encoding\uu\CharacterEncoder.java eu\cqrxs\crypt\encoding\uu\UUDecoder.java eu\cqrxs\crypt\encoding\EnDeCodeHelper.java eu\cqrxs\crypt\encoding\EncodeEnum.java eu\cqrxs\crypt\encoding\IEncodable.java  eu\cqrxs\crypt\encoding\Base16Coder.java  eu\cqrxs\crypt\encoding\Hex16Coder.java eu\cqrxs\crypt\encoding\uu\Hex64Coder.java eu\cqrxs\crypt\encoding\Base64Coder.java  eu\cqrxs\crypt\encoding\UuCoder.java eu\cqrxs\crypt\encoding\XxEncoder.java eu\cqrxs\crypt\encoding\uu\UUEncoder.java 

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\cipher\CipherEnum.java eu\cqrxs\crypt\cipher\CipherPipe.java eu\cqrxs\crypt\cipher\CryptBounceCastle.java eu\cqrxs\crypt\cipher\CryptHelper.java eu\cqrxs\crypt\cipher\CryptParams.java eu\cqrxs\crypt\cipher\SymmCipherEnum.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\cipher\CipherEnum.java eu\cqrxs\crypt\cipher\CipherPipe.java eu\cqrxs\crypt\cipher\CryptBounceCastle.java eu\cqrxs\crypt\cipher\CryptHelper.java eu\cqrxs\crypt\cipher\CryptParams.java eu\cqrxs\crypt\cipher\SymmCipherEnum.java

echo "javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\crypt\hash\Hex.java eu\cqrxs\crypt\hash\KeyHash.java eu\cqrxs\crypt\hash\OpenBSDCrypt.java eu\cqrxs\crypt\hash\BCrypt.java eu\cqrxs\crypt\hash\SCrypt.java eu\cqrxs\crypt\hash\MD5.java eu\cqrxs\crypt\hash\RipeMD256.java eu\cqrxs\crypt\hash\Sha1.java eu\cqrxs\crypt\hash\Sha256.java eu\cqrxs\crypt\hash\Sha384.java eu\cqrxs\crypt\hash\Sha512.java eu\cqrxs\crypt\hash\Whirlpool.java eu\cqrxs\crypt\hash\Dstu7564.java"
javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\crypt\hash\Hex.java eu\cqrxs\crypt\hash\KeyHash.java eu\cqrxs\crypt\hash\OpenBSDCrypt.java eu\cqrxs\crypt\hash\BCrypt.java eu\cqrxs\crypt\hash\SCrypt.java  eu\cqrxs\crypt\hash\MD5.java eu\cqrxs\crypt\hash\RipeMD256.java eu\cqrxs\crypt\hash\Sha1.java eu\cqrxs\crypt\hash\Sha256.java eu\cqrxs\crypt\hash\Sha384.java eu\cqrxs\crypt\hash\Sha512.java eu\cqrxs\crypt\hash\Whirlpool.java eu\cqrxs\crypt\hash\Dstu7564.java eu\cqrxs\crypt\hash\Oct.java  eu\cqrxs\crypt\hash\TupleHash.java
 
echo "javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\util\CException.java eu\cqrxs\util\Constants.java eu\cqrxs\util\ContextLazy.java"
javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\util\CException.java eu\cqrxs\util\Constants.java eu\cqrxs\util\ContextLazy.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\zip\GZ.java  eu\cqrxs\zip\ZipType.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\zip\GZ.java  eu\cqrxs\zip\ZipType.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\gui\PropertyChangeSupport.java eu\cqrxs\gui\PropertyChangeSupport.java eu\cqrxs\gui\ImageViewer.java eu\cqrxs\gui\CqrJDialog.java eu\cqrxs\util\Fortune.java eu\cqrxs\gui\CqrJdFrame.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\gui\PropertyChangeSupport.java eu\cqrxs\gui\PropertyChangeSupport.java eu\cqrxs\util\Fortune.java  eu\cqrxs\gui\ImageViewer.java eu\cqrxs\gui\CqrJDialog.java eu\cqrxs\gui\CqrJdFrame.java
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\gui\ImageTest.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\console\OptEnum.java eu\cqrxs\console\CryptConsole.java "
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\console\OptEnum.java eu\cqrxs\console\CryptConsole.java 


echo "build finished"

pause

java.exe -classpath %MYCLASSPATH% eu\cqrxs\gui\CqrJdFrame.java

pause

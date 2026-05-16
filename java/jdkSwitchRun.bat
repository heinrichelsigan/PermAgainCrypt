@echo off

echo "cleaning classes from last build in eu/cqrxs/* by exec: del /s /f /q *.class"
del /s /f /q *.class

:initial
if "%1"=="coretto" goto coretto
if "%1"=="Coretto" goto coretto
if "%1"=="amazon" goto coretto
if "%1"=="Amazon" goto coretto
if "%1"=="MicrosoftJDK" goto msjdk
if "%1"=="microsoftjdk" goto msjdk
if "%1"=="Microsoft" goto msjdk
if "%1"=="microsoft" goto msjdk
if "%1"=="MSJDK" goto msjdk
if "%1"=="semeru" goto semeru
if "%1"=="Semeru" goto semeru
if "%1"=="ibm" goto semeru
if "%1"=="IBM" goto semeru
if "%1"=="jetbrains" goto jetbrains
if "%1"=="JetBrains" goto jetbrains
if "%1"=="jbr" goto jetbrains
if "%1"=="JBR" goto jetbrains
if "%1"=="yourjdk" goto yourjdk
if "%1"=="YourJdk" goto yourjdk
if "%1"=="YOURJDK" goto yourjdk
if "%1"=="openjdk" goto openjdk
if "%1"=="OpenJDK" goto openjdk
goto yourjdk

:coretto
echo "Amazon Coretto JDK added to Path and CLASSPATH"
set Path=%Path%;%USERPROFILE%\.jdks\corretto-24.0.2\bin
set CLASSPATH=%CLASSPATH%;%USERPROFILE%\.jdks\corretto-24.0.2\lib
goto bouncycastle

:msjdk
echo "Microsoft JDK added to Path and CLASSPATH"
set Path=%Path%;%USERPROFILE%\.jdks\ms-25.0.2\bin
set CLASSPATH=%CLASSPATH%;%USERPROFILE%\.jdks\ms-25.0.2\lib
goto bouncycastle

:semeru
echo "IBM Semeru JDK added to Path and CLASSPATH"
set Path=%Path%;%USERPROFILE%\.jdks\semeru-26\bin
set CLASSPATH=%CLASSPATH%;%USERPROFILE%\.jdks\semeru-26\lib
goto bouncycastle

:jetbrains
echo "JetBrains JBR JDK added to Path and CLASSPATH"
set Path=%Path%;%USERPROFILE%\.jdks\jbr-25.0.2\bin
set CLASSPATH=%CLASSPATH%;%USERPROFILE%\.jdks\jbr-25.0.2\lib
goto bouncycastle

:yourjdk
echo "YOURJDK %YOURJDK% is set"
if "%CLASSPATH%" == "" goto openjdk
echo "ClassPath: %CLASSPATH%"
goto bouncycastle

:openjdk
echo "Orcacle OpenJDK added to Path and CLASSPATH"
set Path=%Path%;%USERPROFILE%\.jdks\openjdk-26\bin
set CLASSPATH=%CLASSPATH%;%USERPROFILE%\.jdks\openjdk-26\lib
goto bouncycastle


:bouncycastle
echo Setting bouncy-castle jar and MYCLASSPATH
REM set BCJAR=bcprov-jdk18on-1.79.jar
set BCJAR=bcprov-lts8on-2.73.10.jar
set MYCLASSPATH=%CLASSPATH%;.\;.\%BCJAR%;.\eu\cqrxs\;.\eu\cqrxs\gui\;.\eu\cqrxs\net\;eu\cqrxs\net\addr\;.\eu\cqrxs\net\server6;.\eu\cqrxs\util\;.\eu\cqrxs\crypt\;.\eu\cqrxs\crypt\encoding\;.\eu\cqrxs\crypt\cipher\;.\eu\cqrxs\crypt\hash\;	

echo "compiling CqrXs.Eu.* now with javac"

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\util\CException.java eu\cqrxs\util\NotImplementedError.java eu\cqrxs\util\Constants.java eu\cqrxs\util\DbgWriter.java eu\cqrxs\gui\ImageHelper.java "
javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\util\CException.java eu\cqrxs\util\NotImplementedError.java eu\cqrxs\util\Constants.java eu\cqrxs\util\DbgWriter.java eu\cqrxs\gui\ImageHelper.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\net\addr\NetworkAddresses.java eu\cqrxs\net\server6\EchoInputStream.java eu\cqrxs\net\server6\EchoServer.java eu\cqrxs\net\server6\EchoClient.java "
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\net\addr\NetworkAddresses.java eu\cqrxs\net\server6\EchoInputStream.java eu\cqrxs\net\server6\EchoServer.java eu\cqrxs\net\server6\EchoClient.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\encoding\uu\CEFormatException.java eu\cqrxs\crypt\encoding\uu\CEStreamExhausted.java eu\cqrxs\crypt\encoding\uu\CharacterDecoder.java eu\cqrxs\crypt\encoding\uu\CharacterEncoder.java  eu\cqrxs\crypt\encoding\uu\UUDecoder.java  eu\cqrxs\crypt\encoding\uu\UUEncoder.java eu\cqrxs\crypt\encoding\EnDeCodeHelper.java  eu\cqrxs\crypt\encoding\EncodeEnum.java eu\cqrxs\crypt\encoding\IEncodable.java  eu\cqrxs\crypt\encoding\Base16Coder.java  eu\cqrxs\crypt\encoding\Hex16Coder.java eu\cqrxs\crypt\encoding\Hex32Coder.java  eu\cqrxs\crypt\encoding\Hex64Coder.java eu\cqrxs\crypt\encoding\Base64Coder.java  eu\cqrxs\crypt\encoding\UuCoder.java eu\cqrxs\crypt\encoding\XxEncoder.java eu\cqrxs\crypt\encoding\Ascii85Coder.java  "

javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\encoding\uu\CEFormatException.java eu\cqrxs\crypt\encoding\uu\CEStreamExhausted.java eu\cqrxs\crypt\encoding\uu\CharacterDecoder.java eu\cqrxs\crypt\encoding\uu\CharacterEncoder.java eu\cqrxs\crypt\encoding\uu\UUDecoder.java eu\cqrxs\crypt\encoding\EnDeCodeHelper.java eu\cqrxs\crypt\encoding\EncodeEnum.java eu\cqrxs\crypt\encoding\IEncodable.java  eu\cqrxs\crypt\encoding\Base16Coder.java  eu\cqrxs\crypt\encoding\Hex16Coder.java  eu\cqrxs\crypt\encoding\Hex32Coder.java  eu\cqrxs\crypt\encoding\Hex64Coder.java eu\cqrxs\crypt\encoding\Base64Coder.java  eu\cqrxs\crypt\encoding\UuCoder.java eu\cqrxs\crypt\encoding\XxEncoder.java eu\cqrxs\crypt\encoding\uu\UUEncoder.java eu\cqrxs\crypt\encoding\Ascii85Coder.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\cipher\CipherEnum.java eu\cqrxs\crypt\cipher\CipherPipe.java eu\cqrxs\crypt\cipher\SecureCipherPipe.java eu\cqrxs\crypt\cipher\CryptBounceCastle.java eu\cqrxs\crypt\cipher\CryptHelper.java eu\cqrxs\crypt\cipher\CryptParams.java  eu\cqrxs\crypt\cipher\JAes.java eu\cqrxs\crypt\cipher\ZenMatrix.java  eu\cqrxs\crypt\cipher\ZenMatrix2.java  eu\cqrxs\crypt\cipher\ZenMatrix3.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\cipher\CipherEnum.java eu\cqrxs\crypt\cipher\CipherPipe.java eu\cqrxs\crypt\cipher\SecureCipherPipe.java eu\cqrxs\crypt\cipher\CryptBounceCastle.java eu\cqrxs\crypt\cipher\CryptHelper.java eu\cqrxs\crypt\cipher\CryptParams.java eu\cqrxs\crypt\cipher\JAes.java eu\cqrxs\crypt\cipher\ZenMatrix.java  eu\cqrxs\crypt\cipher\ZenMatrix2.java  eu\cqrxs\crypt\cipher\ZenMatrix3.java

echo "javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\hash\OpenBSDCrypt.java eu\cqrxs\crypt\hash\BCrypt.java eu\cqrxs\crypt\hash\SCrypt.java eu\cqrxs\crypt\hash\MD5.java eu\cqrxs\crypt\hash\Sha256.java  eu\cqrxs\crypt\hash\Sha512.java eu\cqrxs\crypt\hash\KeyHash.java"
javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\crypt\hash\OpenBSDCrypt.java eu\cqrxs\crypt\hash\BCrypt.java eu\cqrxs\crypt\hash\SCrypt.java  eu\cqrxs\crypt\hash\MD5.java eu\cqrxs\crypt\hash\Sha256.java eu\cqrxs\crypt\hash\Sha512.java eu\cqrxs\crypt\hash\KeyHash.java 
 
echo "javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\util\CException.java eu\cqrxs\util\Constants.java eu\cqrxs\util\ContextLazy.java"
javac.exe  -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\util\CException.java eu\cqrxs\util\Constants.java eu\cqrxs\util\ContextLazy.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\zip\GZ.java  eu\cqrxs\zip\ZipType.java"
javac.exe -classpath %MYCLASSPATH% -Xlint:deprecation eu\cqrxs\zip\GZ.java  eu\cqrxs\zip\ZipType.java

echo "javac -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu/cqrxs/util/Fortune.java eu/cqrxs/gui/DropPanel.java eu/cqrxs/gui/CqrJDialog.java eu/cqrxs/gui/CqrJdFrame.java eu/cqrxs/gui/CqrJFrameSimple.java "
javac -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu/cqrxs/util/Fortune.java eu/cqrxs/gui/DropPanel.java eu/cqrxs/gui/CqrJDialog.java eu/cqrxs/gui/CqrJdFrame.java eu/cqrxs/gui/CqrJFrameSimple.java

echo "javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\console\OptEnum.java eu\cqrxs\console\CryptConsole.java "
javac.exe -classpath %MYCLASSPATH% -Xlint:unchecked -Xlint:deprecation eu\cqrxs\console\OptEnum.java eu\cqrxs\console\CryptConsole.java 

echo "build finished"


echo "testing Cosnole java.exe -classpath %MYCLASSPATH% eu\cqrxs\console\CryptConsole.java"
java.exe -classpath %MYCLASSPATH% eu\cqrxs\console\CryptConsole.java


echo "launching JFrame java.exe -classpath %MYCLASSPATH% eu\cqrxs\console\CryptConsole.java"
java.exe -classpath %MYCLASSPATH% eu\cqrxs\gui\CqrJdFrame.java


pause

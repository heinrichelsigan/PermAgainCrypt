#!/usr/bin/bash

# Path=%Path%;"C:\Program Files\Android\Android Studio\jbr\bin"
# set Path=%Path%;c:\Users\heinr\.jdks\graalvm-jdk-21.0.2\bin
# set Path=%Path%;c:\Users\heinr\.jdks\corretto-23.0.1\bin
# set Path=%Path%;c:\Users\heinr\.jdks\liberica-full-21.0.2\bin
# set Path=%Path%;c:\Users\heinr\.jdks\semeru-21.0.2\bin

# CLASSPATH = %CLASSPATH%;C:\Users\heinrich.elsigan\.jdks\openjdk-25\lib
MYCLASSPATH=$CLASSPATH;.\;.\bcprov-jdk18on-1.79.jar;.\eu\cqrxs\;.\eu\cqrxs\gui\;.\eu\cqrxs\fw\net\;.\eu\cqrxs\fw\util\;.\eu\cqrxs\fw\crypt\;.\eu\cqrxs\fw\crypt\encoding\;.\eu\cqrxs\fw\crypt\cipher\;.\eu\cqrxs\fw\crypt\hash\;

echo "$0: cleaning classes from last build in eu/cqrxs/ eu/cqrxs/cqrframe/ eu/cqrxs/gui/ "
echo -n "$0: rm -f "
for fc in $(find -iname '*.class' -ipath '*eu/cqrxs*') ; do
    echo -n "$fc " ;
    rm -f $fc 
done

echo "$0: compiling now with javac CqrJd: "
for fj in $(find -iname '*.java' -ipath '*eu/cqrxs*') ; do
    echo -n "$fj ";
done 



echo "$0: javac -classpath $MYCLASSPATH -Xlint:deprecation eu\cqrxs\fw\util\CException.java eu\cqrxs\fw\util\NotImplementedError.java eu\cqrxs\fw\util\Constants.java"
javac -classpath $MYCLASSPATH -Xlint:deprecation eu\cqrxs\fw\util\CException.java eu\cqrxs\fw\util\NotImplementedError.java eu\cqrxs\fw\util\Constants.java

echo "$0: javac -classpath $MYCLASSPATH -Xlint:deprecation eu/cqrxs/fw/net/NetworkAddresses.java"
javac -classpath $MYCLASSPATH -Xlint:deprecation eu/cqrxs/fw/net/NetworkAddresses.java

echo "$0: javac -classpath $MYCLASSPATH -Xlint:deprecation gui/*.java "
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\gui\CqrJDialog.java eu\cqrxs\gui\CqrJdFrame.java
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\gui\ImageTest.java


echo "$0: javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation eu\cqrxs\fw\crypt\encoding\uu\CEFormatException.java eu\cqrxs\fw\crypt\encoding\uu\CEStreamExhausted.java eu\cqrxs\fw\crypt\encoding\uu\CharacterDecoder.java eu\cqrxs\fw\crypt\encoding\uu\CharacterEncoder.java  eu\cqrxs\fw\crypt\encoding\uu\UUDecoder.java  eu\cqrxs\fw\crypt\encoding\uu\UUEncoder.java"
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation eu\cqrxs\fw\crypt\encoding\uu\CEFormatException.java eu\cqrxs\fw\crypt\encoding\uu\CEStreamExhausted.java eu\cqrxs\fw\crypt\encoding\uu\CharacterDecoder.java eu\cqrxs\fw\crypt\encoding\uu\CharacterEncoder.java  eu\cqrxs\fw\crypt\encoding\uu\UUDecoder.java  eu\cqrxs\fw\crypt\encoding\uu\UUEncoder.java


echo "$0 javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\crypt\encoding\EncodeEnum.java eu\cqrxs\fw\crypt\encoding\EnDeCoder.java  eu\cqrxs\fw\crypt\encoding\Base16Coder.java  eu\cqrxs\fw\crypt\encoding\Hex16Coder.java eu\cqrxs\fw\crypt\encoding\Base64Coder.java  eu\cqrxs\fw\crypt\encoding\UuCoder.java"
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\crypt\encoding\EnDeCodeHelper.java eu\cqrxs\fw\crypt\encoding\EncodeEnum.java eu\cqrxs\fw\crypt\encoding\EnDeCoder.java  eu\cqrxs\fw\crypt\encoding\Base16Coder.java  eu\cqrxs\fw\crypt\encoding\Hex16Coder.java eu\cqrxs\fw\crypt\encoding\Base64Coder.java  eu\cqrxs\fw\crypt\encoding\UuCoder.java

echo "$0 javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation eu\cqrxs\fw\crypt\cipher\CipherEnum.java eu\cqrxs\fw\crypt\cipher\CipherPipe.java eu\cqrxs\fw\crypt\cipher\CryptBounceCastle.java eu\cqrxs\fw\crypt\cipher\CryptHelper.java eu\cqrxs\fw\crypt\cipher\CryptParams.java eu\cqrxs\fw\crypt\cipher\SymmCipherEnum.java"
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation eu\cqrxs\fw\crypt\cipher\CipherEnum.java eu\cqrxs\fw\crypt\cipher\CipherPipe.java eu\cqrxs\fw\crypt\cipher\CryptBounceCastle.java eu\cqrxs\fw\crypt\cipher\CryptHelper.java eu\cqrxs\fw\crypt\cipher\CryptParams.java eu\cqrxs\fw\crypt\cipher\SymmCipherEnum.java

echo "javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\crypt\hash\Hex.java eu\cqrxs\fw\crypt\hash\KeyHash.java eu\cqrxs\fw\crypt\hash\MD5.java eu\cqrxs\fw\crypt\hash\RipeMD256.java eu\cqrxs\fw\crypt\hash\Sha1.java eu\cqrxs\fw\crypt\hash\Sha256.java eu\cqrxs\fw\crypt\hash\Sha384.java eu\cqrxs\fw\crypt\hash\Sha512.java eu\cqrxs\fw\crypt\hash\Whirlpool.java eu\cqrxs\fw\crypt\hash\Dstu7564.java"
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\crypt\hash\Hex.java eu\cqrxs\fw\crypt\hash\KeyHash.java eu\cqrxs\fw\crypt\hash\OpenBSDCrypt.java eu\cqrxs\fw\crypt\hash\BCrypt.java eu\cqrxs\fw\crypt\hash\SCrypt.java  eu\cqrxs\fw\crypt\hash\MD5.java eu\cqrxs\fw\crypt\hash\RipeMD256.java eu\cqrxs\fw\crypt\hash\Sha1.java eu\cqrxs\fw\crypt\hash\Sha256.java eu\cqrxs\fw\crypt\hash\Sha384.java eu\cqrxs\fw\crypt\hash\Sha512.java eu\cqrxs\fw\crypt\hash\Whirlpool.java eu\cqrxs\fw\crypt\hash\Dstu7564.java
 
echo "javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation  eu\cqrxs\fw\util\CException.java eu\cqrxs\fw\util\Constants.java eu\cqrxs\fw\util\ContextLazy.java"
javac -classpath $MYCLASSPATH -Xlint:unchecked -Xlint:deprecation   eu\cqrxs\fw\util\CException.java eu\cqrxs\fw\util\Constants.java eu\cqrxs\fw\util\ContextLazy.java

echo "javac -classpath $MYCLASSPATH -Xlint:deprecation eu\cqrxs\fw\zip\GZ.java  eu\cqrxs\fw\zip\ZipType.java"
javac -classpath $MYCLASSPATH -Xlint:deprecation eu\cqrxs\fw\zip\GZ.java  eu\cqrxs\fw\zip\ZipType.java

echo "$0: javac -classpath $MYCLASSPATH -Xlint:deprecation eu/cqrxs/cqrframe/CqrFrame.java eu/cqrxs/cqrframe/CqrMenuBar.java "
javac -classpath $MYCLASSPATH -Xlint:deprecation eu/cqrxs/cqrframe/CqrFrame.java 
javac -classpath $MYCLASSPATH -Xlint:deprecation eu/cqrxs/cqrframe/CqrMenuBar.java

javac -classpath $MYCLASSPATH -Xlint:deprecation eu/cqrxs/JFrameApp.java


echo "build finished"


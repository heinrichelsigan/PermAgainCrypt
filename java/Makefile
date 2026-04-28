#
# Makefile for cqrxs java
# JAVA FLAGS for java runtime
JAVAFLAGS=--enable-native-access=ALL-UNNAMED
# JAVACFLAGS flags for javac compiler
JAVACFLAGS=-Xlint:unchecked -Xlint:deprecation

# BCJAR=bcprov-jdk18on-1.79.jar
BCJAR=bcprov-lts8on-2.73.10.jar

# MYCLASSPATH local java classpath
MYCLASSPATH="./:./$(BCJAR):./eu/cqrxs/:./eu/cqrxs/gui/:./eu/cqrxs/net/:./eu/cqrxs/util/:./eu/cqrxs/crypt/:./eu/cqrxs/crypt/encoding/:./eu/cqrxs/crypt/cipher/:./eu/cqrxs/crypt/hash/:"

default: all

all: cqrxs console

cqrxs:
	javac -classpath $(MYCLASSPATH) $(JAVACFLAGS) eu/cqrxs/util/CException.java eu/cqrxs/util/NotImplementedError.java eu/cqrxs/util/Constants.java eu/cqrxs/util/Fortune.java eu/cqrxs/util/DbgWriter.java  
	
	javac -classpath $(MYCLASSPATH) $(JAVACFLAGS) eu/cqrxs/crypt/encoding/uu/CEFormatException.java eu/cqrxs/crypt/encoding/uu/CEStreamExhausted.java eu/cqrxs/crypt/encoding/uu/CharacterDecoder.java eu/cqrxs/crypt/encoding/uu/CharacterEncoder.java  eu/cqrxs/crypt/encoding/uu/UUDecoder.java  eu/cqrxs/crypt/encoding/uu/UUEncoder.java eu/cqrxs/crypt/encoding/EnDeCodeHelper.java eu/cqrxs/crypt/encoding/EncodeEnum.java eu/cqrxs/crypt/encoding/IEncodable.java  eu/cqrxs/crypt/encoding/Base16Coder.java eu/cqrxs/crypt/encoding/Hex16Coder.java eu/cqrxs/crypt/encoding/Hex32Coder.java eu/cqrxs/crypt/encoding/Base64Coder.java eu/cqrxs/crypt/encoding/Hex64Coder.java  eu/cqrxs/crypt/encoding/UuCoder.java eu/cqrxs/crypt/encoding/XxEncoder.java
	
	javac -classpath $(MYCLASSPATH) $(JAVACFLAGS) eu/cqrxs/crypt/cipher/CipherEnum.java eu/cqrxs/crypt/cipher/CipherPipe.java eu/cqrxs/crypt/cipher/SecureCipherPipe.java eu/cqrxs/crypt/cipher/CryptBounceCastle.java eu/cqrxs/crypt/cipher/CryptHelper.java eu/cqrxs/crypt/cipher/CryptParams.java  eu/cqrxs/crypt/cipher/ZenMatrix.java  eu/cqrxs/crypt/cipher/ZenMatrix2.java  eu/cqrxs/crypt/cipher/ZenMatrix3.java
	
	javac -classpath $(MYCLASSPATH) $(JAVACFLAGS) eu/cqrxs/crypt/hash/KeyHash.java eu/cqrxs/crypt/hash/OpenBSDCrypt.java eu/cqrxs/crypt/hash/BCrypt.java eu/cqrxs/crypt/hash/SCrypt.java  eu/cqrxs/crypt/hash/MD5.java eu/cqrxs/crypt/hash/Sha256.java eu/cqrxs/crypt/hash/Sha512.java 
	
	javac -classpath $(MYCLASSPATH) $(JAVACFLAGS) eu/cqrxs/zip/GZ.java  eu/cqrxs/zip/ZipType.java eu/cqrxs/util/Fortune.java eu/cqrxs/gui/DropPanel.java eu/cqrxs/gui/CqrJDialog.java eu/cqrxs/gui/CqrJdFrame.java eu/cqrxs/gui/CqrJFrameSimple.java
	
	echo "build finished"

console: cqrxs
	javac -classpath $(MYCLASSPATH) $(JAVACFLAGS) eu/cqrxs/console/OptEnum.java eu/cqrxs/console/CryptConsole.java 

launch: cqrxs 
	echo "launching eu/cqrxs/gui/CqrJdFrame.java"
	sleep 1
	java $(JAVAFLAGS) -cp $(MYCLASSPATH) eu/cqrxs/gui/CqrJdFrame.java

run: cqrxs console
	echo "run eu/cqrxs/console/ConsoleApp"
	sleep 1
	java $(JAVAFLAGS) -cp $(MYCLASSPATH) eu/cqrxs/console/CryptConsole.java 

debug: cqrxs
	echo "launching eu/cqrxs/gui/CqrJdFrame.java --verbose"
	sleep 1
	java $(JAVAFLAGS) -cp $(MYCLASSPATH) eu/cqrxs/gui/CqrJdFrame.java --debug

verbose: cqrxs
	echo "launching eu/cqrxs/gui/CqrJdFrame.java --verbose"
	sleep 1
	java $(JAVAFLAGS) -cp $(MYCLASSPATH) eu/cqrxs/gui/CqrJdFrame.java --verbose


jar: cqrxs console
	echo "building eu.cqrxs.jar " 
	jar --create --file eu.cqrxs.jar --main-class eu.cqrxs.gui.CqrJdFrame @classes.list

# clean cleans client6 server6_clone 
clean: 
	echo "cleaning classes from last build in eu/cqrxs/ eu/cqrxs/cqrframe/ eu/cqrxs/gui/ "
	rm -rf *.class eu.cqrxs.jar

package eu.cqrxs.crypt.cipher;

import eu.cqrxs.crypt.encoding.EncodeEnum;
import eu.cqrxs.crypt.hash.KeyHash;
import eu.cqrxs.util.CException;
import eu.cqrxs.util.Constants;
import eu.cqrxs.zip.ZipType;
import org.bouncycastle.crypto.InvalidCipherTextException;

import java.io.IOException;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

/**
 * SecureCipherPipe is symmetric block cipher encryption and decryption pipe line
 */
public class SecureCipherPipe extends CipherPipe {

    String cipherKeyHash = "";

	final static KeyHash[] secureHashes = {
			KeyHash.BCrypt, KeyHash.Blake2xs,  KeyHash.CShake, KeyHash.Dstu7564,
			KeyHash.OpenBSDCrypt, KeyHash.SCrypt, KeyHash.RipeMD256, KeyHash.Whirlpool };

    /**
     * parameterless constructor of SecureCipherPipe
     */
    public SecureCipherPipe() {
        cipherKeyHash = "";
        inPipe = new CipherEnum[0];
        encodeType = EncodeEnum.Base64;
        zType = ZipType.GZip;
        cMode2 = CipherMode2.CFB;
    }

    /**
     * SecureCipherPipe constructor with following parameters
     * @param cipherEnums an array of {@link CipherEnum}
     * @param maxpipe maximum pipeline size {@link eu.cqrxs.util.Constants}.MAX_PIPE_LEN
     * @param cmode2 {@link CipherMode2}
     */
    public SecureCipherPipe(CipherEnum[] cipherEnums, int maxpipe, CipherMode2 cmode2) {

        // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
        maxpipe = ((maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe); // if somebody wants more, he/she/it gets less

        int isize = Math.min(((int)cipherEnums.length), ((int)maxpipe));
        inPipe = new CipherEnum[isize];
        for (int ib = 0; (ib < cipherEnums.length && ib < isize); ib++) {
            inPipe[ib] = cipherEnums[ib];
        }
        // System.arraycopy(cipherEnums, 0, inPipe, 0, isize);

        cMode2 = cmode2;
        encodeType = EncodeEnum.Base64;
        zType = ZipType.GZip;
    }

    /**
     *  SecureCipherPipe constructor with an array of [@link String[]} cipherAlgos as inpipe
     *
     * @param cipherAlgos array of [@link String[]} as inpipe
     * @param maxpipe maximum length {@link Constants}.MAX_PIPE_LEN
     * @param cmode2 {@link CipherMode2}
     * */
    public SecureCipherPipe(String[] cipherAlgos, int maxpipe, CipherMode2 cmode2) {

        // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
        maxpipe = ((maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe); // if somebody wants more, he/she/it gets less

        List<CipherEnum> cipherEnums = new ArrayList<CipherEnum>();
        int cnt = 0;
        for (String algo : cipherAlgos) {
            if (algo != null && algo.length() > 0) {
                CipherEnum cipherAlgo = CipherEnum.Aes;
                cipherAlgo = CipherEnum.valueOf(algo);
                cipherEnums.add(cipherAlgo);

                if (++cnt > maxpipe)
                    break;
            }
        }

        inPipe = cipherEnums.toArray(CipherEnum[]::new);
        cMode2 = cmode2;
        encodeType = EncodeEnum.Base64;
        zType = ZipType.GZip;
    }


    /**
     * SecureCipherPipe ctor with array of user key bytes
     * @param keyBytes user key bytes
     * @param maxpipe maximum length {@link Constants}.MAX_PIPE_LEN
     * @param cmode2 {@link CipherMode2}
     */
    public SecureCipherPipe(byte[] keyBytes, int maxpipe, CipherMode2 cmode2) {
        // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
        maxpipe = ((maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe); // if somebody wants more, he/she/it gets less

        short scnt = 0;
        List<CipherEnum> pipeList = new ArrayList<CipherEnum>();

        HashSet<Byte> hashBytes = new HashSet<Byte>();
        for (int i = 0; i < keyBytes.length && pipeList.size() < maxpipe; i++) {
            byte bb = (byte)((int)((int)keyBytes[i] % 0x1d));
            Byte cb = Byte.valueOf(bb);
            if (!hashBytes.contains(cb)) {
                hashBytes.add(cb);
                CipherEnum cipherEnm = CipherEnum.getByteCipherDict().get(cb);
				System.out.println("keybyts[" + i + "]="+ keyBytes[i] + " byte bb = " + (int)bb + " CipherEnum: " + cipherEnm.getName());
                pipeList.add(cipherEnm);
            }
        }

        try {
            inPipe = new CipherEnum[pipeList.size()];
            inPipe = pipeList.toArray(CipherEnum[]::new);
        } catch (Exception ex) {
            inPipe = new CipherEnum[pipeList.size()];
            for (int ib = 0; ib < pipeList.size(); ib++) {
                inPipe[ib] = pipeList.get(ib);
            }
        }
        cMode2 = cmode2;
        encodeType = EncodeEnum.Base64;
        zType = ZipType.GZip;
    }

    /**
     * Constructs a SecureCipherPipe from key and hash
     * @param key users secret key per default email address
     * @param cmode2 {@link CipherMode2}
     */
    public SecureCipherPipe(String key, CipherMode2 cmode2) {

        this(CryptHelper.getKeyBytesSingle(key, 16), Constants.MAX_PIPE_LEN, cmode2);
        cipherKeyHash = key;
    }

    /**
     * SecureCipherPipe constructor with single users key argument
     * all other parameters are set to default
     * @param key only users secret key
     */
    public SecureCipherPipe(String key) {
        this(key, CipherMode2.CFB);
        cipherKeyHash = key;
    }


    /**
     * encryptBytesFast generic encrypt bytes to bytes
     * @param inBytes array of bytes
     * @param cipherAlgo {@link CipherEnum}
     * @param hashKey users secret key for encryption
     * @param cmode2 {@link CipherMode2}
     * @return byte array of encrypted bytes
     */
    public static byte[] encryptBytesFast(byte[] inBytes, CipherEnum cipherAlgo, String hashKey, CipherMode2 cmode2)
                                throws InvalidCipherTextException {

        if (hashKey == null || hashKey.length() < 1)
            throw new IllegalArgumentException("hashKey");

        byte[] encryptBytes = inBytes;
        CryptParams cpParams = new CryptParams(cipherAlgo, hashKey, hashKey, cmode2);

        switch (cipherAlgo) {
            /*
            case CipherEnum.AesNet:
                AesNet aesNet = new AesNet(secretKey, hash);
                encryptBytes = aesNet.Encrypt(inBytes);
            break;
            case CipherEnum.Des3Net:
                Des3Net des3 = new Des3Net(secretKey, hash);
                encryptBytes = des3.Encrypt(inBytes);
                break;             
            case CipherEnum.Rsa:
                AsymmetricCipherKeyPair keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                encryptBytes = Asymmetric.Rsa.Encrypt(inBytes, keyPair);
                break;
            */
            case ZenMatrix:
                encryptBytes = (new ZenMatrix(hashKey, hashKey, false, KeyHash.Hex)).encrypt(inBytes, true);
                break;
            case ZenMatrix3:
                encryptBytes = (new ZenMatrix3(hashKey, hashKey, false)).encrypt(inBytes, true);
                break;
            default:
                CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(cpParams, true);
                encryptBytes = cryptBounceCastle.encrypt(inBytes);
                break;
        }

        return encryptBytes;
    }


    /**
     * Generic decrypt bytes to bytes
     * @param cipherBytes enrypted bytes
     * @param cipherAlgo {@link CipherEnum}
     * @param hashKey users secret key
     * @param cmode2 {@link CipherMode2}
     * @return decrypted bytes for one cipher algo
     */
    public static byte[] decryptBytesFast(byte[] cipherBytes, CipherEnum cipherAlgo, String hashKey, CipherMode2 cmode2)
                            throws InvalidCipherTextException {

        if (hashKey == null || hashKey.length() == 0)
            throw new IllegalArgumentException("hashKey");

        byte[] decryptBytes = cipherBytes; 
        CryptParams cpParams = new CryptParams(cipherAlgo, hashKey, hashKey, cmode2);

        switch (cipherAlgo) {
            /*
            case CipherEnum.AesNet:
                AesNet aesNet = new AesNet(secretKey, hash);
                decryptBytes = aesNet.Decrypt(cipherBytes);
                break;
            case CipherEnum.Des3Net:
                Des3Net des3 = new Des3Net(secretKey, hash);
                decryptBytes = des3.Decrypt(cipherBytes);
                break
            case CipherEnum.Rsa:
                AsymmetricCipherKeyPair keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                decryptBytes = Asymmetric.Rsa.DecryptWithPrivate(cipherBytes, keyPair);
                break;
            */
            case ZenMatrix:
                decryptBytes = (new ZenMatrix(hashKey, hashKey, false, KeyHash.Hex)).decrypt(cipherBytes);
                break;
            case ZenMatrix3:
                decryptBytes = (new ZenMatrix3(hashKey, hashKey, false)).decrypt(cipherBytes);
                break;
            default:
                CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(cpParams, true);
                decryptBytes = cryptBounceCastle.decrypt(cipherBytes);
                break;
        }

        return decryptBytes; // TODO: EnDeCodeHelper.GetBytesTrimNulls(decryptBytes);
    }


    /**
     * merryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
     * @param inBytes plain byte[] to encrypt
     * @param hashKey user secret key to use for all symmetric cipher algorithms in the pipe
     * @param cmode2 {@link CipherMode2}
     * @return encrypted byte[]
     */
    public byte[] merryGoRoundEncrpyt(byte[] inBytes, String hashKey, CipherMode2 cmode2)
                    throws InvalidCipherTextException {

        if (inPipe.length == 0)
            return inBytes;

        if ((hashKey == null && cipherKeyHash == null) || (hashKey.length() == 0 && cipherKeyHash.length() == 0))
            throw new IllegalArgumentException("hashKey");

        int merry = 0;
        byte[] encryptedBytes = new byte[inBytes.length];
        System.arraycopy(inBytes, 0, encryptedBytes, 0, inBytes.length);
        cipherKeyHash = (hashKey != null && hashKey.length() > 0) ? hashKey : cipherKeyHash;

        for (CipherEnum cipher : inPipe) {

            String cipherHashKey = secureHashes[(merry % secureHashes.length)].hash(cipherKeyHash);
            if ((++merry) > (secureHashes.length -1)) merry = 0;

            encryptedBytes = encryptBytesFast(inBytes, cipher, cipherHashKey, cmode2); 
            inBytes = encryptedBytes;
        }

        return encryptedBytes;
    }

    /**
     * decrpytRoundGoMerry against clock turn -
     *    starts merry to turn arround from right to left against clock hour cycle
     * @param cipherBytes encrypted byte array
     * @param hashKey user secret key, normally email address
     * @param cmode2 {@link CipherMode2}
     * @return byte[]
     */
    public byte[] decrpytRoundGoMerry(byte[] cipherBytes, String hashKey, CipherMode2 cmode2) 
                    throws InvalidCipherTextException {

        if (inPipe.length == 0)
            return cipherBytes;

        if ((hashKey == null && cipherKeyHash == null) || (hashKey.length() == 0 && cipherKeyHash.length() == 0))
            throw new IllegalArgumentException("hashKey");

        cipherKeyHash = (hashKey != null && !hashKey.isEmpty()) ? hashKey : cipherKeyHash;        
        int roundsGo = secureHashes.length - 1;

        byte[] decryptedBytes = new byte[cipherBytes.length];
        for (CipherEnum cipher : getOutPipe()) {

            String cipherHashKey = secureHashes[(roundsGo % secureHashes.length)].hash(cipherKeyHash);
            if ((--roundsGo) < 0) roundsGo = secureHashes.length - 1;

            decryptedBytes = decryptBytesFast(cipherBytes, cipher, cipherHashKey, cmode2);
            cipherBytes = decryptedBytes;
        }

        return decryptedBytes;
    }

    /**
     * EncrpytTextGoRounds encrypts text with cipher pipe pipeline
     * @param inString plain text to encrypt
     * @param hashKey private hash key
     * @param cmode2 {@link CipherMode2}
     * @return UTF8 encoded encrypted String without binary data
     */
    public String encrpytTextGoRounds(String inString, String hashKey, CipherMode2 cmode2)
                                throws InvalidCipherTextException, IOException {

        cipherKeyHash = (hashKey != null && !hashKey.isEmpty()) ? hashKey : cipherKeyHash;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;
        cMode2 = cmode2;

        // Transform String to bytes
        byte[] inBytes = inString.getBytes(StandardCharsets.UTF_8);

		try {
            byte[] zippedBytes = (zType != ZipType.None) ? zType.zip(inBytes) : inBytes;
            inBytes = zippedBytes;
        } catch (Exception exZip) {
            exZip.printStackTrace();
        }
        // use EncrpytFileBytesGoRounds for operations zip before and pipe cycöe encryption
        byte[] encryptedBytes = merryGoRoundEncrpyt(inBytes, cipherKeyHash, cMode2);                

        // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
        String encrypted = encodeType.encodeBytesToString(encryptedBytes);

        return encrypted;
    }


    /**
     * encrpytFileBytesGoRounds encrypts a data byte[] array
     * @param inBytes binary data
     * @param hashKey private key to be hashed in symmetric merry go round karusell with 8 different hash algos
     * @param cmode2 {@link CipherMode2}
     * @return binary data
     */
    public byte[] encrpytFileBytesGoRounds(byte[] inBytes, String hashKey, CipherMode2 cmode2)
                                throws InvalidCipherTextException {

        cipherKeyHash = (hashKey != null && !hashKey.isEmpty()) ? hashKey : cipherKeyHash;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;
        cMode2 = cmode2;

        try {
            byte[] zippedBytes = (zType != ZipType.None) ? zType.zip(inBytes) : inBytes;
            inBytes = zippedBytes;
        } catch (Exception exZip) {
            exZip.printStackTrace();
        }
        // perform multi crypt pipe stages
        byte[] encryptedBytes = merryGoRoundEncrpyt(inBytes, cipherKeyHash, cMode2);

        return encryptedBytes;
    }


	/**
     * decryptTextRoundsGo
     * @param cryptedEncodedMsg encoded byte array
	 * @param hashKey  Unique deterministic key which will be hashed at each stage of with a different secure hash {@link KeyHash}.secureHashes
     * @param cmode2 {@link CipherMode2}
     * @return plain bytes
     * @throws InvalidCipherTextException from bouncy-castle
	 * @throws IOException Input/OutputException
     */
    public String decryptTextRoundsGo(String cryptedEncodedMsg, String hashKey, CipherMode2 cmode2)
                            throws InvalidCipherTextException, IOException {

        cipherKeyHash = (hashKey != null && !hashKey.isEmpty()) ? hashKey : cipherKeyHash;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;
        cMode2 = cmode2;

        byte[] cipherBytes = encodeType.decodeStringToBytes(cryptedEncodedMsg);

        // perform multi crypt pipe stages
        byte[] intermediatBytes = decrpytRoundGoMerry(cipherBytes, cipherKeyHash, cMode2);

		// Unzip after all, if it's necessary
		byte[] decryptedBytes = (zType != ZipType.None) ? zType.unzip(intermediatBytes) : intermediatBytes;

        // Get String from decrypted bytes
        String decrypted = (inPipe.length == 0) ?
                    new String(decryptedBytes, "UTF8") :
                new String(decryptedBytes, StandardCharsets.UTF_8);

        // find first \0 = NULL char in String and truncate all after first \0 apperance in String
		for (int ix = 0; ix < decrypted.length(); ix++) {
		 	if (decrypted.charAt(ix) == '\0' && ix > 0) {
		 		decrypted = decrypted.substring(0, ix);
		 		break; 
		 	}
		}

        return decrypted;
    }

    /**
     *  decodeDecrpytBytes
     * @param cipherBytes encoded byte array
	 * @param hashKey Unique deterministic key, which will be hashed with a different {@link KeyHash} secure hash at each stage of the pipe
     *                to generate a secure hashed key
     * @param cmode2 {@link CipherMode2}
     * @return plain bytes
     * @throws InvalidCipherTextException
     */
    public byte[] decryptFileBytesRoundsGo(byte[] cipherBytes, String hashKey, EncodeEnum decoding, ZipType unzipAfter, CipherMode2 cmode2)
                                    throws InvalidCipherTextException  {

        cipherKeyHash = (hashKey == null || hashKey.length() == 0) ? hashKey : cipherKeyHash;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;
        cMode2 = cmode2;

        // perform multi crypt pipe stages
        byte[] decryptedBytes = decrpytRoundGoMerry(cipherBytes, cipherKeyHash, cmode2);

        try {
            byte[] unzipBytes = (unzipAfter != zType.None) ?
                    zType.unzip(decryptedBytes) : decryptedBytes;
            decryptedBytes = unzipBytes;
        } catch (Exception exUnzip) {
            exUnzip.printStackTrace();
        }

        return decryptedBytes;
    }

	/**
     * encrpytGoRounds encrypts a data byte[] array
     * @param inBytes binary data
     * @param hashKey prviate key for encryption, will be hashed to a different hashKey at each stage of pipe
     * @param cmode2 {@link CipherMode2}
     * @return encrypted binary data bytes
     */
    public byte[] encrpytGoRounds(byte[] inBytes, String hashKey, CipherMode2 cmode2)
                            throws InvalidCipherTextException {

        cipherKeyHash = (hashKey != null && hashKey.length() > 0) ? hashKey : cipherKeyHash;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;
        cMode2 = cmode2;

        try {
            byte[] zippedBytes = (zType != ZipType.None) ? zType.zip(inBytes) : inBytes;
            inBytes = zippedBytes;
        } catch (Exception exZip) {
            exZip.printStackTrace();
        }

        return merryGoRoundEncrpyt(inBytes, cipherKeyHash, cMode2);
    }


	/**
     * decrpytRoundsGo decrypts encrypted bytes
     * @param cipherBytes encrypted binary data
     * @param hashKey prviate key for encryption
     * @param cmode2 {@link CipherMode2}
     * @return decrypted bytes
     */
    public byte[] decrpytRoundsGo(byte[] cipherBytes, String hashKey, ZipType unzipAfter, CipherMode2 cmode2)
            throws InvalidCipherTextException {
        
        cipherKeyHash = (hashKey != null && hashKey.length() > 0) ? hashKey : cipherKeyHash;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;
        cMode2 = cmode2;
        byte[] decryptedBytes = decrpytRoundGoMerry(cipherBytes, cipherKeyHash, cMode2);
        try {
            byte[] unzipBytes = (zType != ZipType.None) ?
                    zType.unzip(decryptedBytes) : decryptedBytes;
            decryptedBytes = unzipBytes;
        } catch (Exception exUnzip) {
            exUnzip.printStackTrace();
        }
        return decryptedBytes;
    }

    @Deprecated
    public String encrpytEncode(byte[] inBytes, String hashKey, CipherMode2 cmode2)
                        throws InvalidCipherTextException, IOException {

        cipherKeyHash = (hashKey != null && hashKey.length() > 0) ? hashKey : cipherKeyHash;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;
        cMode2 = cmode2;

        try {
            byte[] zippedBytes = (zType != ZipType.None) ? zType.zip(inBytes) : inBytes;
            inBytes = zippedBytes;
        } catch (Exception exZip) {
            exZip.printStackTrace();
        }

        byte[] outBytes = merryGoRoundEncrpyt(inBytes, cipherKeyHash, cMode2);

        String cryptedEncoded = encodeType.encodeBytesToString(outBytes);
        return cryptedEncoded;
    }


    /**
     *  encryptEncodeBytes
     * @param inBytes String to encrypt multiple times
     * @param hashKey Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
     *     /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe
     * @param cmode2 {@link CipherMode2}
     * @return encrypted byte array
     * @throws InvalidCipherTextException
     * @throws IllegalArgumentException
	 * @throws IOException
     */
    public byte[] encryptEncodeBytes(byte[] inBytes, String hashKey, CipherMode2 cmode2)
            throws InvalidCipherTextException, IOException {

        cipherKeyHash = (hashKey != null && hashKey.length() > 0) ? hashKey : cipherKeyHash;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;
        cMode2 = cmode2;

        try {
            byte[] zippedBytes = (zType != ZipType.None) ? zType.zip(inBytes) : inBytes;
            inBytes = zippedBytes;
        } catch (Exception exZip) {
            exZip.printStackTrace();
        }

        byte[] outBytes = merryGoRoundEncrpyt(inBytes, cipherKeyHash, cMode2);

        byte[] encryptedBytes = new byte[0];
        if (encodeType != EncodeEnum.None)
        {
            String cryptedEncoded = encodeType.encodeBytesToString(outBytes);
            encryptedBytes = cryptedEncoded.getBytes(Charset.forName("UTF-8"));
        }
        else
            encryptedBytes = outBytes;

        return encryptedBytes;
    }


    @Deprecated
    public byte[] decodeDecrpyt(String encoded, String hashKey, CipherMode2 cmode2)
                        throws InvalidCipherTextException, IOException {

        cipherKeyHash = (hashKey != null && !hashKey.isEmpty()) ? hashKey : cipherKeyHash;
        cMode2 = cmode2;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;

        byte[] cipherBytes = encodeType.decodeStringToBytes(encoded);
        byte[] outBytes = decrpytRoundGoMerry(cipherBytes, cipherKeyHash, cMode2);

        try {
            byte[] unzipBytes = (zType != ZipType.None) ?
                    zType.unzip(outBytes) : outBytes;
            outBytes = unzipBytes;
        } catch (Exception exUnzip) {
            exUnzip.printStackTrace();
        }
        return outBytes;
    }


    /**
     *  decodeDecrpytBytes
     * @param encodedBytes encoded byte array
     * @param hashKey Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
     *      	and unique crypt key for each symmetric cipher algorithm in each stage of the pipe
     * @param cmode2 {@link CipherMode2}
     * @return plain bytes
     * @throws InvalidCipherTextException
     * @throws IllegalArgumentException
	 * @throws IOException
     */
    public byte[] decodeDecrpytBytes(byte[] encodedBytes, String hashKey, CipherMode2 cmode2)
                                throws InvalidCipherTextException, IOException {

        cipherKeyHash = (hashKey != null && hashKey.length() > 0) ? hashKey : cipherKeyHash;
        zType = ZipType.GZip;
        encodeType = EncodeEnum.Base64;
        cMode2 = cmode2;

        byte[] cipherBytes = encodedBytes;
        if (encodeType != EncodeEnum.None)  {
            String encoded =  new String(encodedBytes, StandardCharsets.UTF_8);
            cipherBytes = encodeType.decodeStringToBytes(encoded);
        }

        byte[] outBytes = decrpytRoundGoMerry(cipherBytes, cipherKeyHash, cMode2);

        try {
            byte[] unzipBytes = (zType != ZipType.None) ?
                    zType.unzip(outBytes) : outBytes;
            outBytes = unzipBytes;
        } catch (Exception exUnzip) {
            exUnzip.printStackTrace();
        }

        return outBytes;
    }


    /**
     * cryptCodeBytes encrypt or decrypt bytes
     * @param inBytes bytes to transform
     * @param secretKey user's key
     * @param directionDecrypt true for decrypt, false for encrypt
     * @param cmode2 {@link CipherMode2}
     * @return transformed bytes
     */
    public byte[] cryptCodeBytes(byte[] inBytes, String secretKey,
                                 boolean directionDecrypt, CipherMode2 cmode2) {
        byte[] outBytes;
        try {
            outBytes = (!directionDecrypt) ?
                    encryptEncodeBytes(inBytes, secretKey, cmode2) :
                    decodeDecrpytBytes(inBytes, secretKey, cmode2);
        } catch (Exception exc) {
            throw new CException("Exception in CryptCodeBytes", (Throwable)exc);
        }
        return outBytes;
    }

}

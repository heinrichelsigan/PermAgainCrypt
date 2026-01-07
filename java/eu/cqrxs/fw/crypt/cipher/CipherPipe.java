package eu.cqrxs.fw.crypt.cipher;


// import androidx.core.content.res.TypedArrayUtils;
// import com.google.common.primitives.Bytes;

import java.io.IOException;
import java.util.Arrays;
import java.util.List;
import java.io.ByteArrayOutputStream;
import java.nio.ByteBuffer;
import java.nio.charset.StandardCharsets;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.HexFormat;
import java.util.List;
import org.bouncycastle.crypto.*;
import org.bouncycastle.crypto.engines.*;
import org.bouncycastle.crypto.BlockCipher;

import eu.cqrxs.fw.crypt.encoding.EncodeEnum;
import eu.cqrxs.fw.crypt.encoding.Hex16Coder;
import eu.cqrxs.fw.crypt.hash.KeyHash;
import eu.cqrxs.fw.zip.ZipType;
import eu.cqrxs.fw.zip.*;
import eu.cqrxs.fw.util.Constants;
import eu.cqrxs.fw.util.*;

/**
 * CipherPipe is symmetric block cipher encryption and decryption pipe line
 */
public class CipherPipe {


    String cipherKey = "", cipherHash = "";
    ZipType zType = ZipType.None;
    // private readonly CipherEnum[] inPipe;
    CipherEnum[] inPipe;
    // private readonly CipherEnum[] outPipe;
    EncodeEnum  encodeType = EncodeEnum.Base64;
    KeyHash kHash = KeyHash.Hex;
    // private readonly String pipeString;


    public ZipType getZipType() {
        return zType;
    }

    public EncodeEnum getEncodeType() {
        return encodeType;
    }

    public KeyHash getKeyHash() {
        return kHash;
    }

    public CipherEnum[] getInPipe() {
        return inPipe;
    }


    public CipherEnum[] getOutPipe() {
        CipherEnum[] outEnums = new CipherEnum[inPipe.length];
        int outIdx = 0;
        for (int i = inPipe.length - 1; i >= 0; i--)
            outEnums[outIdx++] = inPipe[i];

        return outEnums;
    }

    public String getPipeString() {
        String pipeString = "";
        for (CipherEnum cipher : inPipe)
            pipeString = pipeString + cipher.getCipherChar();
        return pipeString;
    }

    /**
     * parameterless constructor of CipherPipe
     */
    public CipherPipe() {
        cipherKey = ""; //
        cipherHash = "";
        inPipe = new CipherEnum[0];
        encodeType = EncodeEnum.Base64;
        zType = ZipType.None;
        kHash = KeyHash.Hex;
    }

    /**
     * CipherPipe constructor with following parameters
     * @param cipherEnums an array of {@link CipherEnum}
     * @param maxpipe maximum pipeline size {@link Constants.MAX_PIPE_LEN}
     * @param encType {@link EncodeEnum}
     * @param zpType {@link ZipType}
     * @param kh {@link KeyHash}
     */
    public CipherPipe(CipherEnum[] cipherEnums, int maxpipe, EncodeEnum encType, ZipType zpType, KeyHash kh) {

        // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
        maxpipe = ((maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe); // if somebody wants more, he/she/it gets less

        int isize = Math.min(((int)cipherEnums.length), ((int)maxpipe));
        inPipe = new CipherEnum[isize];
        for (int ib = 0; (ib < cipherEnums.length && ib < isize); ib++) {
            inPipe[ib] = cipherEnums[ib];
        }
        // System.arraycopy(cipherEnums, 0, inPipe, 0, isize);

        encodeType = encType;
        zType = zpType;
        kHash = kh;
    }

    /**
     *  CipherPipe constructor with an array of <see cref="T:String[]"/> cipherAlgos as inpipe
     * @param cipherAlgos array of String[] as inpipe
     * @param maxpipe maximum length {@link Constants.MAX_PIPE_LEN}
     * @param encType {@link EncodeEnum}
     * @param zpType {@link ZipType}
     * @param kh {@link KeyHash}
     * */
    public CipherPipe(String[] cipherAlgos, int maxpipe, EncodeEnum encType, ZipType zpType, KeyHash kh) {
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

        encodeType = encType;
        kHash = kh;
        zType = zpType;
    }


    /**
     * CipherPipe ctor with array of user key bytes
     * @param keyBytes user key bytes
     * @param maxpipe maximum length {@link Constants.MAX_PIPE_LEN}
     * @param encType {@link EncodeEnum}
     * @param zpType {@link ZipType}
     * @param kh {@link KeyHash}
     */
    public CipherPipe(byte[] keyBytes, int maxpipe, EncodeEnum encType, ZipType zpType, KeyHash kh) {
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

        zType = zpType;
        encodeType = encType;
        kHash = kh;

    }

    /**
     * Constructs a CipherPipe from key and hash
     * @param key users secret key per default email address
     * @param hash hashed users secret key
     * @param encType {@link EncodeEnum}
     * @param zpType {@link ZipType}
     * @param kh {@link KeyHash}
     */
    public CipherPipe(String key, String hash, EncodeEnum encType, ZipType zpType, KeyHash kh) {

        this(CryptHelper.getKeyBytesSimple(key, hash, 16), Constants.MAX_PIPE_LEN, encType, zpType, kh);
        cipherKey = key;
        cipherHash = hash;
    }

    /**
     * constructor with single users key argument
     * all other parameters are set to default
     * @param key only users secret key
     */
    public CipherPipe(String key) {
        this(key, KeyHash.Hex.hash(key), EncodeEnum.Base64, ZipType.None, KeyHash.Hex);
        cipherKey = key;
    }
    /*
        /// <summary>
        /// ToJson
        /// </summary>
        /// <returns>serialized String</returns>
    public String ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="json">serialized json</param>
        /// <returns><see cref="CipherPipe"/></returns>
        public CipherPipe FromJson(String json)
        {
            CipherPipe pipe = JsonConvert.DeserializeObject<CipherPipe>(json);
            if (pipe == null)
            {
                this.inPipe = pipe.InPipe;
                this.encodeType = pipe.EncodeType;
                this.kHash = pipe.KHash;
                this.zType = pipe.ZType;
                this.cipherKey = pipe.cipherKey;
                this.cipherHash = pipe.cipherHash;
            }
            return pipe;
        }
        */


    /**
     *  Generic encrypt bytes to bytes
     * @param inBytes array of bytes
     * @param cipherAlgo {@link CipherEnum}
     * @param secretKey users secret key for encryption
     * @param hash users key hashed
     * @return byte array of encrypted bytes
     */
    public static byte[] encryptBytesFast(byte[] inBytes, CipherEnum cipherAlgo, String secretKey, String hashedKey)
        throws InvalidCipherTextException {
        if (secretKey == null || secretKey.length() < 1)
            throw new IllegalArgumentException("seretkey");
        if (hashedKey == null || hashedKey.length() == 0)
            throw new IllegalArgumentException("hashedKey");

        byte[] encryptBytes = inBytes;
        CryptParams cpParams = new CryptParams(cipherAlgo, secretKey, hashedKey);

        switch (cipherAlgo)
        {
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
                encryptBytes = (new ZenMatrix(secretKey, hashedKey, false, KeyHash.Hex)).encrypt(inBytes);
                break;
            // case CipherEnum.ZenMatrix2:
            //     encryptBytes = (new ZenMatrix2(secretKey, hash, false)).Encrypt(inBytes);
            //     break;
            case Aes:
            case AesNet:
            case AesLight:
            case Aria:
            case BlowFish:
            case Camellia:
            case Cast5:
            case Cast6:
            case Des:
            case Des3:
            case Dstu7624:
            case Fish2:
            case Fish3:
            // case ThreeFish256:
            case Gost28147:
            case Idea:
            case Noekeon:
            case RC2:
            case RC532:
			case RC564:
            case RC6:
            case Rijndael:
            case Seed:
            case Serpent:
            case SM4:
            case SkipJack:
            case Tea:
            case Tnepres:
            case XTea:
            // case ZenMatrix:
            // case ZenMatrix2:
            default:
                CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(cpParams, true);
                encryptBytes = cryptBounceCastle.encrypt(inBytes);
                // TODO: full port standard bouncycastle wrapper to java
                // TODO: compare and test with C#
                break;
        }

        return encryptBytes;
    }


    /**
     * Generic decrypt bytes to bytes
     * @param cipherBytes enrypted bytes
     * @param cipherAlgo {@link CipherEnum}
     * @param secretKey users secret key
     * @param hash users key hashed
     * @return decrypted bytes for one cipher algo
     */
    public static byte[] decryptBytesFast(byte[] cipherBytes, CipherEnum cipherAlgo, String secretKey, String hash)
            throws InvalidCipherTextException {
        if (secretKey == null || secretKey.length() == 0)
            throw new IllegalArgumentException("seretkey");
        if (hash == null || hash.length() == 0)
            throw new IllegalArgumentException("hash");
        // bool sameKey = true;

        byte[] decryptBytes = cipherBytes; 
        CryptParams cpParams = new CryptParams(cipherAlgo, secretKey, hash);

        switch (cipherAlgo)
        {
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
                decryptBytes = (new ZenMatrix(secretKey, hash, false, KeyHash.Hex)).decrypt(cipherBytes);
                break;
            // case ZenMatrix2:
            //     decryptBytes = (new ZenMatrix2(secretKey, hash, false)).Decrypt(cipherBytes);
            //     break;
            case Aes:
            case AesLight:
            case Aria:
            case BlowFish:
            case Camellia:
            case Cast5:
            case Cast6:
            case Des:
            case Des3:
            case Dstu7624:
            case Fish2:
            case Fish3:
            case Gost28147:
            case Idea:
            case Noekeon:
            case RC2:
            case RC532:
            case RC564:
            case RC6:
            case Rijndael:
            case Seed:
            case Serpent:
            case SM4:
            case SkipJack:
            case Tea:
            case Tnepres:
            case XTea:
            // case ZenMatrix:
            // case ZenMatrix2:
            default:
                CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(cpParams, true);
                decryptBytes = cryptBounceCastle.decrypt(cipherBytes);
                // TODO: full port standard bouncycastle wrapper to java
                // TODO: compare and test with C#
                break;
        }


        return decryptBytes; // TODO: EnDeCodeHelper.GetBytesTrimNulls(decryptBytes);
    }


    /**
     * merryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
     * @param inBytes plain byte[] to encrypt
     * @param secretKey user secret key to use for all symmetric cipher algorithms in the pipe
     * @param hashIv hash key iv relational to secret key
     * @param zipBefore {@link ZipType }
     * @return encrypted byte[]
     */
    public byte[] merryGoRoundEncrpyt(byte[] inBytes, String secretKey, String hashIv, ZipType zipBefore)
            throws InvalidCipherTextException {
        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");

        String hash = (hashIv != null && hashIv.length() > 0) ? hashIv : (kHash != null) ? kHash.hash(secretKey) : KeyHash.Hex.hash(secretKey);
        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = hash;

        byte[] encryptedBytes = new byte[inBytes.length];
        System.arraycopy(inBytes, 0, encryptedBytes, 0, inBytes.length);
        if (zipBefore != ZipType.None)
        {
            try {
			    encryptedBytes = zipBefore.zip(inBytes);
            	inBytes = encryptedBytes;
		    } catch (Exception exZip) {
			    exZip.printStackTrace();
		    }
        }

        for (CipherEnum cipher : inPipe)
        {
            encryptedBytes = encryptBytesFast(inBytes, cipher, cipherKey, cipherHash);
            inBytes = encryptedBytes;
        }

        return encryptedBytes;
    }

    /**
     * decrpytRoundGoMerry against clock turn -
     *    starts merry to turn arround from right to left against clock hour cycle
     * @param cipherBytes encrypted byte array
     * @param secretKey user secret key, normally email address
     * @param hashIv hash relational to secret key
     * @param unzipAfter {@link ZipType}
     * @return byte[]
     */
    public byte[] decrpytRoundGoMerry(byte[] cipherBytes, String secretKey, String hashIv, ZipType unzipAfter)
            throws InvalidCipherTextException {
        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");

        String hash = (hashIv != null && hashIv.length() > 0) ? hashIv : (kHash != null) ? kHash.hash(secretKey) : KeyHash.Hex.hash(secretKey);
        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = hash;


        byte[] decryptedBytes = new byte[cipherBytes.length];
        if (getOutPipe() == null || getOutPipe().length == 0)
            System.arraycopy(cipherBytes, 0, decryptedBytes, 0, cipherBytes.length);
        else
            for (CipherEnum cipher : getOutPipe())
            {
                decryptedBytes = decryptBytesFast(cipherBytes, cipher, cipherKey, cipherHash);
                cipherBytes = decryptedBytes;
            }

        if (unzipAfter != ZipType.None) {
			try {
				decryptedBytes = unzipAfter.unzip(cipherBytes);
			} catch (Exception exUnzip) {
				exUnzip.printStackTrace();
			}
		}

        return decryptedBytes;
    }

    /**
     * EncrpytTextGoRounds encrypts text with cipher pipe pipeline
     * @param inString plain text to encrypt
     * @param cryptKey prviate key for encryption
     * @param hashIv private hash for encryption
     * @param encoding {@link EncodeEnum}
     * @param zipBefore {@link ZipType}
     * @param keyHash {@link KeyHash}
     * @return UTF8 encoded encrypted String without binary data
     */
    public String encrpytTextGoRounds(
            String inString,
            String cryptKey,
            String hashIv,
            EncodeEnum encoding,
            ZipType zipBefore,
            KeyHash keyHash) throws InvalidCipherTextException, IOException
    {
        // Transform String to bytes
        byte[] inBytes = inString.getBytes(StandardCharsets.UTF_8);

        // use EncrpytFileBytesGoRounds for operations zip before and pipe cycöe encryption
        byte[] encryptedBytes = encrpytFileBytesGoRounds(inBytes, cryptKey, hashIv, encoding, zipBefore, keyHash);

        // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
        String encrypted = encoding.encodeBytesToString(encryptedBytes);

        return encrypted;
    }


    /**
     * encrpytFileBytesGoRounds encrypts a data byte[] array
     * @param inBytes binary data
     * @param cryptKey prviate key for encryption
     * @param hashIv hashed private key
     * @param encoding {@link EncodeEnum}
     * @param zipBefore {@link ZipType}
     * @param keyHash {@link KeyHash}
     * @return binary data
     */
    public byte[] encrpytFileBytesGoRounds(
            byte[] inBytes,
            String cryptKey,
            String hashIv,
            EncodeEnum encoding,
            ZipType zipBefore,
            KeyHash keyHash) throws InvalidCipherTextException
    {
        // hashIv if empty hash secretKey with keyHash hashing variant
        hashIv = (hashIv == null || hashIv.length() == 0) ? keyHash.hash(cryptKey) : hashIv;
        cipherKey = cryptKey;
        cipherHash = hashIv;
        kHash = keyHash;
        zType = zipBefore;
        encodeType = encoding;

        // perform multi crypt pipe stages
        byte[] encryptedBytes = merryGoRoundEncrpyt(inBytes, cryptKey, hashIv, zipBefore);

        return encryptedBytes;
    }


	/**
     *  decryptTextRoundsGo
     * @param cryptedEncodedMsg encoded byte array
     * @param cryptKey Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
     *      	and unique crypt key for each symmetric cipher algorithm in each stage of the pipe
	 * @param hashIV key hash
     * @param decoding {@link EncodeEnum} type for encoding encrypted bytes back in plain text
     * @param unzipAfter zip bytes with {@link ZipType}
     * @param keyHash {@link KeyHash} hashing enum => use hash(...) for hashing
     * @return plain bytes
     * @throws InvalidCipherTextException
	 * @throws IOException
     */
    public String decryptTextRoundsGo(
            String cryptedEncodedMsg,
            String cryptKey,
            String hashIv,
            EncodeEnum decoding,
            ZipType unzipAfter,
            KeyHash keyHash) throws InvalidCipherTextException, IOException
    {

        byte[] cipherBytes = decoding.decodeStringToBytes(cryptedEncodedMsg);

        // perform multi crypt pipe stages
        byte[] decryptedBytes = decryptFileBytesRoundsGo(cipherBytes, cryptKey, hashIv, decoding, unzipAfter, keyHash);

        // Get String from decrypted bytes
        String decrypted = (inPipe.length == 0) ?
                new String(cipherBytes, "UTF8") :
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
     * @param cryptKey Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
     *      	and unique crypt key for each symmetric cipher algorithm in each stage of the pipe
	 * @param hashIV key hash
     * @param decoding {@link EncodeEnum} type for encoding encrypted bytes back in plain text
     * @param unzipAfter zip bytes with {@link ZipType}
     * @param keyHash {@link KeyHash} hashing enum => use hash(...) for hashing
     * @return plain bytes
     * @throws InvalidCipherTextException
     */
    public byte[] decryptFileBytesRoundsGo(
            byte[] cipherBytes,
            String cryptKey,
            String hashIv,
            EncodeEnum decoding,
            ZipType unzipAfter,
            KeyHash keyHash) throws InvalidCipherTextException
    {
        // hashIv if empty hash secretKey with keyHash hashing variant
        hashIv = (hashIv == null || hashIv.length() == 0) ? keyHash.hash(cryptKey) : hashIv;
        cipherKey = cryptKey;
        cipherHash = hashIv;
        kHash = keyHash;
        zType = unzipAfter;
        encodeType = decoding;

        // perform multi crypt pipe stages
        byte[] decryptedBytes = decrpytRoundGoMerry(cipherBytes, cryptKey, hashIv, unzipAfter);

        return decryptedBytes;
    }

	/**
     * encrpytGoRounds encrypts a data byte[] array
     * @param inBytes binary data
     * @param secretKey prviate key for encryption
     * @param zipBefore {@link ZipType}
     * @param keyHash {@link KeyHash}
     * @return encrypted binary data bytes
     */
    public byte[] encrpytGoRounds(byte[] inBytes, String secretKey, ZipType zipBefore, KeyHash keyHash)
            throws InvalidCipherTextException {
        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");

        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = keyHash.hash(secretKey);
        zType = zipBefore;
        kHash = keyHash;
        return merryGoRoundEncrpyt(inBytes, secretKey, cipherHash, zipBefore);
    }


	/**
     * decrpytRoundsGo decrypts encrypted bytes
     * @param cipherBytes encrypted binary data
     * @param secretKey prviate key for encryption
     * @param unzipAfter {@link ZipType}
     * @param keyHash {@link KeyHash}
     * @return decrypted bytes
     */
    public byte[] decrpytRoundsGo(byte[] cipherBytes, String secretKey, ZipType unzipAfter, KeyHash keyHash)
            throws InvalidCipherTextException {
        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");
        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = keyHash.hash(secretKey);
        zType = unzipAfter;
        kHash = keyHash;
        return decrpytRoundGoMerry(cipherBytes, secretKey, keyHash.hash(secretKey), unzipAfter);
    }


    public String encrpytEncode(byte[] inBytes, String secretKey, EncodeEnum encType, ZipType zipBefore, KeyHash keyHash)
            throws InvalidCipherTextException, IOException {

        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");
        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = keyHash.hash(secretKey);
        encodeType = encType;
        zType = zipBefore;
        kHash = keyHash;
        byte[] outBytes = merryGoRoundEncrpyt(inBytes, secretKey, cipherHash, zipBefore);
        String cryptedEncoded = encType.encodeBytesToString(outBytes);
        return cryptedEncoded;
    }

    /**
     *  encryptEncodeBytes
     * @param inBytes String to encrypt multiple times
     * @param secretKey Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
     *     /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe
	 * @param hashIV key hash	 
     * @param encType {@link EncodeEnum} type for encoding encrypted bytes back in plain text
     * @param zipBefore zip bytes with {@link ZipType}
     * @param keyHash {@link KeyHash} hashing enum => use hash(...) for hashing
     * @return encrypted byte array
     * @throws InvalidCipherTextException
     * @throws IllegalArgumentException
	 * @throws IOException
     */
    public byte[] encryptEncodeBytes(byte[] inBytes, String secretKey, String hashIV, EncodeEnum encType, ZipType zipBefore, KeyHash keyHash)
            throws InvalidCipherTextException, IOException {

        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");

        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        String hash = (hashIV != null && hashIV.length() > 0) ? hashIV : (kHash != null) ? kHash.hash(secretKey) : KeyHash.Hex.hash(secretKey);
        cipherHash = hash;

        encodeType = encType;
        zType = zipBefore;
        kHash = keyHash;

        byte[] outBytes = merryGoRoundEncrpyt(inBytes, secretKey, cipherHash, zipBefore);
        byte[] encryptedBytes = new byte[0];
        if (encType != EncodeEnum.None)
        {
            String cryptedEncoded = encType.encodeBytesToString(outBytes);
            encryptedBytes = cryptedEncoded.getBytes(Charset.forName("UTF-8"));
        }
        else
            encryptedBytes = outBytes;


        return encryptedBytes;
    }



    public byte[] decodeDecrpyt(String encoded, String secretKey, EncodeEnum encType, ZipType unzipAfter, KeyHash keyHash)
            throws InvalidCipherTextException, IOException {

        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");

        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = keyHash.hash(secretKey);
        encodeType = encType;
        zType = unzipAfter;
        kHash = keyHash;
        byte[] cipherBytes = encodeType.decodeStringToBytes(encoded);
        byte[] outBytes = decrpytRoundGoMerry(cipherBytes, secretKey, keyHash.hash(secretKey), unzipAfter);

        return outBytes;
    }

    /**
     *  decodeDecrpytBytes
     * @param encodedBytes encoded byte array
     * @param cryptKey Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
     *      	and unique crypt key for each symmetric cipher algorithm in each stage of the pipe
	 * @param hashIV key hash
     * @param encType {@link EncodeEnum} type for encoding encrypted bytes back in plain text
     * @param unzipAfter zip bytes with {@link ZipType}
     * @param keyHash {@link KeyHash} hashing enum => use hash(...) for hashing
     * @return plain bytes
     * @throws InvalidCipherTextException
     * @throws IllegalArgumentException
	 * @throws IOException
     */
    public byte[] decodeDecrpytBytes(byte[] encodedBytes, String secretKey, String hashIV, EncodeEnum encType, ZipType unzipAfter, KeyHash keyHash)
            throws InvalidCipherTextException, IOException {

        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");

        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        String hash = (hashIV != null && hashIV.length() > 0) ? hashIV : (kHash != null) ? kHash.hash(secretKey) : KeyHash.Hex.hash(secretKey);
        cipherHash = hash;
        encodeType = encType;
        zType = unzipAfter;
        kHash = keyHash;

        byte[] cipherBytes = new byte[0];
        if (encType != EncodeEnum.None)
        {
            String encoded =  new String(encodedBytes, StandardCharsets.UTF_8);
            cipherBytes = encodeType.decodeStringToBytes(encoded);
        }
        else
            cipherBytes = encodedBytes;

        byte[] outBytes = decrpytRoundGoMerry(cipherBytes, secretKey, hashIV, unzipAfter);

        return outBytes;
    }



    /**
     *  encrpytToString
     * @param inString String to encrypt multiple times
     * @param cryptKey Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
     *     /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe
     * @param encoding {@link EncodeEnum} type for encoding encrypted bytes back in plain text
     * @param zipBefore zip bytes with {@link ZipType}
     * @param keyHash {@link KeyHash} hashing enum => use hash(...) for hashing
     * @return encrypted String<
     * @throws InvalidCipherTextException
     * @throws IOException
     */
    public static String encrpytToString(String inString, String cryptKey,
                                         EncodeEnum encoding,
                                         ZipType zipBefore,
                                         KeyHash keyHash)
            throws InvalidCipherTextException, IOException {
        // construct symmetric cipher pipeline with cryptKey
        CipherPipe cyptPipe = new CipherPipe(cryptKey);

        // Transform String to bytes
        byte[] inBytes = inString.getBytes(Charset.forName("UTF-8"));
        // perform multi crypt pipe stages
        byte[] encryptedBytes = cyptPipe.encrpytGoRounds(inBytes, cryptKey, zipBefore, keyHash);
        // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
        String encrypted = encoding.encodeBytesToString(encryptedBytes);

        return encrypted;
    }

    public static String encrpytBytesToString(byte[] plainBytes, String cryptKey,
                                              EncodeEnum encoding,
                                              ZipType zipBefore,
                                              KeyHash keyHash)
                                            throws InvalidCipherTextException, IOException {
        // construct symmetric cipher pipeline with cryptKey
        CipherPipe cyptPipe = new CipherPipe(cryptKey);

        // perform multi crypt pipe stages
        byte[] encryptedBytes = cyptPipe.encrpytGoRounds(plainBytes, cryptKey, zipBefore, keyHash);
        // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
        String encrypted = encoding.encodeBytesToString(encryptedBytes);

        return encrypted;
    }

    public static byte[] encrpytStringToBytes(String inString, String cryptKey,
                                              EncodeEnum encoding,
                                              ZipType zipBefore,
                                              KeyHash keyHash)
                                        throws InvalidCipherTextException {
        // construct symmetric cipher pipeline with cryptKey and pass pipeString as out param
        CipherPipe cryptPipe = new CipherPipe(cryptKey);

        // Transform String to bytes
        byte[] inBytes = inString.getBytes(Charset.forName("UTF-8"));
        // perform multi crypt pipe stages
        byte[] encryptedBytes = cryptPipe.encrpytGoRounds(inBytes, cryptKey, zipBefore, keyHash);

        return encryptedBytes;
    }



    /**
     * DecrpytToString
     * @param cryptedEncodedMsg encrypted message
     * @param cryptKey Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
     *          and unique crypt key for each symmetric cipher algorithm in each stage of the pipe
     * @param decoding {@link EncodeEnum}
     * @param unzipAfter {@link ZipType}
     * @param keyHash {@link KeyHash}
     * @return Decrypted stirng
     * @throws InvalidCipherTextException
     * @throws IOException
     */
    public static String decrpytToString(String cryptedEncodedMsg, String cryptKey,
                                         EncodeEnum decoding,
                                         ZipType unzipAfter,
                                         KeyHash keyHash)
            throws InvalidCipherTextException, IOException {
        // create symmetric cipher pipe for decryption with crypt key and pass pipeString as out param
        CipherPipe cryptPipe = new CipherPipe(cryptKey);

        // get bytes from encrypted encoded String dependent on the encoding type(uu, base64, base32,..)
        byte[] cipherBytes = decoding.decodeStringToBytes(cryptedEncodedMsg);
        // staged decryption of bytes
        byte[] unroundedMerryBytes = cryptPipe.decrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);

        // Get String from decrypted bytes
        String decrypted = unroundedMerryBytes.toString();
        // find first \0 = NULL char in String and truncate all after first \0 apperance in String
        while (decrypted.charAt(decrypted.length() - 1) == '\0')
            decrypted = decrypted.substring(0, decrypted.length() - 1);

        return decrypted;
    }

    public static byte[] decrpytStringToBytes(String cryptedEncodedMsg, String cryptKey,
                                              EncodeEnum decoding,
                                              ZipType unzipAfter,
                                              KeyHash keyHash)
            throws InvalidCipherTextException, IOException {
        // create symmetric cipher pipe for decryption with crypt key
        CipherPipe cryptPipe = new CipherPipe(cryptKey);

        // get bytes from encrypted encoded String dependent on the encoding type (uu, base64, base32,..)
        byte[] cipherBytes = decoding.decodeStringToBytes(cryptedEncodedMsg);
        // staged decryption of bytes
        byte[] unroundedMerryBytes = cryptPipe.decrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);

        return unroundedMerryBytes;
    }

    public static String decrpytBytesToString(byte[] cipherBytes, String cryptKey,
                                              EncodeEnum decoding,
                                              ZipType unzipAfter,
                                              KeyHash keyHash)
                                        throws InvalidCipherTextException {
        // create symmetric cipher pipe for decryption with crypt key and pass pipeString as out param
        CipherPipe cryptPipe = new CipherPipe(cryptKey);

        // staged decryption of bytes
        byte[] unroundedMerryBytes = cryptPipe.decrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);

        // Get String from decrypted bytes
        String decrypted =  unroundedMerryBytes.toString();
        // find first \0 = NULL char in String and truncate all after first \0 apperance in String
        while (decrypted.charAt(decrypted.length() - 1) == '\0')
            decrypted = decrypted.substring(0, decrypted.length() - 1);

        return decrypted;
    }

}

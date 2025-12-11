package eu.cqrxs.cipherpipe.crypt.cipher;

import static eu.cqrxs.cipherpipe.crypt.cipher.CipherEnum.CamelliaLight;

import androidx.core.content.res.TypedArrayUtils;

import com.google.common.primitives.Bytes;

import java.io.IOException;
import java.util.Arrays;
import java.util.List;
import java.io.ByteArrayOutputStream;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.HexFormat;
import java.util.List;
import org.bouncycastle.crypto.*;
import org.bouncycastle.crypto.engines.*;
import org.bouncycastle.crypto.BlockCipher;

import eu.cqrxs.cipherpipe.crypt.encoding.EncodeEnum;
import eu.cqrxs.cipherpipe.crypt.encoding.Hex16Coder;
import eu.cqrxs.cipherpipe.crypt.hash.KeyHash;
import eu.cqrxs.cipherpipe.enums.ZipType;
import eu.cqrxs.cipherpipe.util.Constants;



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
        List<CipherEnum> ceList = new ArrayList<CipherEnum>();
        for (int i = inPipe.length - 1; i >= 0; i--) {
            ceList.add(inPipe[i]);
        }
        return ceList.toArray(CipherEnum[]::new);
    }

    public String getPipeString() {
        String pipeString = "";
        for (CipherEnum cipher : inPipe)
            pipeString = pipeString + cipher.getCipherChar();
        return pipeString;
    }

    public CipherPipe() {
        cipherKey = ""; //
        cipherHash = "";
        inPipe = new CipherEnum[0];
        encodeType = EncodeEnum.Base64;
        zType = ZipType.None;
        kHash = KeyHash.Hex;
    }

    public CipherPipe(CipherEnum[] cipherEnums, int maxpipe, EncodeEnum encType, ZipType zpType, KeyHash kh) {

        // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
        maxpipe = ((maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe); // if somebody wants more, he/she/it gets less

        int isize = Math.min(((int)cipherEnums.length), ((int)maxpipe));
        inPipe = new CipherEnum[isize];
        System.arraycopy(cipherEnums, 0, inPipe, 0, isize);

        encodeType = encType;
        zType = zpType;
        kHash = kh;
    }

    /// <summary>
    /// CipherPipe constructor with an array of <see cref="T:String[]"/> cipherAlgos as inpipe
    /// </summary>
    /// <param name="cipherAlgos">array of <see cref="T:String[]"/> as inpipe</param>
    /// <param name="maxpipe">maximum lentgh <see cref="Constants.MAX_PIPE_LEN"/></param>
    /// <param name="encType"><see cref="EncodeType"/></param>
    /// <param name="zpType"><see cref="Zip.ZipType"/></param>
    /// <param name="kh"><see cref="KeyHash"/></param>
    public CipherPipe(String[] cipherAlgos, int maxpipe, EncodeEnum encType, ZipType zpType, KeyHash kh) {
        // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
        maxpipe = ((maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe); // if somebody wants more, he/she/it gets less

        List<CipherEnum> cipherEnums = new ArrayList<CipherEnum>();
        int cnt = 0;
        for (String algo : cipherAlgos)
        {
            if (algo != null && algo.length() > 0)
            {
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

    /// <summary>
    /// CipherPipe ctor with array of user key bytes
    /// </summary>
    /// <param name="keyBytes">user key bytes</param>
    /// <param name="maxpipe">maximum length <see cref="Constants.MAX_PIPE_LEN"/></param>
    /// <param name="encType"><see cref="EncodeType"/></param>
    /// <param name="zpType"><see cref="Zip.ZipType"/></param>
    /// <param name="kh"><see cref="KeyHash"/></param>
    /// <exception cref="ArgumentException"></exception>
    public CipherPipe(byte[] keyBytes, int maxpipe, EncodeEnum encType, ZipType zpType, KeyHash kh) {
        // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
        maxpipe = ((maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe); // if somebody wants more, he/she/it gets less

        short scnt = 0;
        List<CipherEnum> pipeList = new ArrayList<CipherEnum>();

        HashSet<Byte> hashBytes = new HashSet<Byte>();
        for (int i = 0; i < keyBytes.length && pipeList.size() < maxpipe; i++) {
            Byte cb = Byte.valueOf((byte)((int)((int)keyBytes[i] % 0x20)));
            if (!hashBytes.contains(cb)) {
                hashBytes.add(cb);
                pipeList.add(CipherEnum.getByteCipherDict().get(cb));
            }
        }

        inPipe = pipeList.toArray(CipherEnum[]::new);

        zType = zpType;
        encodeType = encType;
        kHash = kh;

        //if (inPipe.Length > maxpipe)
        //{
        //    List<String> pipElems = new List<String>(inPipe.Length);
        //    foreach (var cipherEnum in inPipe)
        //        pipElems.Add(cipherEnum.ToString());
        //    throw new ArgumentException($"Pipe \"{String.Join(";", pipElems.ToArray())}\" length exceeds {maxpipe}!");
        //}

        // foreach (CipherEnum cipherE in inPipe)
        // pipeString += cipherE.GetCipherChar();

    }

    /// <summary>
    /// Constructs a <see cref="CipherPipe"/> from key and hash
    /// by getting <see cref="T:byte[]">byte[] keybytes</see> with <see cref="CryptHelper.GetUserKeyBytes(String, String, int)"/>
    /// </summary>
    /// <param name="key">secret key to generate pipe</param>
    /// <param name="hash">hash value of secret key</param>
    /// <param name="maxpipe"></param>
    /// <param name="encType"></param>
    /// <param name="zpType"></param>
    /// <param name="kh"></param>
    public CipherPipe(String key, String hash, EncodeEnum encType, ZipType zpType, KeyHash kh) {

        this(CryptHelper.GetKeyBytesSimple(key, hash, 16), Constants.MAX_PIPE_LEN, encType, zpType, kh);
        cipherKey = key;
        cipherHash = hash;
    }

    /// <summary>
    /// CipherPipe ctor with only key
    /// </summary>
    /// <param name="key"></param>
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

    /// <summary>
    /// Generic encrypt bytes to bytes
    /// </summary>
    /// <param name="inBytes">Array of byte</param>
    /// <param name="cipherAlgo"><see cref="CipherEnum"/> both symmetric and asymetric cipher algorithms</param>
    /// <param name="secretKey">secret key to decrypt</param>
    /// <param name="hash">key's hash</param>
    /// <returns>encrypted byte Array</returns>
    public static byte[] EncryptBytesFast(byte[] inBytes, CipherEnum cipherAlgo, String secretKey, String hash) {
        if (secretKey == null || secretKey.length() < 1)
            throw new IllegalArgumentException("seretkey");
        if (hash == null || hash.length() == 0)
            throw new IllegalArgumentException("hash");

        byte[] encryptBytes = inBytes;

        switch (cipherAlgo)
        {
            /*
            AesNet aesNet = new AesNet(secretKey, hash);
            encryptBytes = aesNet.Encrypt(inBytes);
            break;
            case CipherEnum.Des3Net:
                Des3Net des3 = new Des3Net(secretKey, hash);
                encryptBytes = des3.Encrypt(inBytes);
                break;
            case CipherEnum.RC564:
                RC564.RC564GenWithKey(secretKey, hash, true);
                encryptBytes = RC564.Encrypt(inBytes);
                break;
            case CipherEnum.Rsa:
                AsymmetricCipherKeyPair keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                encryptBytes = Asymmetric.Rsa.Encrypt(inBytes, keyPair);
                break;
            case CipherEnum.ZenMatrix:
                encryptBytes = (new ZenMatrix(secretKey, hash, false)).Encrypt(inBytes);
                break;
            case CipherEnum.ZenMatrix2:
                encryptBytes = (new ZenMatrix2(secretKey, hash, false)).Encrypt(inBytes);
                break;
             */
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
            case ThreeFish256:
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
            case ZenMatrix:
            case ZenMatrix2:
            default:
                CryptParams cpParams = new CryptParams(cipherAlgo, secretKey, hash);
                // CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(cpParams, true);
                // encryptBytes = cryptBounceCastle.Encrypt(inBytes);
                encryptBytes = inBytes; // TODO: port standard bouncycastle wrapper to java
                break;
        }

        return encryptBytes;
    }

    /// <summary>
    /// Generic decrypt bytes to bytes
    /// </summary>
    /// <param name="cipherBytes">Encrypted array of byte</param>
    /// <param name="cipherAlgo"><see cref="CipherEnum"/>both symmetric and asymetric cipher algorithms</param>
    /// <param name="secretKey">secret key to decrypt</param>
    /// <param name="hash">key's hash</param>
    /// <returns>decrypted byte Array</returns>
    public static byte[] DecryptBytesFast(byte[] cipherBytes, CipherEnum cipherAlgo, String secretKey, String hash) {
        if (secretKey == null || secretKey.length() == 0)
            throw new IllegalArgumentException("seretkey");
        if (hash == null || hash.length() == 0)
            throw new IllegalArgumentException("hash");
        // bool sameKey = true;

        byte[] decryptBytes = cipherBytes;

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
                break;
            case CipherEnum.RC564:
                RC564.RC564GenWithKey(secretKey, hash, true);
                decryptBytes = RC564.Decrypt(cipherBytes);
                break;
            case CipherEnum.Rsa:
                AsymmetricCipherKeyPair keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                decryptBytes = Asymmetric.Rsa.DecryptWithPrivate(cipherBytes, keyPair);
                break;
            case CipherEnum.ZenMatrix:
                decryptBytes = (new ZenMatrix(secretKey, hash, false)).Decrypt(cipherBytes);
                break;
            case CipherEnum.ZenMatrix2:
                decryptBytes = (new ZenMatrix2(secretKey, hash, false)).Decrypt(cipherBytes);
                break;
             */
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
            case ThreeFish256:
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
            case ZenMatrix:
            case ZenMatrix2:
            default:
                CryptParams cpParams = new CryptParams(cipherAlgo, secretKey, hash);
                // CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(cpParams, true);
                // decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
                decryptBytes = cipherBytes; // TODO: port bouncy castle wrapper to java
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
    public byte[] merryGoRoundEncrpyt(byte[] inBytes, String secretKey, String hashIv, ZipType zipBefore) {
        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");

        String hash = (hashIv != null && hashIv.length() > 0) ? hashIv : (kHash != null) ? kHash.hash(secretKey) : KeyHash.Hex.hash(secretKey);
        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = hash;

        byte[] encryptedBytes = new byte[inBytes.length];
        System.arraycopy(inBytes, 0, encryptedBytes, 0, inBytes.length);
        //#if DEBUG
        //            stageDictionary = new Dictionary<CipherEnum, byte[]>();
        //            // stageDictionary.Add(CipherEnum.ZenMatrix, inBytes);
        //#endif
        if (zipBefore != ZipType.None)
        {
            encryptedBytes = zipBefore.zip(inBytes);
            inBytes = encryptedBytes;
        }

        for (CipherEnum cipher : inPipe)
        {
            encryptedBytes = EncryptBytesFast(inBytes, cipher, cipherKey, cipherHash);
            inBytes = encryptedBytes;
            //#if DEBUG
            //                stageDictionary.Add(cipher, encryptedBytes);
            //#endif
        }

        return encryptedBytes;
    }

    /***
     * decrpytRoundGoMerry against clock turn -
     *    starts merry to turn arround from right to left against clock hour cycle
     * @param cipherBytes encrypted byte array
     * @param secretKey user secret key, normally email address
     * @param hashIv hash relational to secret key
     * @param unzipAfter {@link ZipType}
     * @return byte[]
     */
    public byte[] decrpytRoundGoMerry(byte[] cipherBytes, String secretKey, String hashIv, ZipType unzipAfter) {
        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");

        String hash = (hashIv != null && hashIv.length() > 0) ? hashIv : (kHash != null) ? kHash.hash(secretKey) : KeyHash.Hex.hash(secretKey);
        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = hash;


        byte[] decryptedBytes = new byte[cipherBytes.length];
        //#if DEBUG
        //            stageDictionary = new Dictionary<CipherEnum, byte[]>();
        //            // stageDictionary.Add(CipherEnum.ZenMatrix, cipherBytes);
        //#endif
        if (getOutPipe() == null || getOutPipe().length == 0)
            System.arraycopy(cipherBytes, 0, decryptedBytes, 0, cipherBytes.length);
        else
            for (CipherEnum cipher : getOutPipe())
            {
                decryptedBytes = DecryptBytesFast(cipherBytes, cipher, cipherKey, cipherHash);
                cipherBytes = decryptedBytes;
                //#if DEBUG
                //                    stageDictionary.Add(cipher, cipherBytes);
                //#endif
            }

        if (unzipAfter != ZipType.None)
            decryptedBytes = unzipAfter.unzip(cipherBytes);

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
            KeyHash keyHash) throws IOException
    {
        // Transform String to bytes
        byte[] inBytes = inString.getBytes();

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
            KeyHash keyHash)
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

    /// <summary>
    /// decrypt encoded encrypted text
    /// </summary>
    /// <param name="cryptedEncodedMsg">encoded encrypted ASCII String</param>
    /// <param name="cryptKey">prviate key for encryption</param>
    /// <param name="hashIv">private hash for encryption</param>
    /// <param name="decoding"><see cref="EncodeEnum"/></param>
    /// <param name="unzipAfter"><see cref="ZipType"/></param>
    /// <param name="keyHash"><see cref="KeyHash"/></param>
    /// <returns>decrypted UTF8 String, containing no binary data</returns>
    public String decryptTextRoundsGo(
            String cryptedEncodedMsg,
            String cryptKey,
            String hashIv,
            EncodeEnum decoding,
            ZipType unzipAfter,
            KeyHash keyHash) throws IOException
    {
        byte[] cipherBytes = decoding.decodeStringToBytes(cryptedEncodedMsg);

        // perform multi crypt pipe stages
        byte[] decryptedBytes = decryptFileBytesRoundsGo(cipherBytes, cryptKey, hashIv, decoding, unzipAfter, keyHash);

        // Get String from decrypted bytes
        String decrypted = decryptedBytes.toString();
        // find first \0 = NULL char in String and truncate all after first \0 apperance in String
        int idx = decrypted.length() - 1;
        while (decrypted.charAt(decrypted.length() - 1) == '\0')
            decrypted = decrypted.substring(0, decrypted.length() - 1);

        return decrypted;
    }

    /// <summary>
    /// DecryptFileBytesRoundsGo
    /// </summary>
    /// <param name="cipherBytes"></param>
    /// <param name="cryptKey">prviate key for encryption</param>
    /// <param name="hashIv">private hash for encryption</param>
    /// <param name="decoding"><see cref="EncodeEnum">decoding type</see> for decodinng</param>
    /// <param name="unzipAfter"><see cref="ZipType"/></param>
    /// <param name="keyHash"><see cref="KeyHash"/></param>
    /// <returns>plain data byte[]</returns>
    public byte[] decryptFileBytesRoundsGo(
            byte[] cipherBytes,
            String cryptKey,
            String hashIv,
            EncodeEnum decoding,
            ZipType unzipAfter,
            KeyHash keyHash)
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


    public byte[] encrpytGoRounds(byte[] inBytes, String secretKey, ZipType zipBefore, KeyHash keyHash) {
        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");

        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = keyHash.hash(secretKey);
        zType = zipBefore;
        kHash = keyHash;
        return merryGoRoundEncrpyt(inBytes, secretKey, cipherHash, zipBefore);
    }


    public byte[] decrpytRoundsGo(byte[] cipherBytes, String secretKey, ZipType unzipAfter, KeyHash keyHash) {
        if ((secretKey == null && cipherKey == null) || (secretKey.length() == 0 && cipherKey.length() == 0))
            throw new IllegalArgumentException("seretkey");
        cipherKey = (secretKey != null && secretKey.length() > 0) ? secretKey : cipherKey;
        cipherHash = keyHash.hash(secretKey);
        zType = unzipAfter;
        kHash = keyHash;
        return decrpytRoundGoMerry(cipherBytes, secretKey, keyHash.hash(secretKey), unzipAfter);
    }


    public String encrpytEncode(byte[] inBytes, String secretKey, EncodeEnum encType, ZipType zipBefore, KeyHash keyHash) throws IOException {

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

    public byte[] encryptEncodeBytes(byte[] inBytes, String secretKey, String hashIV, EncodeEnum encType, ZipType zipBefore, KeyHash keyHash) throws IOException {

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
            encryptedBytes = cryptedEncoded.getBytes();
        }
        else
            encryptedBytes = outBytes;


        return encryptedBytes;
    }



    public byte[] decodeDecrpyt(String encoded, String secretKey, EncodeEnum encType, ZipType unzipAfter, KeyHash keyHash)
                    throws IOException {

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


    public byte[] decodeDecrpytBytes(byte[] encodedBytes, String secretKey, String hashIV, EncodeEnum encType, ZipType unzipAfter, KeyHash keyHash)
            throws IOException {

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
            String encoded = encodedBytes.toString();
            cipherBytes = encodeType.decodeStringToBytes(encoded);
        }
        else
            cipherBytes = encodedBytes;

        byte[] outBytes = decrpytRoundGoMerry(cipherBytes, secretKey, hashIV, unzipAfter);

        return outBytes;
    }


    /// <summary>
    /// EncrpytToStringd
    /// </summary>
    /// <param name="inString">String to encrypt multiple times</param>
    /// <param name="cryptKey">Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
    /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe</param>
    /// <param name="encoding"><see cref="EncodeEnum"/> type for encoding encrypted bytes back in plain text</param>
    /// <param name="zipBefore">Zip bytes with <see cref="ZipType"/> before passing them in encrypted stage pipeline. <see cref="ZipTypeExtensions.Zip(ZipType, byte[])"/></param>
    /// <param name="keyHash"><see cref="KeyHash"/> hashing key algorithm</param>
    /// <returns>encrypted String</returns>
    public static String EncrpytToString(String inString, String cryptKey,
                                         EncodeEnum encoding,
                                         ZipType zipBefore,
                                         KeyHash keyHash) throws IOException {
        // construct symmetric cipher pipeline with cryptKey
        CipherPipe cyptPipe = new CipherPipe(cryptKey);

        // Transform String to bytes
        byte[] inBytes = inString.getBytes();
        // perform multi crypt pipe stages
        byte[] encryptedBytes = cyptPipe.encrpytGoRounds(inBytes, cryptKey, zipBefore, keyHash);
        // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
        String encrypted = encoding.encodeBytesToString(encryptedBytes);

        return encrypted;
    }

    public static String encrpytBytesToString(byte[] plainBytes, String cryptKey,
                                              EncodeEnum encoding,
                                              ZipType zipBefore,
                                              KeyHash keyHash) throws IOException {
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
                                              KeyHash keyHash)  {
        // construct symmetric cipher pipeline with cryptKey and pass pipeString as out param
        CipherPipe cryptPipe = new CipherPipe(cryptKey);

        // Transform String to bytes
        byte[] inBytes = inString.getBytes();
        // perform multi crypt pipe stages
        byte[] encryptedBytes = cryptPipe.encrpytGoRounds(inBytes, cryptKey, zipBefore, keyHash);

        return encryptedBytes;
    }


    /// <summary>
    /// DecrpytToString
    /// </summary>
    /// <param name="cryptedEncodedMsg">encrypted message</param>
    /// <param name="cryptKey">Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline
    /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe</param>
    /// <param name="decoding"><see cref="EncodeEnum"/> type for encoding encrypted bytes back in plain text></param>
    /// <param name="unzipAfter"><see cref="ZipType"/> and <see cref="ZipTypeExtensions.Unzip(ZipType, byte[])"/></param>
    /// <param name="keyHash"><see cref="KeyHash"/> hashing key algorithm</param>
    /// <returns>Decrypted stirng</returns>
    public static String decrpytToString(String cryptedEncodedMsg, String cryptKey,
                                         EncodeEnum decoding,
                                         ZipType unzipAfter,
                                         KeyHash keyHash) throws IOException {
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
                                              KeyHash keyHash) throws IOException {
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
                                              KeyHash keyHash) {
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

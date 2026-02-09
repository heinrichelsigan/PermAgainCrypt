package eu.cqrxs.crypt.cipher;

// import static eu.cqrxs.crypt.cipher.CipherEnum.CamelliaLight;
// import androidx.core.content.res.TypedArrayUtils;
// import com.google.common.primitives.Bytes;
import org.bouncycastle.crypto.BlockCipher;
import org.bouncycastle.crypto.engines.AESEngine;
import org.bouncycastle.crypto.engines.AESLightEngine;
import org.bouncycastle.crypto.engines.ARIAEngine;
import org.bouncycastle.crypto.engines.BlowfishEngine;
import org.bouncycastle.crypto.engines.TwofishEngine;
import org.bouncycastle.crypto.engines.ThreefishEngine;
import org.bouncycastle.crypto.engines.CamelliaEngine;
import org.bouncycastle.crypto.engines.CamelliaLightEngine;
import org.bouncycastle.crypto.engines.CAST5Engine;
import org.bouncycastle.crypto.engines.CAST6Engine;
import org.bouncycastle.crypto.engines.DESedeEngine;
import org.bouncycastle.crypto.engines.DESEngine;
import org.bouncycastle.crypto.engines.DSTU7624Engine;
import org.bouncycastle.crypto.engines.GOST28147Engine;
import org.bouncycastle.crypto.engines.IDEAEngine;
import org.bouncycastle.crypto.engines.NoekeonEngine;
import org.bouncycastle.crypto.engines.RC2Engine;
import org.bouncycastle.crypto.engines.RC532Engine;
import org.bouncycastle.crypto.engines.RC564Engine;
import org.bouncycastle.crypto.engines.RC6Engine;
import org.bouncycastle.crypto.engines.RijndaelEngine;
import org.bouncycastle.crypto.engines.SEEDEngine;
import org.bouncycastle.crypto.engines.SkipjackEngine;
import org.bouncycastle.crypto.engines.SM4Engine;
import org.bouncycastle.crypto.engines.TEAEngine;
import org.bouncycastle.crypto.engines.TnepresEngine;
import org.bouncycastle.crypto.engines.XTEAEngine;
// import org.bouncycastle.crypto.engines.
import eu.cqrxs.crypt.hash.KeyHash;
import eu.cqrxs.util.Constants;


/**
 * Crypt Params
 */
public class CryptParams {

    public CipherEnum cipher;

    public String getAlgorithmName() {
        return cipher.getName();
    }

    public void setAlgorithmName(String algo) {
        cipher = CipherEnum.valueOf(algo);
    }

    /**
     * getMode()
     * @return {@link String} with 3 characters for {@link CipherMode2}
     */
    public String getMode() { return cmode2.getName(); }

    public String key;
    public String hash;
    // public String mode;

    public CipherMode2 cmode2;

    public int size;

    public int blockSize;

    public int keyLen;

    public BlockCipher blockCipher;

    public KeyHash keyHashing;


    /**
     * CryptParams(CipherEnum cparameter less default constructor
     */
    public CryptParams() {
        cipher = CipherEnum.Aes;
        size = 256;
        keyLen = 32;
        cmode2 = CipherMode2.ECB;
        blockCipher = new AESEngine();
        keyHashing = KeyHash.Hex;
        blockSize = blockCipher.getBlockSize();
    }



    /**
     * CryptParams(CipherEnum constructor
     * @param cipherAlgo {@link CipherEnum}
     */
    public CryptParams(CipherEnum cipherAlgo) {
        cipher = cipherAlgo;
        size = 256;
        keyLen = 32;
        cmode2 = CipherMode2.ECB;
        keyHashing = KeyHash.Hex;

        switch (cipher) {
            case Aes:
            /* case AesNet: // TODO: Implement interface IBlockCipher in AesNet
                blockCipher = new AESEngine();
                break;
             */
            case AesLight:
                size = 128;
                blockCipher = new AESLightEngine();
                break;
            case Aria:
                size = 128;
                blockCipher = new ARIAEngine();
                break;
            case BlowFish:
                size = 64;
                keyLen = 8;
                blockCipher = new BlowfishEngine();
                break;
            case Fish2:
                size = 128;
                keyLen = 16;
                blockCipher = new TwofishEngine();
                break;
            case Fish3:
				blockCipher = new ThreefishEngine(size);
                break;                            
            case Camellia:
                size = 128;
                keyLen = 16;
                blockCipher = new CamelliaEngine();
                break;
            case CamelliaLight:
                size = 128;
                keyLen = 16;
                blockCipher = new CamelliaLightEngine();
                break;
            case Cast5:
                size = 128;
                keyLen = 16;
                blockCipher = new CAST5Engine();
                break;
            case Cast6:
                blockCipher = new CAST6Engine();
                break;
            case Des:
                size = 64;
                keyLen = 8;
                blockCipher = new DESEngine();
                break;
            /*
            case Des3Net: // TODO: implement IBlockCipher in Des3Net
                size = 128;
                keyLen = 16;
                blockCipher = new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
                break;
            */
            case Des3:
                size = 128;
                keyLen = 16;
                blockCipher = new DESedeEngine();
                break;
            case Dstu7624:
                size = 128;
                keyLen = 16;
                blockCipher = new DSTU7624Engine(size);
                break;
            case Gost28147:
                blockCipher = new GOST28147Engine();
                break;
            case Idea:
                blockCipher = new IDEAEngine();
                break;
            case Noekeon:
                size = 128;
                keyLen = 16;
                blockCipher = new NoekeonEngine();
                break;
            case RC2:
                size = 128;
		        keyLen = 32;
                blockCipher = new RC2Engine();
                break;
            case RC532:
                blockCipher = new RC532Engine();
                break;
            case RC564:
                size = 64;
		        keyLen = 32;
                blockCipher = new RC564Engine();
                break;
            case RC6:
                blockCipher = new RC6Engine();
                break;
	        case Rijndael:
				blockCipher = new RijndaelEngine();
				break;
            case Seed:
                blockCipher = new SEEDEngine();
                size = 128;
                keyLen = 16;
                break;
            case Serpent:
                blockCipher = new org.bouncycastle.crypto.engines.SerpentEngine();
                size = 128;
                keyLen = 16;
                break;
            case SM4:
                blockCipher = new SM4Engine();
                size = 128;
                keyLen = 16;
                break;
            case SkipJack:
                blockCipher = new SkipjackEngine();
                break;
            case Tea:
                size = 128;
                keyLen = 16;
                blockCipher = new TEAEngine();
                break;
            case Tnepres:
                size = 128;
                keyLen = 16;
                blockCipher = new TnepresEngine();
                break;
            case XTea:
				size = 128;
                keyLen = 16;
                blockCipher = new XTEAEngine();
                break;
            case ZenMatrix:
                size = 16;
                keyLen = 16;
                // TODO: port it to java
                blockCipher = new ZenMatrix(size);
                break;
            /*)
            case ZenMatrix2:
                // throw new NotImplementedException("ZenMatrix2 IBlockCipher interface not implemented");)
                size = 32;
                keyLen = 16;
                // TODO: port it to java
                // blockCipher = new ZenMatrix2();
                // break;
             */
            default:
                blockCipher = new  AESEngine();
                break;
        }

        blockSize = blockCipher.getBlockSize();

    }


    /**
     * CryptParams(CipherEnum constructor
     * @param cipherAlgo {@link CipherEnum}
     * @param secretKey user key for encryption {@link String}
     * @param keyHashed hashed user key
     * @param keyHash {@link KeyHash}
     * @param cMode2 {@link CipherMode2}
     */
    public CryptParams(CipherEnum cipherAlgo,
                       String secretKey,
                       String keyHashed,
                       KeyHash keyHash,
                       CipherMode2 cMode2
                    ) {
        this(cipherAlgo);
        keyHashing = keyHash;
        cmode2 = cMode2;
        key = (secretKey == null || secretKey.isEmpty()) ? Constants.AUTHOR_EMAIL : secretKey;
        hash = (keyHashed == null || keyHashed.isEmpty()) ? keyHashing.hash(secretKey) : keyHashed;
    }

    /**
     * CryptParams(CipherEnum constructor
     * @param cipherAlgo {@link CipherEnum}
     * @param secretKey user key for encryption {@link String}
     * @param keyHash {@link KeyHash}
     * @param cMode2 {@link CipherMode2}
     */
    public CryptParams(CipherEnum cipherAlgo,
                       String secretKey,
                       KeyHash keyHash,
                       CipherMode2 cMode2) {
        this(cipherAlgo, secretKey, keyHash.hash(secretKey), keyHash, cMode2);
    }

    /***
     * constructs a  CryptParams object by {@link CipherEnum}
     * @param cipherAlgo {@link CipherEnum}
     * @param secretKey user key for encryption {@link String}
     * @param keyHashed hashed user key
     * @param cMode2 {@link CipherMode2}
     */
    public CryptParams(CipherEnum cipherAlgo,
                       String secretKey,
                       String keyHashed,
                       CipherMode2 cMode2) {
        this(cipherAlgo);
        cmode2 = cMode2;
        key = (secretKey == null || secretKey.isEmpty()) ? Constants.AUTHOR_EMAIL : secretKey;
        hash = (keyHashed == null || keyHashed.isEmpty()) ? keyHashing.hash(secretKey) : keyHashed;
    }

    /***
     * constructs a  CryptParams object by {@link CipherEnum}
     * @param cipherAlgo {@link CipherEnum}
     * @param secretKey user key for encryption {@link String}
     * @param keyHashed hashed user key
     */
    public CryptParams(CipherEnum cipherAlgo,
                       String secretKey,
                       String keyHashed) {
        this(cipherAlgo, secretKey, keyHashed, CipherMode2.ECB);
    }

    /***
     * Constructs instance via another object instance
     * @param cryptParams another instance
     */
    public CryptParams(CryptParams cryptParams) {
        this(cryptParams.cipher, cryptParams.key, cryptParams.hash,
            cryptParams.keyHashing, cryptParams.cmode2);
    }


    /***
     *  static way to get valid CryptParams for a requested  {@link CipherEnum}
     * @param cipherAlgo {@link CipherEnum}
     * @return new constructed CryptParams
     */
    @Deprecated
    public static CryptParams RequestAlgorithm(CipherEnum cipherAlgo) {

        return new CryptParams(cipherAlgo);
    }

    @Deprecated
    public static CryptParams GetCryptParams(CryptParams cParams) {

        return new CryptParams(cParams);
    }

    @Deprecated
    public static BlockCipher GetBlockCipher(CipherEnum cipherAlgo) {
        return (new CryptParams(cipherAlgo)).blockCipher;
    }


}

package eu.cqrxs.cipherpipe.crypt.cipher;

import static eu.cqrxs.cipherpipe.crypt.cipher.CipherEnum.CamelliaLight;

import androidx.core.content.res.TypedArrayUtils;

// import com.google.common.primitives.Bytes;
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

import eu.cqrxs.cipherpipe.crypt.encoding.Hex16Coder;
import eu.cqrxs.cipherpipe.crypt.hash.KeyHash;
import eu.cqrxs.cipherpipe.util.Constants;



public class CryptParams {

    public CipherEnum cipher;

    public String getAlgorithmName() {
        return cipher.getName();
    }

    public void setAlgorithmName(String algo) {
        cipher = CipherEnum.valueOf(algo);
    }

    public String key;
    public String hash;
    public String mode;
    public int size;

    public int blockSize;

    public int keyLen;

    public BlockCipher blockCipher;

    public KeyHash keyHashing;


    public CryptParams() {
        cipher = CipherEnum.Aes;
        size = 256;
        keyLen = 32;
        mode = "ECB";
        blockCipher = new org.bouncycastle.crypto.engines.AESEngine();
        keyHashing = KeyHash.Hex;
        blockSize = blockCipher.getBlockSize();
    }


    public CryptParams(CipherEnum cipherAlgo) {
        cipher = cipherAlgo;
        size = 256;
        keyLen = 32;
        mode = "ECB";
        keyHashing = KeyHash.Hex;

        switch (cipher) {
            case Aes:
            case Rijndael:
            case AesNet: // TODO: Implement interface IBlockCipher in AesNet
                blockCipher = new org.bouncycastle.crypto.engines.AESEngine();
                break;
            case AesLight:
                size = 128;
                blockCipher = new org.bouncycastle.crypto.engines.AESLightEngine();
                break;
            case Aria:
                size = 128;
                blockCipher = new org.bouncycastle.crypto.engines.ARIAEngine();
                break;
            case BlowFish:
                size = 64;
                keyLen = 8;
                blockCipher = new org.bouncycastle.crypto.engines.BlowfishEngine();
                break;
            case Fish2:
                size = 128;
                keyLen = 16;
                blockCipher = new org.bouncycastle.crypto.engines.TwofishEngine();
                break;
            case Fish3:
            case ThreeFish256:
                blockCipher = new org.bouncycastle.crypto.engines.ThreefishEngine(size);
                break;
            case Camellia:
                size = 128;
                keyLen = 16;
                blockCipher = new org.bouncycastle.crypto.engines.CamelliaEngine();
                break;
            case CamelliaLight:
                size = 128;
                keyLen = 16;
                blockCipher = new org.bouncycastle.crypto.engines.CamelliaLightEngine();
                break;
            case Cast5:
                size = 128;
                keyLen = 16;
                blockCipher = new org.bouncycastle.crypto.engines.CAST5Engine();
                break;
            case Cast6:
                blockCipher = new org.bouncycastle.crypto.engines.CAST6Engine();
                break;
            case Des:
                size = 64;
                keyLen = 8;
                blockCipher = new org.bouncycastle.crypto.engines.DESEngine();
                break;
            case Des3Net: // TODO: implement IBlockCipher in Des3Net
            /*
                size = 128;
                keyLen = 16;
                blockCipher = new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
                break;
            */
            case Des3:
                size = 128;
                keyLen = 16;
                blockCipher = new org.bouncycastle.crypto.engines.DESedeEngine();
                break;
            case Dstu7624:
                size = 128;
                keyLen = 16;
                blockCipher = new org.bouncycastle.crypto.engines.DSTU7624Engine(size);
                break;
            case Gost28147:
                blockCipher = new org.bouncycastle.crypto.engines.GOST28147Engine();
                break;
            case Idea:
                blockCipher = new org.bouncycastle.crypto.engines.IDEAEngine();
                break;
            case Noekeon:
                size = 128;
                keyLen = 16;
                blockCipher = new org.bouncycastle.crypto.engines.NoekeonEngine();
                break;
            case RC2:
                size = 128;
                blockCipher = new org.bouncycastle.crypto.engines.RC2Engine();
                break;
            case RC532:
                blockCipher = new org.bouncycastle.crypto.engines.RC532Engine();
                break;
            case RC564:
                size = 64;
                blockCipher = new org.bouncycastle.crypto.engines.RC564Engine();
                break;
            case RC6:
                blockCipher = new org.bouncycastle.crypto.engines.RC6Engine();
                break;
            case Seed:
                blockCipher = new org.bouncycastle.crypto.engines.SEEDEngine();
                size = 128;
                keyLen = 16;
                break;
            case Serpent:
                blockCipher = new org.bouncycastle.crypto.engines.SerpentEngine();
                size = 128;
                keyLen = 16;
                break;
            case SM4:
                blockCipher = new org.bouncycastle.crypto.engines.SM4Engine();
                size = 128;
                keyLen = 16;
                break;
            case SkipJack:
                blockCipher = new org.bouncycastle.crypto.engines.SkipjackEngine();
                break;
            case Tea:
                size = 128;
                keyLen = 16;
                blockCipher = new org.bouncycastle.crypto.engines.TEAEngine();
                break;
            case Tnepres:
                size = 128;
                keyLen = 16;
                blockCipher = new org.bouncycastle.crypto.engines.TnepresEngine();
                break;
            case XTea:
                blockCipher = new org.bouncycastle.crypto.engines.XTEAEngine();
                break;
            case ZenMatrix:
                size = 16;
                keyLen = 16;
                // TODO: port it to java
                // blockCipher = new ZenMatrix(Size);
                // break;
            case ZenMatrix2:
                // throw new NotImplementedException("ZenMatrix2 IBlockCipher interface not implemented");)
                size = 32;
                keyLen = 16;
                // TODO: port it to java
                // blockCipher = new ZenMatrix2();
                // break;
            default:
                blockCipher = new org.bouncycastle.crypto.engines.AESEngine();
                break;
        }

        blockSize = blockCipher.getBlockSize();

    }


    public CryptParams(CipherEnum cipherAlgo, String secretKey, String keyHashed, KeyHash keyHash) {
        this(cipherAlgo);
        keyHashing = keyHash;
        key = (secretKey == null || secretKey.length() == 0) ? Constants.AUTHOR_EMAIL : secretKey;
        hash = (keyHashed == null || keyHashed.length() < 1) ? keyHashing.hash(secretKey) : keyHashed;
    }


    public CryptParams(CipherEnum cipherAlgo, String secretKey, KeyHash keyHash) {
        this(cipherAlgo, secretKey, keyHash.hash(secretKey), keyHash);
    }

    /***
     * constructs a  CryptParams object by {@link CipherEnum}
     * @param cipherAlgo {@link CipherEnum}
     * @param key user key for encryption {@link String}
     * @param hash hashed user key
     */
    public CryptParams(CipherEnum cipherAlgo, String secretKey, String keyHashed) {
        this(cipherAlgo);
        key = (secretKey == null || secretKey.length() == 0) ? Constants.AUTHOR_EMAIL : secretKey;
        hash = (keyHashed == null || keyHashed.length() < 1) ? keyHashing.hash(secretKey) : keyHashed;
    }

    /***
     * Constructs instance via another object instance
     * @param cryptParams another instance
     */
    public CryptParams(CryptParams cryptParams) {
        this(cryptParams.cipher, cryptParams.key, cryptParams.hash, cryptParams.keyHashing);
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
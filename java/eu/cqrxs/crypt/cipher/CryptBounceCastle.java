package eu.cqrxs.crypt.cipher;

// import static eu.cqrxs.cipherpipe.crypt.cipher.CipherEnum.CamelliaLight;
// import androidx.core.content.res.TypedArrayUtils;
// import com.google.common.primitives.Bytes;

import java.io.IOException;
import java.nio.charset.StandardCharsets;

import eu.cqrxs.crypt.encoding.EncodeEnum;
import eu.cqrxs.crypt.encoding.EnDeCodeHelper;
import eu.cqrxs.zip.ZipType;
import eu.cqrxs.zip.GZ;
import eu.cqrxs.util.DbgWriter;
import eu.cqrxs.util.Constants;

import org.bouncycastle.crypto.*;
import org.bouncycastle.crypto.engines.*;
import org.bouncycastle.crypto.BlockCipher;
import org.bouncycastle.crypto.modes.CBCBlockCipher;
import org.bouncycastle.crypto.modes.CCMBlockCipher;
import org.bouncycastle.crypto.modes.CFBBlockCipher;
import org.bouncycastle.crypto.modes.CTSBlockCipher;
import org.bouncycastle.crypto.modes.EAXBlockCipher;
import org.bouncycastle.crypto.modes.GOFBBlockCipher;
import org.bouncycastle.crypto.paddings.BlockCipherPadding;
import org.bouncycastle.crypto.paddings.PaddedBufferedBlockCipher;
import org.bouncycastle.crypto.params.ParametersWithIV;

/**
 * CryptBouncyCastle generic crypt wrapper class
 * great thanks to the legion of bouncycastle.com
 */
public class CryptBounceCastle  {

    private String privateKey = "";

    private String privateHash = "";

    private byte[] tmpIv;
    private byte[] tmpKey;

    byte[] key;
    byte[] iv;

    public int size;
    public int keyLen;
    public String mode;

    public BlockCipher CryptoBlockCipher;

    public BlockCipherPadding CryptoBlockCipherPadding;

    protected PaddedBufferedBlockCipher PadBufBChipger;


    /**
     * parameterless default constructor
     */
    public CryptBounceCastle()  {
        CryptoBlockCipher = null;
        CryptoBlockCipherPadding = null;
        keyLen = 32;
        size = 256;
        mode = "CFB";

        privateKey = "";
        privateHash = "";
        tmpKey = Constants.AES_ENVIROMENT_KEY.getBytes(StandardCharsets.UTF_8);
        tmpIv = Constants.AUTHOR_IV.getBytes(StandardCharsets.UTF_8);

        key = new byte[keyLen];
        iv = new byte[keyLen];
        System.arraycopy(tmpIv, 0, iv, 0, keyLen);
        System.arraycopy(tmpKey, 0, key, 0, keyLen);

        tmpKey = null;
        tmpIv = null;
    }


    /**
     * Generic CryptBounceCastle constructor
     * @param cparams parameters to crypt
     * @param init init first time with a new key
     */
    public CryptBounceCastle(CryptParams cparams, boolean init)  {
        CryptoBlockCipher = (cparams.blockCipher == null) ? new AESEngine() : cparams.blockCipher;
        if (CryptoBlockCipher.getAlgorithmName() == "RC564" || CryptoBlockCipher.getAlgorithmName() == "RC5-64")
            CryptoBlockCipher = new RC564Engine();
        CryptoBlockCipherPadding = new org.bouncycastle.crypto.paddings.ZeroBytePadding();
        keyLen = cparams.keyLen;
        size = Math.min(cparams.size, CryptoBlockCipher.getBlockSize());
        mode = cparams.getMode();

        if (init)
        {
            tmpKey = new byte[keyLen];
            tmpIv = new byte[keyLen];


            // throw new IllegalArgumentException("cparams.Key");

            privateKey = (cparams.key != null && cparams.key.length() > 0) ?
                    cparams.key :  Constants.AUTHOR_EMAIL;
            privateHash = (cparams.hash != null && cparams.hash.length() > 0) ?
                    cparams.hash : cparams.keyHashing.hash(cparams.key);

            tmpKey = getUserKeyBytes(privateKey, privateHash);
            // tmpKey = (privateKey.Length >= KeyLen) ? Encoding.UTF8.GetBytes(privateKey) : Encoding.UTF8.GetBytes(privateKey + privateHash);
            tmpIv = getUserKeyBytes(privateHash, privateKey);
            // tmpIv = (privateHash.Length >= KeyLen) ? Encoding.UTF8.GetBytes(privateHash) : Encoding.UTF8.GetBytes(privateHash + privateKey);

            key = new byte[keyLen];
            iv = new byte[keyLen];
            System.arraycopy(tmpIv, 0, iv, 0, keyLen);
            System.arraycopy(tmpKey, 0, key, 0, keyLen);
        }
        else
        {
            if (tmpKey == null || tmpIv == null || tmpKey.length <= 1 || tmpIv.length <= 1)
            {
                tmpKey = new byte[keyLen];
                tmpIv = new byte[keyLen];
                System.arraycopy(iv, 0, tmpIv, 0, keyLen);
                System.arraycopy(key, 0, tmpKey, 0, keyLen);
            }
        }
    }


    /**
     * getUserKeyBytes gets symmetric cipher private byte[KeyLen] encryption / decryption key
     * @param secretKey user secret key, default email address
     * @param secretHash user host ip address
     * @return array of bytes with length keyLen
     */
    protected byte[] getUserKeyBytes(String secretKey, String secretHash) {
        privateKey = secretKey;
        privateHash = secretHash;

        String keyByteHashString = privateKey;
        tmpKey = new byte[keyLen];
        tmpKey = CryptHelper.getUserKeyBytes(privateKey, privateHash, keyLen);
        if (tmpKey.length < keyLen)
            throw new IllegalArgumentException("key tmpKey.ToHexString() is shorten then KeyLen " + keyLen);

        return tmpKey;

    }
	
	
	/**
	 * canAlgoKeyIV
	 * @param cryptoBlockCipher {@link BlockCipher}
	 * @return true, if algo handles initialization with key and initialization vector
	 */
	protected boolean canAlgoKeyIV(BlockCipher cryptoBlockCipher)
	{
		if (cryptoBlockCipher == null)
			cryptoBlockCipher = CryptoBlockCipher;
		String algoName = cryptoBlockCipher.getAlgorithmName().toUpperCase();
		if (algoName.startsWith("AES") || algoName.startsWith("RIJNDAEL"))
			return false;
		if (algoName.startsWith("ARIA") || algoName.startsWith("ASCON"))
			return false;
		if (algoName.startsWith("CAST") || algoName.startsWith("GOST"))
			return false;
		if (algoName.startsWith("RC") || algoName.startsWith("IDEA"))
			return false;
		if (algoName.startsWith("SKIPJACK") || algoName == "DESEDE")
			return false;
		if (algoName.startsWith("TEA") || algoName.startsWith("XTEA"))
			return false;
		return true;
	}



    /***
     *  Generic CryptBounceCastle encrypt member function
     *  difference between out parameter encryptedData and return value, are 2 different encryption methods, but with the same result at the end
     * @param plainData plain data byte[] array
     * @return encrypted byte[] array
     */
    public byte[] encrypt(byte[] plainData)  throws InvalidCipherTextException {
        var cipher = CryptoBlockCipher;
		plainData = (CryptoBlockCipher.getAlgorithmName() == "RC564" || CryptoBlockCipher.getAlgorithmName() == "RC5-64") ?
			EnDeCodeHelper.getBytesFromBytes(plainData, 64, true) : plainData;
        PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CFBBlockCipher(CryptoBlockCipher, size), CryptoBlockCipherPadding);

        switch (mode)
        {
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CBCBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                break;
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(CryptoBlockCipher, CryptoBlockCipherPadding);
                break;
            case "CFB":
                cipherMode = new PaddedBufferedBlockCipher(new CFBBlockCipher(CryptoBlockCipher, size), CryptoBlockCipherPadding);
                break;
            // case "CCM":
            //     org.bouncycastle.crypto.modes.CCMBlockCipher ccmCipher = new CCMBlockCipher(CryptoBlockCipher);
            //     cipherMode = new PaddedBufferedBlockCipher((BlockCipher)ccmCipher, CryptoBlockCipherPadding);
            //     break;
            case "CTS":
                org.bouncycastle.crypto.modes.CTSBlockCipher ctsCipher = new CTSBlockCipher(CryptoBlockCipher);
                cipherMode = new PaddedBufferedBlockCipher((BlockCipher)ctsCipher, CryptoBlockCipherPadding);
                break;
            case "EAX":
                org.bouncycastle.crypto.modes.EAXBlockCipher eaxCipher = new EAXBlockCipher(CryptoBlockCipher);
                cipherMode = new PaddedBufferedBlockCipher((BlockCipher)eaxCipher, CryptoBlockCipherPadding);
                break;
            case "GOFB":
                org.bouncycastle.crypto.modes.GOFBBlockCipher gOfbCipher = new GOFBBlockCipher(CryptoBlockCipher);
                cipherMode = new PaddedBufferedBlockCipher((BlockCipher)gOfbCipher, CryptoBlockCipherPadding);
                break;
            default:
                break;
        }

        CipherParameters keyParam;
		if (CryptoBlockCipher.getAlgorithmName() == "RC564" || CryptoBlockCipher.getAlgorithmName() == "RC5-64") 
            keyParam = new org.bouncycastle.crypto.params.RC5Parameters(key, 2);
		else 
			keyParam = new org.bouncycastle.crypto.params.KeyParameter(key);
        CipherParameters keyParamIV = new org.bouncycastle.crypto.params.ParametersWithIV(keyParam, iv);

        if (mode == "ECB" || !canAlgoKeyIV(CryptoBlockCipher))
			cipherMode.init(true, keyParam);
        else
			cipherMode.init(true, keyParamIV);

        if (PadBufBChipger == null && cipherMode != null)
            PadBufBChipger = cipherMode;

        // encryptedData = cipherMode.ProcessBytes(plainData);

        int outputSize = cipherMode.getOutputSize(plainData.length);
        byte[] cipherData = new byte[outputSize];
        int result = cipherMode.processBytes(plainData, 0, plainData.length, cipherData, 0);
        cipherMode.doFinal(cipherData, result);

        return cipherData;
    }

    /**
     * Generic CryptBounceCastle decrypt member function
     * difference between out parameter decryptedData and return value, are 2 different decryption methods, but with the same result at the end
     * @param cipherData encrypted byte[] arrey
     * @return decrypted plain byte[] data
     */
    public byte[] decrypt(byte[] cipherData) throws InvalidCipherTextException {
        var cipher = CryptoBlockCipher;
        PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CFBBlockCipher(CryptoBlockCipher, size), CryptoBlockCipherPadding);

        switch (mode)
        {
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CBCBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                break;
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(CryptoBlockCipher, CryptoBlockCipherPadding);
                break;
            case "CFB":
                cipherMode = new PaddedBufferedBlockCipher(new CFBBlockCipher(CryptoBlockCipher, size), CryptoBlockCipherPadding);
                break;
            // case "CCM":
            //     org.bouncycastle.crypto.modes.CCMBlockCipher ccmCipher = new CCMBlockCipher(CryptoBlockCipher);
            //     cipherMode = new PaddedBufferedBlockCipher((BlockCipher)ccmCipher, CryptoBlockCipherPadding);
            //     break;
            case "CTS":
                org.bouncycastle.crypto.modes.CTSBlockCipher ctsCipher = new CTSBlockCipher(CryptoBlockCipher);
                cipherMode = new PaddedBufferedBlockCipher((BlockCipher)ctsCipher, CryptoBlockCipherPadding);
                break;
            case "EAX":
                org.bouncycastle.crypto.modes.EAXBlockCipher eaxCipher = new EAXBlockCipher(CryptoBlockCipher);
                cipherMode = new PaddedBufferedBlockCipher((BlockCipher)eaxCipher, CryptoBlockCipherPadding);
                break;
            case "GOFB":
                org.bouncycastle.crypto.modes.GOFBBlockCipher gOfbCipher = new GOFBBlockCipher(CryptoBlockCipher);
                cipherMode = new PaddedBufferedBlockCipher((BlockCipher)gOfbCipher, CryptoBlockCipherPadding);
                break;
            default:
                break;
        }
        // cipherMode.Reset()

		CipherParameters keyParam;
		if (CryptoBlockCipher.getAlgorithmName() == "RC564" || CryptoBlockCipher.getAlgorithmName() == "RC5-64") 
            keyParam = new org.bouncycastle.crypto.params.RC5Parameters(key, 2);
		else 
			keyParam = new org.bouncycastle.crypto.params.KeyParameter(key);
        
        CipherParameters keyParamIV = new ParametersWithIV(keyParam, iv);

        // Decrypt with initialization vector only when !ECB + algorithm is IV capable
		if (mode == "ECB" || !canAlgoKeyIV(CryptoBlockCipher))
			cipherMode.init(false, keyParam);
        else
			cipherMode.init(false, keyParamIV);

        // decryptedData = cipherMode.ProcessBytes(cipherData);
        if (cipherMode != null)
            PadBufBChipger = cipherMode;

        int result = 0, bs = 0;
        int outputSize = cipherMode.getOutputSize(cipherData.length);
        byte[] plainData = new byte[outputSize];
        byte[] decryptedData = new byte[outputSize];
        try
        {
            result = cipherMode.processBytes(cipherData, 0, cipherData.length, plainData, 0);
            cipherMode.doFinal(plainData, result);
        }
        catch (Exception exDecrypt)
        {
			DbgWriter.msg("CryptBounceCastle " + CryptoBlockCipher.getAlgorithmName() + ": Exceptíon on decrypting final block" + exDecrypt.toString(), false);
            try
            {
                plainData = new byte[outputSize];
                result = cipherMode.processBytes(cipherData, 0, cipherData.length, plainData, 0);
            }
            catch (Exception exDecrypt2)
            {
				DbgWriter.msg("CryptBounceCastle " + CryptoBlockCipher.getAlgorithmName() + ": Exceptíon on 2x decrypting final block: " + exDecrypt2.toString(), false);
                // plainData = new byte[outputSize];
                bs = cipherMode.doFinal(plainData, result);
            }
        }

		if (CryptoBlockCipher.getAlgorithmName() == "RC564" || CryptoBlockCipher.getAlgorithmName() == "RC5-64") 
			return EnDeCodeHelper.getBytesTrimNulls(plainData);

        return plainData;
    }

    /***
     * Generic CryptBounceCastle encryptString method
     * @param inString plain String to encrypt
     * @param encodingType {@link EncodeEnum} beware to use uu in TestWebForm in C#
     *                                       because Form validation thinks,
     *                                       that brackets are HTML, XML injection
     * @return encoded encrypted String, default base64 encoded
     */
    public String encryptString(String inString, EncodeEnum encodingType) throws InvalidCipherTextException, IOException {
        byte[] plainTextData = inString.getBytes(StandardCharsets.UTF_8);
        byte[] encryptedBytes = encrypt(plainTextData);
        String encryptedString = encodingType.encodeBytesToString(encryptedBytes);

        return encryptedString;
    }


    /***
     * Generic CryptBounceCastle decryptString method
     * @param inCryptString encoded encrypted String, default base64 encoded
     * @param encodingType {@link EncodeEnum}
     * @return plain text decrypted String
     */
    public String decryptString(String inCryptString, EncodeEnum encodingType) throws InvalidCipherTextException, IOException {
        byte[] cipherBytes = encodingType.decodeStringToBytes(inCryptString);
        byte[] plainData = decrypt(cipherBytes);
        String plainTextString = plainData.toString();
        // nDeCodeHelper.GetString(plainData).TrimEnd('\0');

        return plainTextString;
    }


}

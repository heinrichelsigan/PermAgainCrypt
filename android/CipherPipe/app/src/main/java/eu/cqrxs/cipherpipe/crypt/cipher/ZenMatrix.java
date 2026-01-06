package eu.cqrxs.cipherpipe.crypt.cipher;

// import static eu.cqrxs.cipherpipe.crypt.cipher.CipherEnum.CamelliaLight;
// import androidx.core.content.res.TypedArrayUtils;
// import com.google.common.primitives.Bytes;

import android.renderscript.RSInvalidStateException;
import com.google.ai.client.generativeai.common.InvalidStateException;

import eu.cqrxs.cipherpipe.crypt.encoding.EncodeEnum;
import eu.cqrxs.cipherpipe.crypt.encoding.Hex16Coder;
import eu.cqrxs.cipherpipe.crypt.hash.KeyHash;
import eu.cqrxs.cipherpipe.zip.ZipType;
import eu.cqrxs.cipherpipe.zip.*;
import eu.cqrxs.cipherpipe.util.Constants;
import eu.cqrxs.cipherpipe.util.*;

import java.io.ByteArrayOutputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.HexFormat;
import java.util.List;

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
import org.bouncycastle.crypto.params.KeyParameter;
import org.bouncycastle.crypto.params.ParametersWithIV;
import org.bouncycastle.jcajce.provider.symmetric.AES;
import org.bouncycastle.crypto.modes.CBCBlockCipher;
import org.bouncycastle.crypto.modes.CCMBlockCipher;
import org.bouncycastle.crypto.modes.CFBBlockCipher;
import org.bouncycastle.crypto.modes.CTSBlockCipher;
import org.bouncycastle.crypto.modes.EAXBlockCipher;
import org.bouncycastle.crypto.modes.GOFBBlockCipher;
import org.bouncycastle.crypto.paddings.BlockCipherPadding;
import org.bouncycastle.crypto.paddings.PaddedBufferedBlockCipher;
import org.bouncycastle.crypto.params.KeyParameter;
import org.bouncycastle.crypto.params.ParametersWithIV;
import org.bouncycastle.jcajce.provider.symmetric.AES;


/**
 * CryptBouncyCastle generic crypt wrapper class
 * great thanks to the legion of bouncycastle.com
 */
public class ZenMatrix implements BlockCipher  {

    private final static String SYMMCIPHERALGONAME = "ZenMatrix";
    public final static int ZEN_SIZE = 0x10;
    static int BLOCK_SIZE = 256;
    final static int[] BLOCK_SIZES = { 16, 64, 128, 256, 1024, 4096, 16384, 65536 };
    boolean initialised = false;
    boolean forEncryption;

    final static byte[] matrixPermutationBase = {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7,
            0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
    };
    final static int[] magicOrder = {
            0x8,    0x3,    0x1,    0xe,
            0x9,    0xf,    0x5,    0xc,
            0x4,    0xd,    0xa,    0x7,
            0xb,    0x2,    0x0,    0x6
    };
    protected byte[] privateBytes = new byte[0x10];

    public byte[] matrixPermutationKey;
    protected byte[] _inverseMatrix = new byte[0];

    protected byte[] getInverseMatrix()
    {
        if (_inverseMatrix == null ||
                _inverseMatrix.length < 0x10 ||
                (_inverseMatrix[0] == (byte)0x0 && _inverseMatrix[1] == (byte)0x0 && _inverseMatrix[0xf] == (byte)0x0) ||
                (_inverseMatrix[0] == (byte)0x0 && _inverseMatrix[1] == (byte)0x1 && _inverseMatrix[0xf] == (byte)0xf))
        {
            _inverseMatrix = ZenMatrix.buildInverseMatrix(matrixPermutationKey, 0x10);
        }

        return _inverseMatrix;
    }

    public HashSet<Byte> permutationKeyHash;

    /* #region IBlockCipher interface  */

    public String getAlgorithmName()  {
        return SYMMCIPHERALGONAME;
    }

    public int getBlockSize() {
        return BLOCK_SIZE;
    }


    @Override
    public void reset() {

    }


    public void init(boolean forEncryption, CipherParameters parameters)  {
        if (!(parameters instanceof KeyParameter) && !(parameters instanceof ParametersWithIV))
            throw new IllegalArgumentException("parameters: only KeyParameter or ParametersWithIV expected.");

        try  {
            this.privateBytes = ((KeyParameter)parameters).getKey();
        }  catch (Exception ex) {
            ex.printStackTrace();
        }
        if (parameters instanceof  ParametersWithIV) {
            byte[] bKey = new byte[0];
            if (((ParametersWithIV) parameters).getParameters() instanceof KeyParameter)
                bKey = ((KeyParameter) (((ParametersWithIV) parameters).getParameters())).getKey();
            byte[] bIv = ((ParametersWithIV) parameters).getIV();

            bKey = (bKey == null || bKey.length == 0) ? new byte[0] : bKey;
            bIv = (bIv == null || bIv.length == 0) ? new byte[0] : bIv;
            if (bKey.length == 0 && bIv.length == 0)
                throw new IllegalArgumentException("parameters: KeyParameter and/or ParametersWithIV contain a null or empty key or iv.");

            this.privateBytes = CryptHelper.tarBytes(bKey, bIv);
        }

        this.forEncryption = forEncryption;

        // ZenMatrixGenWithBytes(privateBytes, false);
        initialised = true;
    }

    /**
     * Processes one BLOCK with BLOCK_SIZE <see cref="BLOCK_SIZE"/>
     * @param inBuf in bytes buffer
     * @param inOff in bytes offset
     * @param outBuf out bytes buffer
     * @param outOff out bytes offset
     * @return BLOCKSIZE of processed bytes or when no bytes processed ß
     * @throws RuntimeException
     */
    @Override
    public int processBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff)
            throws DataLengthException, IllegalStateException {
        if (privateBytes == null)
            throw new RuntimeException(SYMMCIPHERALGONAME + " engine not initialised");

        // int len = BLOCK_SIZE;
        int aCnt = 0, bCnt = 0;

        if (inOff >= inBuf.length || inOff + BLOCK_SIZE > inBuf.length)
            throw new RuntimeException("Cannot process next " +  BLOCK_SIZE + " bytes, because inOff (" + inOff + ") + BLOCK_SIZE (" + BLOCK_SIZE + ") > inBuf.length " + inBuf.length + ")");
        if (outOff >= outBuf.length || outOff + BLOCK_SIZE > outBuf.length)
            throw new RuntimeException("Cannot process next " + BLOCK_SIZE + " bytes, because inOff (" + outOff + ") + BLOCK_SIZE (" + BLOCK_SIZE + ") > outBuf.length (" + outBuf.length + ")");

        if (inOff < inBuf.length && inOff + BLOCK_SIZE <= inBuf.length && outOff < outBuf.length && outOff + BLOCK_SIZE <= outBuf.length) {
            byte[] inOffBuf = new byte[inBuf.length - inOff];
            System.arraycopy(inBuf, inOff, inOffBuf, 0, inOffBuf.length);

            if (forEncryption)  {
                byte[] padBytes = padBuffer(inOffBuf, true);
                inOffBuf = padBytes;
            }

            if (BLOCK_SIZE > inOffBuf.length)
                throw new RuntimeException(BLOCK_SIZE + "> inOffBuf.length = " + inOffBuf.length + ".");

            byte[] processed = new byte[BLOCK_SIZE];

            for (aCnt = 0, bCnt = 0; aCnt < BLOCK_SIZE; aCnt++)  {
                byte b = inOffBuf[aCnt];
                byte mappedByte;
                mappedByte = mapByteValue(b, forEncryption)[0];
                byte sm = forEncryption ? matrixPermutationKey[aCnt % 0x10] : _inverseMatrix[aCnt % 0x10];
                int pos = bCnt + ((int)sm) % 0x10;
                processed[pos] = mappedByte;
                if (aCnt != 0 && aCnt % 0x10 == 0)
                    bCnt += 0x10;
            }

            // byte[] outBytes = processed;
            // if (!forEncryption)
            //    outBytes = PadBuffer(processed);
            // Array.Copy(outBytes, 0, outBuf, outOff, BLOCK_SIZE);

            System.arraycopy(processed, 0, outBuf, outOff, BLOCK_SIZE);

            return BLOCK_SIZE;
        }

        return 0;
    }

        /*
        public int processBlock(ReadOnlySpan<byte> input, Span<byte> output) {
            int aCnt = 0, bCnt = 0;
            byte[] buffer = input.ToArray();
            if (forEncryption)                                  // add padding buffer to match BLOCK_SIZE
            {
                byte[] padBytes = PadBuffer(input.ToArray());
                buffer = padBytes;
            }

            if (BLOCK_SIZE > buffer.Length)
                throw new InvalidOperationException($"{BLOCK_SIZE} > buffer.Length = {buffer.Length}");

            byte[] processed = new byte[BLOCK_SIZE];

            for (aCnt = 0, bCnt = 0; aCnt < BLOCK_SIZE; aCnt++)
            {
                byte b = buffer[aCnt];
                byte mappedByte = mapByteValue(b, forEncryption)[0];
                byte sm = forEncryption ? matrixPermutationKey[aCnt % 0x10] : _inverseMatrix[aCnt % 0x10];
                int pos = bCnt + ((int)sm) % 0x10;
                processed[pos] = mappedByte;
                if (aCnt != 0 && aCnt % 0x10 == 0)
                    bCnt += 0x10;
            }

            // byte[] outBytes = processed;
            // if (!forEncryption)                             // trim padding buffer from decrypted output
            //     outBytes = PadBuffer(processed);
            // output = new Span<byte>(outBytes);

            output = new Span<byte>(processed);

            return BLOCK_SIZE;
        }
        */
    /* #endregion IBlockCipher interface */


    /* #region ctor_init_gen_reverse */

    /**
     * public constructor
     * @param bs
     */
    public ZenMatrix(int bs)  {
        for (int i = 0; i < BLOCK_SIZES.length; i++)  {
            if (bs == BLOCK_SIZES[i])
                BLOCK_SIZE = BLOCK_SIZES[i];
        }
        byte sbcnt = 0x0;
        matrixPermutationKey = new byte[ZEN_SIZE];
        for (byte s : matrixPermutationBase)  {
            privateBytes[sbcnt % ZEN_SIZE] = (byte)0x0;
            matrixPermutationKey[sbcnt++] = s;
        }

        permutationKeyHash = new HashSet<Byte>();
        for (int khidx = 0; khidx < matrixPermutationBase.length; khidx++) {
            permutationKeyHash.add(Byte.valueOf(matrixPermutationKey[khidx]));
        }
        _inverseMatrix = buildInverseMatrix(matrixPermutationBase, 0x10);
    }


    /**
     * initializes a {@link ZenMatrix} with secret user key string and hash iv
     * @param secretKey user's secret key
     * @param hashIV private key hash iv string
     * @param fullSymmetric fullSymmetric means that zen matrix is it's inverse element and decrypts back to plain text, when encrypting twice or ²
     */
    public ZenMatrix(String secretKey, String hashIV, boolean fullSymmetric) {
        this(16);
        if (secretKey.isEmpty())
            throw new IllegalArgumentException("secretKey is null or empty");

        byte[] keyBytes = CryptHelper.getUserKeyBytes(secretKey, hashIV, 0x10);

        genBuildWithBytes(keyBytes, fullSymmetric);
    }


    /**
     * initializes a {@link ZenMatrix} with secret user key string and hash iv
     * @param secretKey user's secret key
     * @param hashIV private key hash iv string
     * @param fullSymmetric fullSymmetric means that zen matrix is it's inverse element and decrypts back to plain text, when encrypting twice or ²
     * @param keyHash {@link KeyHash} is used, when hash iv is null or empty to get new hash iv from user key
     */
    public ZenMatrix(String secretKey, String hashIV, boolean fullSymmetric, KeyHash keyHash) {
        this(secretKey, String.valueOf(hashIV.isEmpty() ? keyHash.hash(secretKey) : hashIV), fullSymmetric);
    }


    /***
     * initializes a {@link ZenMatrix} with an array of key bytes
     * @param keyBytes user key bytes to init algorithm instance
     * @param fullSymmetric if true inverse matrix is same as encrypt matrix V * M * M = V * M * MInverse = V
     */
    public ZenMatrix(byte[] keyBytes, boolean fullSymmetric) {
        this(16);
        genBuildWithBytes(keyBytes, fullSymmetric);
    }


    /**
     * InitMatrixSymChiffer - base initialization of variables, needed for matrix sym chiffer encryption
     */
    private void initMatrixSymChiffer() {
        byte sbcnt = 0x0;
        matrixPermutationKey = new byte[0x10];
        for (byte s : matrixPermutationBase)  {
            privateBytes[sbcnt % 0x10] = (byte)0x0;
            matrixPermutationKey[sbcnt++] = s;
        }
        permutationKeyHash = new HashSet<Byte>();
        for (int khidx = 0; khidx < matrixPermutationKey.length; khidx++) {
            permutationKeyHash.add((Byte.valueOf(matrixPermutationKey[khidx])));
        }
        _inverseMatrix = buildInverseMatrix(matrixPermutationKey, 0x10);
    }


    /**
     * Generates / builds a ZenMatrix with key bytes
     * @param keyBytes users keybytes created by users key and key hash iv
     *                  must have at least 4 bytes and will be truncated after
     *                  16 bytes only the first 16 bytes will be taken from keyBytes for {@link ZenMatrix}
     * @param fullSymmetric fullSymmetric means that zen matrix is it's inverse element and decrypts back to plain text, when encrypting twice or ²
     * @return true, if init with key bytes was successful
     */
    protected void genBuildWithBytes(byte[] keyBytes, boolean fullSymmetric)  {
        if ((keyBytes == null || keyBytes.length < 4))
            throw new RuntimeException("byte[] keyBytes is null or keyBytes.Length < 4");

        // InitMatrixSymChiffer();
        int ba = 0, bb = 0;
        System.arraycopy(keyBytes, 0, privateBytes, 0, Math.min(keyBytes.length, 0x10));

        permutationKeyHash = new HashSet<Byte>();

        // MatrixDict is only needed, when (fullSymmetric == true)
        HashMap<Byte, Byte> matrixDict = new HashMap<Byte, Byte>();

        // Simplest method to fill deterministic up privateBytes from keyBytes with keyBytes.Length < 16
        // for (int i = keyBytes.Length; i < 0x10; i++)
        // {
        //       if (i < 0x08)
        //          privateBytes[i] = (byte)keyBytes[i % keyBytes.Length];
        //      else
        //          privateBytes[i] = (byte)keyBytes[0x08 - (i - 0x07)];
        // }
        //

        for (byte keyByte : privateBytes)
        {
            byte b = (byte)(keyByte % 0x10);
            for (int i = 0; i < 0x20; i++)
            {
                if (permutationKeyHash.contains(b) || ((int)b) == ba)
                    b = (i >= 0x10) ? ((byte)(((int)(keyByte) + i) % 0x10)) :
                            ((byte)(((int)(keyByte) + magicOrder[i]) % 0x10));
                else break;
            }

            if (!permutationKeyHash.contains(b))
            {
                bb = (int)b;
                if (ba != bb)
                {
                    if (fullSymmetric)
                    {
                        if (!matrixDict.keySet().contains(Byte.valueOf(b)) &&
                                !matrixDict.containsValue((Byte.valueOf((byte)ba)))) {
                            matrixDict.put(
                                    Byte.valueOf((byte)ba), Byte.valueOf((byte)bb));
                            matrixDict.put(
                                    Byte.valueOf((byte)ba), Byte.valueOf((byte)bb));
                        }
                    }

                    permutationKeyHash.add(b);
                    // TODO:

                    // swap(matrixPermutationKey, ba, bb);
                    swapValue(matrixPermutationKey, (byte)ba, (byte)bb);
                    //matrixPermutationKey = matrixPermutationKey.SwapTPositions<sbyte>(ba, bb);
                    ba++;
                }
            }
        }

        if (fullSymmetric) {
            /* #region fullSymmetric => InverseMatrix = MatrixPermutationKey; */
            if (matrixDict.size() < 0x0f)  {
                for (int k = 0; k < 0x10; k++) {
                    if (!matrixDict.keySet().contains(Byte.valueOf((byte)k))) {
                        for (int l = 0x0f; l >= 0; l--)  {
                            if (matrixDict.values().contains(Byte.valueOf((byte)l)))
                                continue;

                            matrixDict.put((byte)k, (byte)l);
                            if (!matrixDict.keySet().contains(Byte.valueOf((byte)l)))
                                matrixDict.put((byte)l, (byte)k);
                            break;
                        }
                    }
                }
            }
            if (matrixDict.size() == 0x10) {
                byte bKey, bValue;
                permutationKeyHash.clear();;
                for (int n = 0; n < 0x10; n++) {
                    bKey = Byte.valueOf((byte)n);
                    Byte mtrb = matrixDict.get(bKey);
                    bValue = (byte)mtrb.byteValue();
                    permutationKeyHash.add(bValue);
                    matrixPermutationKey[(int)bKey] = bValue;
                    matrixPermutationKey[(int)bValue] = bKey;
                }
            }
            /* #endregion fullSymmetric => InverseMatrix = MatrixPermutationKey; */

            _inverseMatrix = matrixPermutationKey;
        }  else  {
            /* #region bugfix for missing permutations */
            byte[] strikeBytes = {  (byte)0x0, (byte)0x1, (byte)0x2, (byte)0x3, (byte)0x4, (byte)0x5, (byte)0x6, (byte)0x7,
                    (byte)0x8, (byte)0x9, (byte)0xa, (byte)0xb, (byte)0xc, (byte)0xd, (byte)0xe, (byte)0xf  };
            HashSet<Byte> strikeList = new HashSet<Byte>();
            for (int sl = 0; sl < strikeBytes.length; sl++) {
                strikeList.add(Byte.valueOf(strikeBytes[sl]));
            }



            for (int i = 0; i < 0x10; i++)  {
                if ((permutationKeyHash.size() <= i) && strikeList.size() > 0) {
                    Byte[] strikeArray = strikeList.toArray(Byte[]::new);
                    permutationKeyHash.add(strikeArray[0]);
                }

                Byte inByte = Byte.valueOf((byte)i);
                if ((permutationKeyHash.toArray(Byte[]::new))[1] != i)  {
                    inByte = (permutationKeyHash.toArray(Byte[]::new))[1];
                    matrixPermutationKey[i] = inByte.byteValue();
                }
                if (strikeList.contains(inByte))
                    strikeList.remove(inByte);
            }

            _inverseMatrix = buildInverseMatrix(matrixPermutationKey, 0x10);
            /* #endregion bugfix for missing permutations */
        }

        String perm = "", kbs = "";

        for (int j = 0; j < 0x10; j++)
            perm += String.format("%x", matrixPermutationKey[j]);
        for (int j = 0; j < keyBytes.length; j++)
            kbs += String.format("%x2", keyBytes[j]);


        initialised = true;
        (new eu.cqrxs.cipherpipe.util.DbgWriter()).msg("ZenMatrix" +  perm + " KeyBytes = " + kbs, 2, true);
    }


    /* #endregion ctor_init_gen_reverse */


    /* #region ProcessEncryptDecryptBytes */



    /***
     * ProcessBytes processes bytes for encryption or decryption depending on {@link forEncryption}
     *         processes the next len=16 bytes to encrypt, starting at offSet
     *         or processes the next len=16 bytes to decrypt, starting at offSet
     * @param inBytes bytes array to encrypt
     * @param offSet starting offSet
     * @param len of byte block (default 16)
     * @return byte[len] (default: 16) segment of encrypted bytes
     */
    protected  byte[] processBytes(byte[] inBytes, int offSet, int len) {
        int aCnt = 0, bCnt = 0;
        if (offSet < inBytes.length && offSet + len <= inBytes.length) {
            byte[] processed = new byte[len];
            for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++) {
                byte b = inBytes[bCnt];
                byte mappedByte = mapByteValue(b, forEncryption)[0];
                byte pos = (forEncryption) ? matrixPermutationKey[aCnt % 0x10] : _inverseMatrix[aCnt % 0x10];
                processed[(int)pos] = mappedByte;
            }

            return processed;
        }

        return new byte[0];
    }



    /* #endregion ProcessEncryptDecryptBytes */

    /* #region encrypt decrypt */

    /**
     * in case of encryption,
     *         pads 0 or random buffer at end of inBytes,
     *         so that inBytes % BLOCK_SIZE == 0
     *         in case of decryption,
     *         trims remaining padding buffer from inBytes
     *         encryption or decryption are triggered via {@link forEncryption}
     * @param inBytes input bytes to pad
     * @param useRandom >use random padding
     * @return padded or unpadded out bytes
     */
    public byte[] padBuffer(byte[] inBytes, boolean useRandom) {
        int ilen = inBytes.length;                          // length of data bytes
        int oSize = (BLOCK_SIZE - (ilen % BLOCK_SIZE));     // oSize is rounded up to next number % BLOCK_SIZE == 0
        byte[] outBytes;

        if (forEncryption)  {                               // add buffer for encryption to inbytes
            int olen = ((int)(ilen + oSize));             // olen is (long)(ilen + oSize)
            byte[] padbuf = new byte[oSize];                // padding buffer
            outBytes = new byte[olen];                      // out bytes with random padding bytes at end

            if (!useRandom)
                for (int ic = 0; ic < padbuf.length; padbuf[ic++] = (byte)0) ;
            else  {
                java.util.Random rnd = new java.util.Random(ilen);
                rnd.nextBytes(padbuf);
            }

            for (int i = 0, j = 0; i < olen; i++) {
                // outBytes[i] = (i < ilen) ? inBytes[i] : ((i == ilen || i == (olen - 1)) ? (byte)0x0 : buf[j++]);
                if (i < ilen)
                    outBytes[i] = inBytes[i];               // copy full inBytes to outBytes
                else if (i == ilen)
                    outBytes[i] = (byte)0x0;                // write 0x0 at end of inBytes
                else if (i > ilen)
                    outBytes[i] = padbuf[j++];              // fill rest with padding buffer
                else if (i == (olen - 1))
                    outBytes[i] = (byte)0x0;                // terminate outBytes with NULL
            }
        } else {                                            // truncate padding buffer to get trimmed decrypted output

            int olen = inBytes.length;
            boolean last0 = false;

            for (olen = ilen; (olen > 0 && !last0); olen--) {
                if (olen < (ilen - 2))  {
                    if ((inBytes[olen - 1] == (byte)0x0) && inBytes[olen - 2] != (byte)0x0)  {
                        last0 = true;
                        break;
                    }
                }
            }

            outBytes = (olen > 1) ? new byte[olen] : new byte[ilen];
            System.arraycopy(inBytes, 0, outBytes, 0, outBytes.length);
        }

        return outBytes;

    }

    /**
     * MatrixSymChiffer Encrypt member function
     * @param pdata plain data as
     * @return encrypted data
     */
    public byte[] encrypt(byte[] pdata) {
        // Check arguments.
        if (pdata == null || pdata.length <= 0)
            throw new IllegalArgumentException("ZenMatrix byte[] Encrypt(byte[] pdata): ArgumentNullException pdata = null or Lenght 0.");

        forEncryption = true;
        byte[] obytes = padBuffer(pdata, false);

        List<Byte> encryptedBytes = new ArrayList<Byte>();
        for (int i = 0; i < obytes.length; i += 0x10)  {
            for (byte pb : processBytes(obytes, i, 0x10))  {
                encryptedBytes.add(Byte.valueOf(pb));
            }
        }

        byte[] retbytes = new byte[encryptedBytes.size()];
        int ib = 0;
        for (Byte bb : encryptedBytes) {
            retbytes[ib++] = (byte)(bb.byteValue());
        }

        return retbytes; // encryptedBytes.toArray();
    }


    /**
     * decrypt
     * @param ecdata encrypted byte array
     * @return decrypted plain bytes
     */
    public  byte[] decrypt(byte[] ecdata)
    {
        if (ecdata == null || ecdata.length <= 0)
            throw new IllegalArgumentException("ZenMatrix byte[] Encrypt(byte[] ecdata): ArgumentNullException ecdata = null or lenght 0.");

        forEncryption = false;
        int eclen = ecdata.length;

        List<Byte> decBytes = new ArrayList<Byte>();
        for (int pc = 0; pc < ecdata.length; pc += 16)  {
            for (byte rb : processBytes(ecdata, pc, 16))
            {
                decBytes.add(Byte.valueOf(rb));
            }
        }

        byte[] outBytes = new byte[decBytes.size()];
        int ib = 0;
        for (Byte bb : decBytes) {
            outBytes[ib++] = (byte)(bb.byteValue());
        }

        byte[] retbytes = padBuffer(outBytes, false);

        return retbytes;
    }


    /* #endregion encrypt decrypt */


    /* #region static helpers swap byte and SwapT{T} generic */



    /**
     * buildInverseMatrix, builds the determinant decryption matrix for sbyte[16] encryption matrix
     * @param matrix byte[16] encryption matrix
     * @param size byte size
     * @return byte[16] decryption matrix (determinante)
     */
    public static byte[] buildInverseMatrix(byte[] matrix, int size) {
        size = (size < 0x10) ? 0x10 : size;
        if (matrix != null && matrix.length == size)  {
            byte[] inverseM = new byte[size];
            for (int m = 0; m < size; m++)  {
                byte sm = matrix[m];
                inverseM[(int)sm] = (byte)m;
            }
            return inverseM;
        }
        String msg = "byte[] matrix is null or matrix.Length != " + String.valueOf(size) + ".";
        throw new NotImplementedError("ZenMatrix");
    }


    /**
     * MapByteValue splits a byte in 2 0x0 - 0xf segments and map both trough <see cref="MatrixPermutationKey"/> in case of encrypt,
     * @param inByte  in byte to map<
     * @param encrypt   mapped out byte
     * @return byte[]
     */
    protected  byte[] mapByteValue(byte inByte,  boolean encrypt) {
        byte[] outSBytes = new byte[4];
        // ArrayList<Byte> outSBytes = new ArrayList<Byte>(2);
        byte outByte;
        byte lsbIn = (byte)((short)inByte % 0x10);
        byte msbIn = (byte)((short)((short)inByte / 0x10));
        byte lsbOut, msbOut;
        if (encrypt)
        {
            lsbOut = matrixPermutationKey[(int)lsbIn];
            msbOut = matrixPermutationKey[(int)msbIn];
            outSBytes[1] = (byte)0;
            outSBytes[2] = msbOut;
            outSBytes[3] = lsbOut;
            outByte = (byte)((short)(((short)msbOut * 0x10) + ((short)lsbOut)));
            outSBytes[0] = outByte;
        }
        else // if decrypt
        {
            lsbOut = _inverseMatrix[(int)lsbIn];
            msbOut = _inverseMatrix[(int)msbIn];
            outSBytes[1] = (byte)0;
            outSBytes[2] = msbOut;
            outSBytes[3] = lsbOut;
            outByte = (byte)((short)(((short)msbOut * 0x10) + ((short)lsbOut)));
            outSBytes[0] = outByte;
        }

        return outSBytes;
    }

    static void swap(byte[] arr, int i, int j) {
        // error checking
        if (arr == null || i == j) {
            return;
        }
        if (i < 0 || j < 0 || i > arr.length - 1 || j > arr.length - 1) {
            return;
        }
        // looks good, swap the values
        byte t = arr[i];
        arr[i] = arr[j];
        arr[j] = t;
    }


    static void swapValue(byte[] arr, byte b, byte d) {
        // error checking
        if (arr == null || b == d) {
            return;
        }
        int i = -1, j = -1;
        for (i = 0; i < arr.length; i++) {
            if (arr[i] == b)
                break;
        }

        for (j = 0; j < arr.length; j++) {
            if (arr[j] == d)
                break;
        }
        if (i != arr.length && j != arr.length) {
            // looks good, swap the values
            byte t = arr[i];
            arr[i] = arr[j];
            arr[j] = t;
        }
    }



    /* #endregion static helpers swap byte and SwapT{T} generic */

}

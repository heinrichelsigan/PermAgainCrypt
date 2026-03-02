package eu.cqrxs.crypt.cipher;

// import static eu.cqrxs.crypt.cipher.CipherEnum.CamelliaLight;
// import androidx.core.content.res.TypedArrayUtils;
// import com.google.common.primitives.Bytes;

// import android.renderscript.RSInvalidStateException;

// import com.google.ai.client.generativeai.common.InvalidStateException;


import eu.cqrxs.crypt.hash.KeyHash;
import eu.cqrxs.util.Constants;
import eu.cqrxs.util.DbgWriter;
import eu.cqrxs.util.NotImplementedError;
import eu.cqrxs.zip.ZipType;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;

import org.bouncycastle.crypto.*;
import org.bouncycastle.crypto.BlockCipher;
import org.bouncycastle.crypto.params.KeyParameter;
import org.bouncycastle.crypto.params.ParametersWithIV;

/**
 * CryptBouncyCastle generic crypt wrapper class
 * great thanks to the legion of bouncycastle.com
 */
public class ZenMatrix implements BlockCipher  {

    private final static String SYMMCIPHERALGONAME = "ZenMatrix";
    public final static int ZEN_SIZE = 0x10;
    static int BLOCK_SIZE = 16;
    final static int[] BLOCK_SIZES = { 16, 64, 128, 256, 1024, 4096, 16384, 65536 };
    boolean initialised = false, symmetric = false, forEncryption;

    final static byte[] matrixPermutationBase = {
            0x0, 0x1, 0x2, 0x3, 
			0x4, 0x5, 0x6, 0x7,
            0x8, 0x9, 0xa, 0xb, 
			0xc, 0xd, 0xe, 0xf
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

    protected byte[] getInverseMatrix() {
        if (_inverseMatrix == null ||
                _inverseMatrix.length < 0x10 ||
                (_inverseMatrix[0] == (byte)0x0 && _inverseMatrix[1] == (byte)0x0 && _inverseMatrix[0xf] == (byte)0x0) ||
                (_inverseMatrix[0] == (byte)0x0 && _inverseMatrix[1] == (byte)0x1 && _inverseMatrix[0xf] == (byte)0xf))
        {
            _inverseMatrix = ZenMatrix.buildInverseMatrix(matrixPermutationKey, 0x10);
        }

        return _inverseMatrix;
    }

    /* #region IBlockCipher interface  */

    public String getAlgorithmName()  {
        return SYMMCIPHERALGONAME;
    }

    public int getBlockSize() {
        return BLOCK_SIZE;
    }


    @Override
    public void reset() {
		byte sbcnt = 0x0;
        matrixPermutationKey = new byte[0x10];
        for (byte s : matrixPermutationBase)  {
            privateBytes[sbcnt % 0x10] = (byte)0x0;
            matrixPermutationKey[sbcnt++] = s;
        }
        _inverseMatrix = buildInverseMatrix(matrixPermutationKey, 0x10);
		DbgWriter.msg(("ZenMatrix reseted"), false);
		initialised = false;
    }


    public void init(boolean forEncryption, CipherParameters parameters)  {
        if (!(parameters instanceof KeyParameter) && !(parameters instanceof ParametersWithIV))
            throw new IllegalArgumentException("parameters: only KeyParameter or ParametersWithIV expected.");

		reset();
		this.forEncryption = forEncryption;
		DbgWriter.msg(("ZenMatrix init(boolean forEncryption = " + String.valueOf(forEncryption) + ", ...) ..."), false);
				
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

            this.privateBytes = CryptHelper.getKeyHashBytes(bKey, bIv, 0x10);
			DbgWriter.msg(("\tprivateBytes.lenght = " + privateBytes.length +  " bKey.length = " + bKey.length +  " bIv.length = " + bIv.length), false);
        }
										        
		genBuildWithBytes(privateBytes, false);
        initialised = true;
    }

    /**
     * Processes one BLOCK with BLOCK_SIZE {@link ZenMatrix}.BLOCK_SIZE
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

			byte[] outBytes = processed;
            if (!forEncryption)
                outBytes = padBuffer(processed, false);			

            System.arraycopy(outBytes, 0, outBuf, outOff, BLOCK_SIZE);

            return BLOCK_SIZE;
        }

        return 0;
    }

        
    /* #endregion IBlockCipher interface */
    /* #region ctor_init_gen_reverse */

    public ZenMatrix() {
        this(16);
    }


    /**
     * public constructor
     * @param bs
     */
    public ZenMatrix(int bs)  {
        for (int i = 0; i < BLOCK_SIZES.length; i++)  {
            if (bs == BLOCK_SIZES[i])
                BLOCK_SIZE = BLOCK_SIZES[i];
        }
        reset();
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

		reset();
		symmetric = fullSymmetric;
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
     * Generates / builds a ZenMatrix with key bytes
     * @param keyBytes users keybytes created by users key and key hash iv
     *                  must have at least 4 bytes and will be truncated after
     *                  16 bytes only the first 16 bytes will be taken from keyBytes for {@link ZenMatrix}
     * @param fullSymmetric fullSymmetric means that zen matrix is it's inverse element and decrypts back to plain text, when encrypting twice or ²
     */
    protected void genBuildWithBytes(byte[] keyBytes, boolean fullSymmetric)  {
        if ((keyBytes == null || keyBytes.length < 4))
            throw new RuntimeException("byte[] keyBytes is null or keyBytes.Length < 4");

		symmetric = fullSymmetric;
		
		String kbs = "ZenMatrix genBuildWithBytes(byte keyBytes.length =" + keyBytes.length + ", fullSymmetric " +  fullSymmetric + ")\n\tKeyBytes = ";				
        for (int j = 0; j < keyBytes.length; kbs += String.format("%x", keyBytes[j++]));
		DbgWriter.msg(kbs, false);
        // InitMatrixSymChiffer();
        int ba = 0, bb = 0;
        System.arraycopy(keyBytes, 0, privateBytes, 0, Math.min(keyBytes.length, 0x10));
		
        byte[] permutationKeyHash = new byte[0];
		ArrayList<Byte> byteList = new ArrayList<Byte>();
		for (int bl = 0; bl < keyBytes.length; byteList.add(Byte.valueOf(keyBytes[bl++]))); 
		
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
		byte keyByte;
		
        for (Byte KeyByte : byteList) {
			keyByte = KeyByte.byteValue();
            byte b = (byte)(keyByte % 0x10);
			Byte B = Byte.valueOf(b);
            for (int i = 0; i < 0x20; i++)
            {
				B = Byte.valueOf(b);
                if (arrayContains(permutationKeyHash, b) || ((int)b) == ba)
                    b = (i >= 0x10) ? ((byte)(((int)(keyByte) + i) % 0x10)) :
                            ((byte)(((int)(keyByte) + magicOrder[i]) % 0x10));
                else break;
            }
			B = Byte.valueOf(b);

            if (!arrayContains(permutationKeyHash, b)) {
				b = B.byteValue();
                bb = (int)b;
                if (ba != bb) {
                    if (fullSymmetric) {
				    	Byte BA = Byte.valueOf((byte)ba);
				    	Byte BB = Byte.valueOf((byte)bb);
                        if (!matrixDict.keySet().contains(B) && !matrixDict.containsValue(BA)) {
                            matrixDict.put(BA, BB);
                            matrixDict.put(BB, BA);
                        }
                    }

                    permutationKeyHash = arrayAdd(permutationKeyHash, b);
                    // TODO:
					int bc = 0; 
					swap(matrixPermutationKey, (int)ba, (int)bb);				
					
                    ba++;
                }
            }
        }
		
		String perm = "KeyBytes: ";
		for (int j=0; j < keyBytes.length; perm += String.format("%x", keyBytes[j++]));               
        for (int j=0; j<16; j++) 
			perm += (j < permutationKeyHash.length && j < matrixPermutationKey.length) ?
				String.format("\n%02x \t=> %02x => %02x", j, permutationKeyHash[j], matrixPermutationKey[j]) :
				String.format("\n%02x \t=> permKeyHash.Count: %02d, matrixPermKey.Count: %02d", j, permutationKeyHash.length,  matrixPermutationKey.length);			
		DbgWriter.msg(("ZenMatrix " +  perm), false);	

        if (fullSymmetric) {
            /* #region fullSymmetric => InverseMatrix = MatrixPermutationKey; */
            if (matrixDict.size() < 0x0f)  {
                for (int k = 0; k < 0x10; k++) {
                    if (!matrixDict.keySet().contains(Byte.valueOf((byte)k))) {
                        for (int l = 0x0f; l >= 0; l--)  {
                            if (matrixDict.values().contains(Byte.valueOf((byte)l)))
                                continue;

                            matrixDict.put(Byte.valueOf((byte)k), Byte.valueOf((byte)l));
                            if (!matrixDict.keySet().contains(Byte.valueOf((byte)l)))
                                matrixDict.put(Byte.valueOf((byte)l), Byte.valueOf((byte)k));
                            break;
                        }
                    }
                }
            }
            if (matrixDict.size() == 0x10) {
                byte bKey, bValue;
                permutationKeyHash = new byte[16];
                for (int n = 0; n < 0x10; n++) {
					bKey = (byte)n;
                    Byte BKey = Byte.valueOf(bKey);
                    Byte BValue = matrixDict.get(BKey);
                    bValue = (byte)BValue.byteValue();
                    permutationKeyHash[n] = bValue;
                    matrixPermutationKey[(int)bKey] = bValue;
                    matrixPermutationKey[(int)bValue] = bKey;
                }
            }
            /* #endregion fullSymmetric => InverseMatrix = MatrixPermutationKey; */

            _inverseMatrix = matrixPermutationKey;
        }  
		else  {
            /* #region bugfix for missing permutations */
            byte[] strikeBytes = {  (byte)0x0, (byte)0x1, (byte)0x2, (byte)0x3, (byte)0x4, (byte)0x5, (byte)0x6, (byte)0x7,
                    (byte)0x8, (byte)0x9, (byte)0xa, (byte)0xb, (byte)0xc, (byte)0xd, (byte)0xe, (byte)0xf  };
            HashSet<Byte> strikeList = new HashSet<Byte>();
            for (int sl = 0; sl < strikeBytes.length; sl++) 
                strikeList.add(Byte.valueOf(strikeBytes[sl]));

            for (int i = 0; i < 0x10; i++)  {
				
                if ((permutationKeyHash.length <= i) && strikeList.size() > 0) {
                    Byte[] strikeArray = strikeList.toArray(Byte[]::new);
                    permutationKeyHash = arrayAdd(permutationKeyHash, strikeArray[0].byteValue());
                }

				byte inbyte = (byte)i;
                Byte InByte = Byte.valueOf(inbyte);
				if (permutationKeyHash[i] != inbyte) {	
					
					inbyte = (byte)permutationKeyHash[i];
                    matrixPermutationKey[i] = inbyte;
                }
                if (strikeList.contains(InByte))
                    strikeList.remove(InByte);
            }
			
            /* #endregion bugfix for missing permutations */
        }
		_inverseMatrix = buildInverseMatrix(matrixPermutationKey, 0x10);

        perm = "KeyBytes: ";
		for (int j=0; j < keyBytes.length; perm += String.format("%x", keyBytes[j++]));               
        for (int j=0; j<16; perm += String.format("\n%2x \t=> %02x => %2x", j, permutationKeyHash[j], matrixPermutationKey[j++])) ;
		DbgWriter.msg(("ZenMatrix " +  perm), false);	
        perm = "";
        for (int j=0; j<16; perm += String.format("\n%x \t=> %x ", j, _inverseMatrix[j++]));
		DbgWriter.msg(("ZenMatrix " +  perm), false);	

        initialised = true;        		
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
    protected byte[] processBytes(byte[] inBytes, int offSet, int len) {
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
                else if (i == ilen + 1)
                    outBytes[i] = (byte)0xff;                // write 0xff as stop byte at first byte of padding buffer
                else if (i == (olen - 1))
                    outBytes[i] = (byte)0x0;                // terminate outBytes with NULL
                else if (i > (ilen + 1))
                    outBytes[i] = padbuf[j++];              // fill rest with padding buffer

            }
        } else {                                            // truncate padding buffer to get trimmed decrypted output

            int olen = inBytes.length;
            boolean last0 = false;

            for (olen = ilen; (olen > 0 && !last0); olen--) {
                if ((inBytes[olen - 1] == (byte)0xff) && inBytes[olen - 2] == (byte)0x0) {
                    last0 = true;
                    break;
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
    public byte[] encrypt(byte[] pdata, boolean randomBuffer) {
        // Check arguments.
        if (pdata == null || pdata.length <= 0)
            throw new IllegalArgumentException("ZenMatrix byte[] Encrypt(byte[] pdata): ArgumentNullException pdata = null or Lenght 0.");

        forEncryption = true;
        byte[] obytes = padBuffer(pdata, randomBuffer);

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

	public byte[] encrypt(byte[] pdata) {
		return encrypt(pdata, false);
	}

    /**
     * decrypt
     * @param ecdata encrypted byte array
     * @return decrypted plain bytes
     */
    public byte[] decrypt(byte[] ecdata)  {   
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
     * MapByteValue splits a byte in 2 0x0 to 0xf segments and map both trough {@see MatrixPermutationKey} in case of encrypt,
     * @param inByte in byte to map
     * @param encrypt  mapped out byte
     * @return byte[]
     */
    protected  byte[] mapByteValue(byte inByte,  boolean encrypt) {
        byte[] outSBytes = new byte[4];
        // ArrayList<Byte> outSBytes = new ArrayList<Byte>(2);
        byte outByte;
        String s = String.format("%02x", inByte);
        byte maskLsb = (byte)0x0f;
        byte maskMsb = (byte)0xf0;
		byte lsbIn = (byte)((inByte & 0x0f) % 16);
		lsbIn = (byte)(mapChar(s.charAt(1)));
        // byte lsbIn = (byte)(inByte & maskLsb);

		byte msbIn = (byte)((inByte & 0xf0) / 0x10); 
        msbIn = (byte)(mapChar(s.charAt(0)));
        // byte msbIn = (byte)(inByte & maskMsb);
        byte lsbOut, msbOut;
        if (encrypt)
        {
            lsbOut = matrixPermutationKey[(byte)lsbIn];
            msbOut = matrixPermutationKey[(byte)msbIn];
            outByte = (byte)((msbOut * 0x10) + lsbOut);
            outSBytes[0] = outByte;
			outSBytes[1] = (byte)inByte;
            outSBytes[2] = msbOut;
            outSBytes[3] = lsbOut;            
        }
        else // if decrypt
        {
            lsbOut = _inverseMatrix[(byte)lsbIn];
            msbOut = _inverseMatrix[(byte)msbIn];
			outByte = (byte)((msbOut * 0x10) + lsbOut);
			outSBytes[0] = outByte;
            outSBytes[1] = (byte)inByte;
            outSBytes[2] = msbOut;
            outSBytes[3] = lsbOut;            
        }

        return outSBytes;
    }


	/***
	 * swap values in array by index positions i, j
	 * @param arr {@link byte[]} array
	 * @param i first position index to swap
	 * @param j second (last) position index to swap
	 */
    public static void swap(byte[] arr, int i, int j) {
        // error checking
        if (arr == null || i == j) {
            return;
        }
        if (i < 0 || j < 0 || i > arr.length - 1 || j > arr.length - 1) {
            return;
        }
        // looks good, swap the values
        byte t0 = arr[i];
		byte t1 = arr[j];
        arr[i] = t1;
        arr[j] = t0;
    }

	/***
	 * swapValue swap values of byte b, byte d in array
	 * @param arr {@link byte[]} array
	 * @param b first byte value to swap with byte d
	 * @param d second (last) byte value  to swap with b
	 */
    public static void swapValue(byte[] arr, byte b, byte d) {
        // error checking
        if (arr == null || b == d) 
            return;
        
        int i = -1, j = -1, foundII = 0;
        for (i = 0; (i < arr.length); i++) {
            if (arr[i] == b) {
                foundII++;
				break;
			}
        }

        for (j = 0; (j < arr.length); j++) {
            if (arr[j] == d) {
                foundII++;
				break;
			}
        }
        if (foundII == 2 && i < arr.length && j < arr.length) {
            // looks good, swap the values
            byte t = arr[i];
            arr[i] = arr[j];
            arr[j] = t;
        }
    }


    public boolean arrayContains(byte[] barr, byte  b) {
        for (int bc = 0; bc < barr.length; bc++) 
            if (barr[bc]==b)
                return true;
        return false;
    }

    public byte[] arrayAdd(byte[] barr, byte b) {
        byte[] nbarr = new byte[barr.length + 1];
        for (int bc = 0; bc < nbarr.length; bc++) 
            nbarr[bc] = (bc < barr.length) ? barr[bc] : b;
        barr = nbarr;
        return nbarr;
    }

    public byte mapChar(char c) {
        switch(c) {
            case '1': return (byte)0x1;
            case '2': return (byte)0x2;
            case '3': return (byte)0x3;
            case '4': return (byte)0x4;
            case '5': return (byte)0x5;
            case '6': return (byte)0x6;
            case '7': return (byte)0x7;
            case '8': return (byte)0x8;
            case '9': return (byte)0x9;
            case 'A': 
            case 'a': return (byte)0xa;
            case 'B':
            case 'b': return (byte)0xb;
            case 'C': 
            case 'c': return (byte)0xc;
            case 'D':
            case 'd': return (byte)0xd;
            case 'E': 
            case 'e': return (byte)0xe;
            case 'F':
            case 'f': return (byte)0xf;
            case '0':
            default: break;
       } 
       return (byte)0x0;
   }
   /* #endregion static helpers swap byte and SwapT{T} generic */

}

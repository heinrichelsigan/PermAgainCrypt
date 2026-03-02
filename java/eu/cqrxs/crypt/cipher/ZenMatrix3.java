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
 * ZenMatrix2  wrapper class
 * great thanks to the legion of bouncycastle.com
 */
public class ZenMatrix3 extends ZenMatrix implements BlockCipher  {

    private final static String SYMMCIPHERALGONAME = "ZenMatrix2";
    public final static int ZEN_SIZE = 0x100;
    static int BLOCK_SIZE = 256;

    final static short[] matrixPermutationBase2 = {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
				0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
				0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
				0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
				0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
				0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
				0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
				0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
                0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
				0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
				0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
				0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
				0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
				0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0xdf,
				0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef,
				0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff
	};
    
    protected byte[] privateBytes2 = new byte[0x100];

    public short[] matrixPermutationKey2;
    protected short[] _inverseMatrix2 = new short[0];

    protected short[] getInverseMatrix2() {
        if (_inverseMatrix2 == null ||
                _inverseMatrix2.length < 0x100 ||
                (_inverseMatrix2[0] == (short)0x0 && _inverseMatrix2[1] == (short)0x0 &&
                    _inverseMatrix2[0xe] == (short)0x0 && _inverseMatrix2[0xf] == (short)0x0) ||
                (_inverseMatrix2[0] == (short)0x0 && _inverseMatrix2[1] == (short)0x1 &&
                    _inverseMatrix2[0xe] == (short)0xe  && _inverseMatrix2[0xf] == (short)0xf))
        {
            _inverseMatrix2 = buildInverseMatrix2(matrixPermutationKey2, 0x100);
        }

        return _inverseMatrix2;
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
        super.reset();
		byte sbcnt = 0x0;
        matrixPermutationKey2 = new short[0x100];
        privateBytes2 = new byte[0x100];
        for (short s : matrixPermutationBase2)  {
            try {
                short sbshort = (short)sbcnt;
                privateBytes2[sbshort % 0x100] = (byte) 0x0;
                matrixPermutationKey2[sbshort % 0x100] = s;
            } catch (Exception exi) {
                DbgWriter.msgex(exi, false);
            }
            sbcnt++;
        }
        _inverseMatrix2 = buildInverseMatrix2(matrixPermutationKey2, 0x100);
		DbgWriter.msg(("ZenMatrix2 reseted"), false);
		initialised = false;
    }

    @Override
    public void init(boolean forEncryption, CipherParameters parameters)  {
        super.init(forEncryption, parameters);
		
		if (!(parameters instanceof KeyParameter) && !(parameters instanceof ParametersWithIV))
            throw new IllegalArgumentException("parameters: only KeyParameter or ParametersWithIV expected.");

		this.forEncryption = forEncryption;
		DbgWriter.msg(("ZenMatrix2 init(boolean forEncryption = " + String.valueOf(forEncryption) + ", ...) ..."), false);
				
        try  {
            privateBytes2 = ((KeyParameter)parameters).getKey();
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

			privateBytes2 = CryptHelper.getKeyHashBytes(bKey, bIv, 0x100);
			DbgWriter.msg(("\tprivateBytes2.lenght = " + privateBytes2.length +  " bKey.length = " + bKey.length +  " bIv.length = " + bIv.length), false);
        }
										        
		genBuildWithBytes2(privateBytes2, false);
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
    public int processBlock2(byte[] inBuf, int inOff, byte[] outBuf, int outOff)
            throws DataLengthException, IllegalStateException {
        if (privateBytes2 == null)
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
                mappedByte = mapByteValue2(b, forEncryption)[0];
                short sm = forEncryption ? matrixPermutationKey2[aCnt % 0x100] : _inverseMatrix2[aCnt % 0x100];
                int pos = bCnt + ((int)sm) % 0x100;
                processed[pos] = mappedByte;
                if (aCnt != 0 && aCnt % 0x100 == 0)
                    bCnt += 0x100;
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

    public ZenMatrix3() {
        this(256);
    }



    /**
     * public constructor
     * @param bs
     */
    public ZenMatrix3(int bs)  {
		BLOCK_SIZE = (bs < 256) ? 256 : bs;
        reset();
    }

        
    /* #endregion IBlockCipher interface */
    /* #region ctor_init_gen_reverse */


    /**
     * initializes a {@link ZenMatrix3} with secret user key string and hash iv
     * @param secretKey user's secret key
     * @param hashIV private key hash iv string
     * @param fullSymmetric fullSymmetric means that zen matrix is it's inverse element and decrypts back to plain text, when encrypting twice or ²
     */
    public ZenMatrix3(String secretKey, String hashIV, boolean fullSymmetric) {
        this(256);
        if (secretKey.isEmpty())
            throw new IllegalArgumentException("secretKey is null or empty");

		symmetric = fullSymmetric;
        byte[] keyBytes2 = CryptHelper.getUserKeyBytes(secretKey, hashIV, 0x100);

        genBuildWithBytes2(keyBytes2, fullSymmetric);
    }


    /**
     * initializes a {@link ZenMatrix3} with secret user key string and hash iv
     * @param secretKey user's secret key
     * @param hashIV private key hash iv string
     * @param fullSymmetric fullSymmetric means that zen matrix is it's inverse element and decrypts back to plain text, when encrypting twice or ²
     * @param keyHash {@link KeyHash} is used, when hash iv is null or empty to get new hash iv from user key
     */
    public ZenMatrix3(String secretKey, String hashIV, boolean fullSymmetric, KeyHash keyHash) {
        this(secretKey, String.valueOf(hashIV.isEmpty() ? keyHash.hash(secretKey) : hashIV), fullSymmetric);
    }


    /***
     * initializes a {@link ZenMatrix3} with an array of key bytes
     * @param keyBytes2 user key bytes to init algorithm instance
     * @param fullSymmetric if true inverse matrix is same as encrypt matrix V * M * M = V * M * MInverse = V
     */
    public ZenMatrix3(byte[] keyBytes2, boolean fullSymmetric) {
        this(16);
        genBuildWithBytes2(keyBytes2, fullSymmetric);
    }


    

    /**
     * Generates / builds a ZenMatrix with key bytes
     * @param keyBytes2 users keybytes created by users key and key hash iv
     *                  must have at least 4 bytes and will be truncated after
     *                  16 bytes only the first 16 bytes will be taken from keyBytes for {@link ZenMatrix}
     * @param fullSymmetric fullSymmetric means that zen matrix is it's inverse element and decrypts back to plain text, when encrypting twice or ²
     */
    protected void genBuildWithBytes2(byte[] keyBytes2, boolean fullSymmetric)  {
        if ((keyBytes2 == null || keyBytes2.length < 4))
            throw new RuntimeException("byte[] keyBytes2 is null or keyBytes2.Length < 4");

        genBuildWithBytes(keyBytes2, fullSymmetric);
		symmetric = fullSymmetric;

        if (keyBytes2.length < 0x100)
            privateBytes2 = CryptHelper.getKeyBytesSingle(keyBytes2, 0x100);
        else
        {
            for (int l = 0, k = keyBytes2.length - 1; (k >= 0 && l < 0x100); k--, l++)
                privateBytes2[l] = (byte)keyBytes2[k];
        }

		String kbs = "ZenMatrix2 genBuildWithBytes(byte keyBytes.length =" + keyBytes2.length + ", fullSymmetric " +  fullSymmetric + ")\n\tKeyBytes = ";
		DbgWriter.msg(kbs, false);
        // InitMatrixSymChiffer();
        int ba = 0, bb = 0;

		
        short[] permutationKeyHash2 = new short[0];
		ArrayList<Byte> byteList2 = new ArrayList<Byte>();
		for (int bl = 0; bl < keyBytes2.length; byteList2.add(Byte.valueOf(keyBytes2[bl++])));
		
        // MatrixDict is only needed, when (fullSymmetric == true)
        HashMap<Byte, Byte> matrixDict2 = new HashMap<Byte, Byte>();

        // Simplest method to fill deterministic up privateBytes2 from keyBytes with keyBytes.Length < 16
        // for (int i = keyBytes.Length; i < 0x100; i++)
        // {
        //       if (i < 0x08)
        //          privateBytes2[i] = (byte)keyBytes[i % keyBytes.Length];
        //      else
        //          privateBytes2[i] = (byte)keyBytes[0x08 - (i - 0x07)];
        // }
        //
		byte keyByte;
		
        for (Byte KeyByte : byteList2) {
			keyByte = KeyByte.byteValue();
            byte b = (byte)(keyByte % 0x100);
			Byte B = Byte.valueOf(b);
            for (int i = 0; i < 0x100; i++)
            {
				B = Byte.valueOf(b);
                if (arrayContainsS(permutationKeyHash2, b) || ((int)b) == ba) {
                    if (i < 0x100)
                        b =  ((byte)(((int)(keyByte) +
                                (magicOrder[i % 10] * magicOrder[i % 10])) % 0x100));
                    else if (i >= 0x100)
                        b = ((byte) (((int) (keyByte) + i) % 0x100));
                }
                else break;
            }
			B = Byte.valueOf(b);

            if (!arrayContainsS(permutationKeyHash2, b)) {
				b = B.byteValue();
                bb = (int)b;
                if (ba != bb) {
                    if (fullSymmetric) {
				    	Byte BA = Byte.valueOf((byte)ba);
				    	Byte BB = Byte.valueOf((byte)bb);
                        if (!matrixDict2.keySet().contains(B) && !matrixDict2.containsValue(BA)) {
                            matrixDict2.put(BA, BB);
                            matrixDict2.put(BB, BA);
                        }
                    }

                    permutationKeyHash2 = arrayAddS(permutationKeyHash2, b);
                    // TODO:
					int bc = 0; 
					swap(matrixPermutationKey2, (int)ba, (int)bb);
					
                    ba++;
                }
            }
        }
		
		String perm = "KeyBytes: ";
		for (int j=0; j < keyBytes2.length; perm += String.format("%x", keyBytes2[j++]));
        for (int j=0; j<256; j++)
			perm += (j < permutationKeyHash2.length && j < matrixPermutationKey2.length) ?
				String.format("\n%02x \t=> %02x => %02x", j, permutationKeyHash2[j], matrixPermutationKey2[j]) :
				String.format("\n%02x \t=> permKeyHash.Count: %02d, matrixPermKey.Count: %02d",
                        j, permutationKeyHash2.length,  matrixPermutationKey2.length);
		DbgWriter.msg(("ZenMatrix " +  perm), false);	

        if (fullSymmetric) {
            /* #region fullSymmetric => InverseMatrix = MatrixPermutationKey2; */
            if (matrixDict2.size() < 0xff)  {
                for (int k = 0; k < 0x100; k++) {
                    if (!matrixDict2.keySet().contains(Byte.valueOf((byte)k))) {
                        for (int l = 0xff; l >= 0; l--)  {
                            if (matrixDict2.values().contains(Byte.valueOf((byte)l)))
                                continue;

                            matrixDict2.put(Byte.valueOf((byte)k), Byte.valueOf((byte)l));
                            if (!matrixDict2.keySet().contains(Byte.valueOf((byte)l)))
                                matrixDict2.put(Byte.valueOf((byte)l), Byte.valueOf((byte)k));
                            break;
                        }
                    }
                }
            }
            if (matrixDict2.size() == 0x100) {
                byte bKey, bValue;
                permutationKeyHash2 = new short[0x100];
                for (int n = 0; n < 0x100; n++) {
					bKey = (byte)n;
                    Byte BKey = Byte.valueOf(bKey);
                    Byte BValue = matrixDict2.get(BKey);
                    bValue = (byte)BValue.byteValue();
                    permutationKeyHash2[n] = bValue;
                    matrixPermutationKey2[(int)bKey] = bValue;
                    matrixPermutationKey2[(int)bValue] = bKey;
                }
            }
            /* #endregion fullSymmetric => InverseMatrix = MatrixPermutationKey2; */

            _inverseMatrix2 = matrixPermutationKey2;
        }  
		else  {
            /* #region bugfix for missing permutations */
            short[] strikeBytes2 = {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
                0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
                0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
                0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
                0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
                0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
                0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
                0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
                0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
                0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
                0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
                0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0xdf,
                0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef,
                0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff
            };

            HashSet<Short> strikeList2 = new HashSet<Short>();
            for (int sl = 0; sl < strikeBytes2.length; sl++)
                strikeList2.add(Short.valueOf(strikeBytes2[sl]));

            for (int i = 0; i < (byte)0x100; i++)  {
				
                if ((permutationKeyHash2.length <= i) && strikeList2.size() > 0) {
                    Short[] strikeArray2 = strikeList2.toArray(Short[]::new);
                    permutationKeyHash2 = arrayAddS(permutationKeyHash2, strikeArray2[0].shortValue());
                }

				short inbyte = (short)i;
                Short InByte = Short.valueOf(inbyte);
				if (permutationKeyHash2[i] != inbyte) {
					
					inbyte = (short)permutationKeyHash2[i];
                    matrixPermutationKey2[i] = inbyte;
                }
                if (strikeList2.contains(InByte))
                    strikeList2.remove(InByte);
            }
			
            /* #endregion bugfix for missing permutations */
        }
		_inverseMatrix2 = buildInverseMatrix2(matrixPermutationKey2, 0x100);

        perm = "KeyBytes: ";
		for (int j=0; j < keyBytes2.length; perm += String.format("%x", keyBytes2[j++]));
        for (int j=0; j<16; perm += String.format("\n%2x \t=> %02x => %2x",
                j, permutationKeyHash2[j], matrixPermutationKey2[j++])) ;
		DbgWriter.msg(("ZenMatrix " +  perm), false);	
        perm = "";
        for (int j=0; j<16; perm += String.format("\n%x \t=> %x ", j, _inverseMatrix2[j++]));
		DbgWriter.msg(("ZenMatrix2 " +  perm), false);

        initialised = true;        		
    }


    /* #endregion ctor_init_gen_reverse */
    /* #region ProcessEncryptDecryptBytes */

    /***
     * processBytes2 processes bytes for encryption or decryption depending on {@link forEncryption}
     *         processes the next len=16 bytes to encrypt, starting at offSet
     *         or processes the next len=16 bytes to decrypt, starting at offSet
     * @param inBytes bytes array to encrypt
     * @param offSet starting offSet
     * @param len of byte block (default 16)
     * @return byte[len] (default: 16) segment of encrypted bytes
     */
    protected byte[] processBytes2(byte[] inBytes, int offSet, int len) {
        int aCnt = 0, bCnt = 0;
        if (offSet < inBytes.length && offSet + len <= inBytes.length) {
            byte[] processed = new byte[len];
            for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++) {
                byte b = inBytes[bCnt];
                byte mappedByte = mapByteValue2(b, forEncryption)[0];
                short pos = (forEncryption) ? matrixPermutationKey2[aCnt % 0x100] : _inverseMatrix2[aCnt % 0x100];
                processed[pos] = mappedByte;
            }

            return processed;
        }

        return new byte[0];
    }

    protected byte[] processBlocks2(byte[] inBytes)
    {
        int aCnt = 0, bCnt = 0;
        byte[] processed = new byte[(int)inBytes.length];
        System.arraycopy(inBytes, 0, processed, 0, inBytes.length);

        for (int bs = 0; bs < inBytes.length; bs += 0x100)
        {
            for (int cs = 0, ds = 0; cs < 0x100 && (bs + cs) < inBytes.length; cs += 0x10)
            {
                int sm = (int)(0x10 * (int)((forEncryption) ?
                        matrixPermutationKey[ds] : _inverseMatrix[ds]));
                System.arraycopy(inBytes, bs + cs, processed, bs + sm, 0x10);
                ds++;
            }
        }

        return (processed != null && processed.length > 0) ? processed : new byte[0];
    }



    /* #endregion ProcessEncryptDecryptBytes */
    /* #region encrypt decrypt */


    /**
     * MatrixSymChiffer Encrypt member function
     * @param pdata plain data as
     * @return encrypted data
     */
    @Override
    public byte[] encrypt(byte[] pdata, boolean randomBuffer) {
        // Check arguments.
        if (pdata == null || pdata.length <= 0)
            throw new IllegalArgumentException("ZenMatrix byte[] Encrypt(byte[] pdata): ArgumentNullException pdata = null or Lenght 0.");

        forEncryption = true;
        byte[] obytes = padBuffer(pdata, randomBuffer);

        List<Byte> encryptedBytes = new ArrayList<Byte>();
        for (int i = 0; i < obytes.length; i += 0x100)  {
            for (byte pb : processBytes2(obytes, i, 0x100))  {
                encryptedBytes.add(Byte.valueOf(pb));
            }
        }

        byte[] rBytes = new byte[encryptedBytes.size()];
        int ib = 0;
        for (Byte bb : encryptedBytes) {
            rBytes[ib++] = (byte)(bb.byteValue());
        }
        byte[] processed2 = processBlocks2(rBytes);

        return processed2; // encryptedBytes.toArray();
    }

    @Override
	public byte[] encrypt(byte[] pdata) {
		return encrypt(pdata, false);
	}

    /**
     * decrypt
     * @param ecdata encrypted byte array
     * @return decrypted plain bytes
     */
    @Override
    public byte[] decrypt(byte[] ecdata)  {   
        if (ecdata == null || ecdata.length <= 0)
            throw new IllegalArgumentException("ZenMatrix byte[] Encrypt(byte[] ecdata): ArgumentNullException ecdata = null or lenght 0.");

        forEncryption = false;
        int eclen = ecdata.length;

        byte[] preProcessed = processBlocks2(ecdata);
        List<Byte> decBytes = new ArrayList<Byte>();
        for (int pc = 0; pc < preProcessed.length; pc += 256)  {
            for (byte rb : processBytes(preProcessed, pc, 256))
            {
                decBytes.add(Byte.valueOf(rb));
            }
        }

        byte[] outBytes = new byte[decBytes.size()];
        int ib = 0;
        for (Byte bb : decBytes) {
            outBytes[ib++] = (byte)(bb.byteValue());
        }

        byte[] retBytes = padBuffer(outBytes, false);

        return retBytes;
    }


    /* #endregion encrypt decrypt */
    /* #region static helpers swap byte and SwapT{T} generic */



    /**
     * buildInverseMatrix2, builds the determinant decryption matrix for sbyte[16] encryption matrix
     * @param matrix2 byte[16] encryption matrix
     * @param size byte size
     * @return byte[16] decryption matrix (determinante)
     */
    public static short[] buildInverseMatrix2(short[] matrix2, int size) {
        size = (size < 0x100) ? 0x100 : size;
        if (matrix2 != null && matrix2.length == size)  {
            short[] inverseM = new short[size];
            for (int m = 0; m < size; m++)  {
                short sm;
                try {
                    sm = matrix2[m];
                    inverseM[(short)sm] = (short)m;
                } catch (Exception exin) {
                    DbgWriter.msgex(exin, false);
                }
            }
            return inverseM;
        }
        String msg = "byte[] matrix is null or matrix.Length != " + String.valueOf(size) + ".";
        throw new NotImplementedError("ZenMatrix2");
        // return buildInverseMatrix(matrix2, 0x100);
    }

    /**
     * MapByteValue2 splits a byte in 2 0x0 to 0xf segments and map both trough {@see MatrixPermutationKey2} in case of encrypt,
     * @param inByte in byte to map
     * @param encrypt  mapped out byte
     * @return byte[]
     */
    protected byte[] mapByteValue2(byte inByte,  boolean encrypt) {
        byte[] outSBytes = new byte[4];
        // ArrayList<Byte> outSBytes = new ArrayList<Byte>(2);
        byte outByte;
        String s = String.format("%02x", inByte);
        byte lsbOut, msbOut;
        short sbshort =  ((short) inByte >= 0) ? (short) inByte : (short)(256 + inByte);
        if (encrypt)
        {
            outByte = (byte)matrixPermutationKey2[sbshort];
        }
        else // if decrypt
        {
            outByte = (byte)_inverseMatrix2[sbshort];
        }
        outSBytes[0] = outByte;
        outSBytes[1] = (byte)inByte;
        outSBytes[2] = (byte)(outByte & 0x0F);
        outSBytes[3] = (byte)((outByte & 0xF0) / 0x10);

        return outSBytes;
    }


    /***
     * swap values in array by index positions i, j
     * @param arr {@link byte[]} array
     * @param i first position index to swap
     * @param j second (last) position index to swap
     */
    public static void swap(short[] arr, int i, int j) {
        // error checking
        if (arr == null || i == j) {
            return;
        }
        if (i < 0 || j < 0 || i > arr.length - 1 || j > arr.length - 1) {
            return;
        }
        // looks good, swap the values
        short t0 = arr[i];
        short t1 = arr[j];
        arr[i] = t1;
        arr[j] = t0;
    }


    public boolean arrayContainsS(short[] barr, short  b) {
        for (int bc = 0; bc < barr.length; bc++)
            if (barr[bc]==b)
                return true;
        return false;
    }

    public short[] arrayAddS(short[] barr, short b) {
        short[] nbarr = new short[barr.length + 1];
        for (int bc = 0; bc < nbarr.length; bc++)
            nbarr[bc] = (bc < barr.length) ? barr[bc] : b;
        barr = nbarr;
        return nbarr;
    }

    /* #endregion static helpers swap byte and SwapT{T} generic */

}

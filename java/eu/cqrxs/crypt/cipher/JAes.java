/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.crypt.cipher;

import eu.cqrxs.crypt.encoding.EncodeEnum;
import eu.cqrxs.crypt.encoding.Hex16Coder;
import eu.cqrxs.crypt.hash.KeyHash;
import eu.cqrxs.util.CException;
import eu.cqrxs.util.Constants;
import eu.cqrxs.util.DbgWriter;
import eu.cqrxs.util.NotImplementedError;
import eu.cqrxs.zip.ZipType;
import org.bouncycastle.crypto.BlockCipher;
import org.bouncycastle.crypto.CipherParameters;
import org.bouncycastle.crypto.DataLengthException;
import org.bouncycastle.crypto.params.KeyParameter;
import org.bouncycastle.crypto.params.ParametersWithIV;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;
import java.util.List;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.util.ArrayList;

/**
 * JAes
 */
public class JAes implements BlockCipher {

    protected String secretKey = "";
    protected byte[] privateBytes = new byte[32];
    protected Cipher c;
    protected SecretKeySpec k;
   /* #region IBlockCipher interface  */

    private boolean initialised = false, forEncryption = true;
    private static final String SYMMCIPHERALGONAME = "JAes";
    private static int BLOCK_SIZE = 128;

    public String getAlgorithmName()  {
        return SYMMCIPHERALGONAME;
    }

    public int getBlockSize() {
        return BLOCK_SIZE;
    }


    @Override
    public void reset() {
        privateBytes = new byte[32];
        DbgWriter.msg(("JAes reseted"), false);
        initialised = false;
        c = null;
        k = null;
    }


    public void init(boolean encrypt, CipherParameters parameters)  {
        if (!(parameters instanceof KeyParameter) && !(parameters instanceof ParametersWithIV))
            throw new IllegalArgumentException("parameters: only KeyParameter or ParametersWithIV expected.");

        reset(); 
        forEncryption = encrypt;
        DbgWriter.msg(("JAes init(boolean forEncryption = " + String.valueOf(forEncryption) + ", ...) ..."), false);

        try  {
            privateBytes = ((KeyParameter)parameters).getKey();
        }  catch (Exception ex) {
            DbgWriter.msgex(ex, true);
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

            privateBytes = CryptHelper.tarBytes(bKey, bIv);
            DbgWriter.msg(("\tprivateBytes.lenght = " + privateBytes.length +  " bKey.length = " + bKey.length +  " bIv.length = " + bIv.length), false);
        }

        try {
            c = Cipher.getInstance("AES");
        } catch (Exception noche) {
            throw new CException("Unknown Cipher", (Throwable)noche);
        }
        try {
            k = new SecretKeySpec(privateBytes, "AES");
            c.init(Cipher.ENCRYPT_MODE, k);
        } catch (Exception invKey) {
            throw new CException("Invalid Key", (Throwable)invKey);
        }


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

            byte[] processed = new byte[inOffBuf.length - outOff];
            if (!initialised) {

                privateBytes = secretKey.getBytes(Charset.forName("UTF-8"));
                try {
                    c = Cipher.getInstance("AES");
                } catch (Exception noche) {
                    throw new CException("Unknown Cipher", (Throwable)noche);
                }
                try {
                    k = new SecretKeySpec(privateBytes, "AES");
                } catch (Exception invKey) {
                    throw new CException("Invalid Key", (Throwable)invKey);
                }

                try {
                    if (forEncryption)
                        c.init(Cipher.ENCRYPT_MODE, k);
                    else
                        c.init(Cipher.DECRYPT_MODE, k);
                } catch (Exception cipherInit) {
                    throw new CException("Cipher init Exception", (Throwable)cipherInit);
                }
            }

            try {
                processed = c.doFinal(inOffBuf);
            } catch (Exception finalTransform) {
                throw new CException("Cipher doFinal Exception", (Throwable)finalTransform);
            }
            byte[] outBytes = processed;
            if (!forEncryption)
                outBytes = padBuffer(processed, false);

            System.arraycopy(outBytes, 0, outBuf, outOff, BLOCK_SIZE);

            return BLOCK_SIZE;
        }

        return 0;
    }

    /**
     * public constructor
     * @param bs
     */
    public JAes(int bs)  { 
        BLOCK_SIZE =  bs;
        reset();
    }

    public JAes(int bs, String key) {
        this(bs);
        secretKey = key;
    }


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
     * JAes Encrypt member function
     * @param pdata plain data as
     * @return encrypted data
     */
    public byte[] encrypt(byte[] pdata, boolean randomBuffer) {
        // Check arguments.
        if (pdata == null || pdata.length <= 0)
            throw new IllegalArgumentException("ZenMatrix byte[] Encrypt(byte[] pdata): ArgumentNullException pdata = null or Lenght 0.");

        forEncryption = true;
        if (!initialised) {
            privateBytes = secretKey.getBytes(Charset.forName("UTF-8"));
            try {
                c = Cipher.getInstance("AES");
            } catch (Exception noche) {
                throw new CException("Unknown Cipher", (Throwable)noche);
            }
            try {
                k = new SecretKeySpec(privateBytes, "AES");
                c.init(Cipher.ENCRYPT_MODE, k);
            } catch (Exception invKey) {
                throw new CException("Invalid Key", (Throwable)invKey);
            }
        }
        byte[] obytes = padBuffer(pdata, randomBuffer);
        byte[] retBytes;
        try {
            retBytes = c.doFinal(obytes);
        } catch (Exception finalExc) {
            throw new CException("Cipher Exception on doFinal", (Throwable)finalExc);
        }

        return retBytes; // encryptedBytes.toArray();
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

        if (!initialised) {
            privateBytes = secretKey.getBytes(Charset.forName("UTF-8"));
            try {
                c = Cipher.getInstance("AES");
            } catch (Exception noche) {
                throw new CException("Unknown Cipher", (Throwable)noche);
            }
            try {
                k = new SecretKeySpec(privateBytes, "AES");
                c.init(Cipher.DECRYPT_MODE, k);
            } catch (Exception invKey) {
                throw new CException("Invalid Key", (Throwable)invKey);
            }
        }
        byte[] decBytes;
        try {
            decBytes = c.doFinal(ecdata);
        } catch (Exception finalExc) {
            throw new CException("Cipher Exception on doFinal", (Throwable)finalExc);
        }

        byte[] retBytes = padBuffer(decBytes, false);

        return retBytes;
    }



}

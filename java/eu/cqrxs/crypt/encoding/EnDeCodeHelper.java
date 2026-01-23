/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 34
 *
 * Coded 2021-2028 by <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
.*/

package eu.cqrxs.crypt.encoding;

import java.nio.charset.StandardCharsets;
import java.lang.String;
import java.util.ArrayList;


public final class EnDeCodeHelper {

    
    /**
     * keyBytesToHexBytesSalt converts key bytes to hex bytes and salt it
     * @param keyBytes bytes of secret key
     * @paran length length that final salt bytes should have
     * @return salt bytes
     */
    public static byte[] keyBytesToHexBytesSalt(byte[] keyBytes, int length)  {
        if (keyBytes == null || keyBytes.length== 0)
            throw new IllegalArgumentException("keyBytes");

        String hexString = (new Hex16Coder()).encodeBytesToString(keyBytes);
        byte[] hexBytes = hexString.getBytes(StandardCharsets.UTF_8);

        while (hexBytes.length < length) {
            hexBytes = eu.cqrxs.crypt.cipher.CryptHelper.tarBytes(keyBytes,
                    hexString.getBytes(StandardCharsets.UTF_8));
        }

        int len = (length > 0 && hexBytes.length >= length) ? length : hexBytes.length;

        byte[] outBytes = new byte[len];
        for (int i = 0; i < len; outBytes[i++] = ((byte)0)) ;
        System.arraycopy(hexBytes, 0, outBytes, 0, len);

        return outBytes;
    }

    public static byte[] keyToHexBytesSalt(String key, int length)  {
        if (key == null || key.isEmpty())
            throw new IllegalArgumentException("key");

        return keyBytesToHexBytesSalt(key.getBytes(StandardCharsets.UTF_8), length);
    }

    /**
     * getBytesFromBytes pads zero bytes at the end, until blocksize is reached
     * @param inBytes
     * @param blockSize 
     * @param upStretchToCorrectBlockSize if false, no padding will be added
     * @return bytes padded with \0 bytes
     */
    public static byte[] getBytesFromBytes(byte[] inBytes, int blockSize, boolean upStretchToCorrectBlockSize) {
        if (!upStretchToCorrectBlockSize) 
            return inBytes; 

        int addByteLen = blockSize - (inBytes.length % blockSize); 
        ArrayList<Byte> outList = new ArrayList<Byte>(); 
        for (int i=0; i < inBytes.length; i++) {
            outList.add(Byte.valueOf(inBytes[i]));
        }
        while (outList.size() % blockSize != 0) { 
            outList.add(Byte.valueOf(((byte)0)));
        } 
        byte[] outBytes  = new byte[outList.size()];
        for (int i = 0; i < outList.size(); i++) {
                outBytes[i] = (byte) outList.get(i);
        }

        return outBytes; 
    }

            
    /**
     * GetBytesTrimNulls gets a byte[] from binary byte[] data and truncate all 0 byte at the end. 
     * @param inBytes decrypted byte[]  
     * @return truncated byte[] without a lot of \0 (null) characters
     */ 
    public static byte[] getBytesTrimNulls(byte[] inBytes) { 
        int ig = inBytes.length; 
        int endIdx = ig;
        while (inBytes[--ig] == '\0') {
            endIdx = ig + 1;
        }
        if (endIdx >= inBytes.length) 
            endIdx = inBytes.length;
        byte[] outBytes = new byte[endIdx];
        System.arraycopy(inBytes, 0, outBytes, 0, endIdx);

         return outBytes;
    } 
    
}

/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 34
 *
 * Coded 2021-2028 by <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
.*/

package eu.cqrxs.fw.crypt.encoding;

import java.nio.charset.StandardCharsets;
import java.lang.String;

public final class EnDeCodeHelper {


    public static byte[] keyBytesToHexBytesSalt(byte[] keyBytes, int length)  {
        if (keyBytes == null || keyBytes.length== 0)
            throw new IllegalArgumentException("keyBytes");

        String hexString = (new Hex16Coder()).encodeBytesToString(keyBytes);
        byte[] hexBytes = hexString.getBytes(StandardCharsets.UTF_8);

        while (hexBytes.length < length) {
            hexBytes = eu.cqrxs.fw.crypt.cipher.CryptHelper.tarBytes(keyBytes,
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

}

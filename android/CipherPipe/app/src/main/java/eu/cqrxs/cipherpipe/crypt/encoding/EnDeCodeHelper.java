/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
.*/

package eu.cqrxs.cipherpipe.crypt.encoding;

import java.nio.charset.StandardCharsets;
import java.lang.String;

public final class EnDeCodeHelper {


    public static byte[] keyBytesToHexBytesSalt(byte[] keyBytes, int length)  {
        if (keyBytes == null || keyBytes.length== 0)
            throw new IllegalArgumentException("keyBytes");

        String hexString = (new Hex16Coder()).encodeBytesToString(keyBytes);
        byte[] hexBytes = hexString.getBytes(StandardCharsets.UTF_8);

        while (hexBytes.length < length) {
            hexBytes = eu.cqrxs.cipherpipe.crypt.cipher.CryptHelper.tarBytes(keyBytes,
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

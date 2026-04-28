/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.crypt.hash.MD5
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

package eu.cqrxs.crypt.hash;

import java.lang.String;
import java.nio.charset.Charset;

import org.bouncycastle.crypto.Digest;

import java.util.HexFormat;

import eu.cqrxs.util.*;

public class MD5 {


    public static String hashString(String instr) {
        byte[] inBytes = instr.getBytes(Charset.forName("UTF-8"));
        if (inBytes == null || inBytes.length == 0)
            throw new IllegalArgumentException("public static String hash(String instr) inBytes from instr is null!");

        String hexString = "";
        Digest digest = new org.bouncycastle.crypto.digests.MD5Digest();
        byte[] resBuf = new byte[digest.getDigestSize()];
        // digest.update(inBytes);
        digest.update(inBytes, 0, inBytes.length);
        digest.doFinal(resBuf, 0);
        HexFormat hex = HexFormat.of();
        hexString = hex.formatHex(resBuf);

        return hexString;
    }

}

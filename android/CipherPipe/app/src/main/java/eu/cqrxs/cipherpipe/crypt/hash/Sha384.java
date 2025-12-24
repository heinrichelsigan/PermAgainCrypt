/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.cipherpipe.crypt.hash;

import org.bouncycastle.crypto.Digest;

import java.lang.String;
import java.nio.charset.Charset;
import java.util.HexFormat;

public class Sha384 {


    public static String hashString(String instr) {
        byte[] inBytes = instr.getBytes(Charset.forName("UTF-8"));
        if (inBytes == null || inBytes.length == 0)
            throw new IllegalArgumentException("public static string hash(String instr) inBytes from instr is null!");

        Digest digest = new org.bouncycastle.crypto.digests.SHA384Digest();
        byte[] resBuf = new byte[digest.getDigestSize()];
        // digest.update(inBytes);
        digest.update(inBytes, 0, inBytes.length);
        digest.doFinal(resBuf, 0);

        HexFormat hex = HexFormat.of();
        String hexString = hex.formatHex(resBuf);

        return hexString;
    }

}
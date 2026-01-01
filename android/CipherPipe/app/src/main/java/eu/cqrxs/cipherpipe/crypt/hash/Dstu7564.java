/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2027 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

package eu.cqrxs.cipherpipe.crypt.hash;

import org.bouncycastle.crypto.Digest;

import eu.cqrxs.cipherpipe.util.Constants;
import eu.cqrxs.cipherpipe.crypt.cipher.CryptHelper;
import java.io.Serializable;
import java.lang.String;
import java.nio.charset.Charset;
import java.security.Key;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.HexFormat;
import java.util.List;
import java.util.Set;

public class Dstu7564 {

    public final static String VALID_CHARS = "0123456789abcdef";

    public static String hashString(String instr) {
        byte[] inBytes = instr.getBytes(Charset.forName("UTF-8"));
        if (inBytes == null || inBytes.length == 0)
            throw new IllegalArgumentException("public static string hash(String instr) inBytes from instr is null!");

		if (Constants.DEBUG) {
            System.out.println("Dstu7564: instr=" +instr + " \tinBytes.length=" + inBytes.length + " \t");
        }

        String hexString = "";
        Digest digest = new org.bouncycastle.crypto.digests.DSTU7564Digest(256);
        byte[] resBuf = new byte[digest.getDigestSize()];
        digest.update(inBytes, 0, inBytes.length);
        digest.doFinal(resBuf, 0);
        		
		HexFormat hex = HexFormat.of();
        hexString = hex.formatHex(resBuf);

		if (Constants.DEBUG) 
			System.out.println("Dstu7564 bytes.length=" +resBuf.length + " \thexstring=" + hexString);

        return hexString;
    }


}

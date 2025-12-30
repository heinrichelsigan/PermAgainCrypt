/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.fw.crypt.hash;

import org.bouncycastle.crypto.Digest;

import eu.cqrxs.fw.util.Constants;
import eu.cqrxs.fw.crypt.hash.Hex;
import eu.cqrxs.fw.crypt.encoding.Hex16Coder;
import eu.cqrxs.fw.crypt.cipher.CryptHelper;
import java.nio.charset.StandardCharsets;
import java.io.Serializable;
import java.lang.String;
import java.nio.charset.Charset;
import java.security.Key;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.HexFormat;
import java.util.List;
import java.util.Set;

public class CShake {

    public final static String VALID_CHARS = "0123456789abcdef";

    public static String hashString(String instr) {
        byte[] inBytes = instr.getBytes(Charset.forName("UTF-8"));
        if (inBytes == null || inBytes.length == 0)
            throw new IllegalArgumentException("public static string hash(String instr) inBytes from instr is null!");

		if (Constants.DEBUG)
            System.out.println("CShake instr=" +instr + " \tinBytes.length=" + inBytes.length + " \t");

        String hexString = "";
        Digest digest = new org.bouncycastle.crypto.digests.CSHAKEDigest(256, inBytes, CryptHelper.GetKeyBytesFromBytes(inBytes, 32));
        byte[] resBuf = new byte[digest.getDigestSize()];
        // digest.update(inBytes);
        digest.update(inBytes, 0, inBytes.length);
        digest.doFinal(resBuf, 0);

        String hexs = (new Hex16Coder()).encodeBytesToString(resBuf);

		try {
			HexFormat hex = HexFormat.of();
			hexString = hex.formatHex(resBuf);
		} catch (Exception ex) {
			ex.printStackTrace();
		}

        if (Constants.DEBUG) 
			System.out.println("CShake bytes.length=" +resBuf.length + " \thexstring=" + hexString + " \thexs=" + hexs);

        return hexString;
    }


}

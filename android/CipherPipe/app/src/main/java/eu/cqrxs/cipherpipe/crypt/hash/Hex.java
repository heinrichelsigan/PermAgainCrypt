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

import java.io.Serializable;
import java.lang.String;
import java.nio.charset.Charset;
import java.security.Key;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.HexFormat;
import java.util.List;
import java.util.Set;
import eu.cqrxs.cipherpipe.util.*;
	
public class Hex {

	public final static String VALID_CHARS = "0123456789abcdef";

	public static String hashString(String instr) {
		
		byte[] inBytes = instr.getBytes(Charset.forName("UTF-8"));
		
		if (inBytes == null || inBytes.length == 0)
			throw new IllegalArgumentException("public static string hash(String instr) inBytes from instr is null!");

		String hexString = "";
        for (int wc = 0; wc < inBytes.length; wc++)
            hexString += String.format("%x2", inBytes[wc]);

        if (hexString != null && !hexString.isEmpty())
            return hexString;

        try {
            HexFormat hex = HexFormat.of();
            hexString = hex.formatHex(inBytes, 0, inBytes.length - 1);
        } catch (Exception exi) {
            DbgWriter.msg("Exception: " + exi.toString(), true);
            hexString = "";
        }

		// string strUtf8 = System.Text.Encoding.UTF8.GetString(inBytes);
		return hexString;
	}


}

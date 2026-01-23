/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.crypt.hash;

import java.lang.String;
import java.nio.charset.Charset;
import java.util.HexFormat;

import eu.cqrxs.util.*;
	
public class Oct {

	public final static String VALID_CHARS = "0123456789abcdef";

	public static String hashString(String instr) {
		
		byte[] inBytes = instr.getBytes(Charset.forName("UTF-8"));
		
		if (inBytes == null || inBytes.length == 0)
			throw new IllegalArgumentException("public static string hash(String instr) inBytes from instr is null!");

		String octalString = "", hexString = "";
		HexFormat hex = HexFormat.of();
		hexString = hex.formatHex(inBytes);
		for (int wc = 0; wc < inBytes.length; wc++)
		    octalString += decimaltooctal(inBytes[wc]);

		// string strUtf8 = System.Text.Encoding.UTF8.GetString(inBytes);
		return octalString;
	}

    /**
	 * To calculate the octal value of the given decimal number
     */ 
    static String decimaltooctal(int deciNum)
    {
        // Initially declaring and initializing the
        // octal number with zero
        int octalNum = 0, countval = 1;
        int dNo = deciNum;

        // Condition check
        while (deciNum != 0) {

            // Decimals remainder is calculated
            int remainder = deciNum % 8;

            // Storing the octalvalue
            octalNum += remainder * countval;

            // Storing exponential value
            countval = countval * 10;
            deciNum /= 8;
        }

        // Print and display the octal number
        return String.valueOf(octalNum);
    }

}

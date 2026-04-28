/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.crypt.encoding.Base16Coder
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 *
 * Thanx to the legion of <a href="https://bouncycastle.org/">bouncycastle.org/</a>
 */

package eu.cqrxs.crypt.encoding;

import java.lang.Character;
import java.nio.charset.Charset;
import java.util.HexFormat;

/**
 * Base16 En-/Decoder
 *
 * 2026-01-21 changed encoding output toUpperCase
 *
 */
public class Base16Coder implements IEncodable {

	public final static String VALID_CHARS = "0123456789abcdef";
	public String error = "";

	public Base16Coder() { 
	}

	/**
	 * encodeBytesToString - converts a binary byte array to hex string
	 * @param inBytes byte array
	 * @return hex string
	 * @exception IllegalArgumentException is thrown when inBytes is null or empty
	 */
	public String encodeBytesToString(byte[] inBytes) {
        if (inBytes == null || inBytes.length < 1)
            throw new IllegalArgumentException("public static string encodeBytesToString(byte[] inBytes == NULL)");

		String hexString = "";
		HexFormat hex = HexFormat.of();
		hexString = hex.formatHex(inBytes).toUpperCase();

		return hexString;
	}

	/**
	 * encode a String into a hex string
	 * @param inString: plain input string
	 * @return hexString
	 * @exception IllegalArgumentException is thrown when hexStr is null or empty
	 */
	public String encode(String inString) throws IllegalArgumentException {
        byte[] inBytes = inString.getBytes(Charset.forName("UTF-8"));
		if (inBytes == null || inBytes.length == 0)
			throw new IllegalArgumentException("public static string hash(String inString) inBytes from instr is null!");

		String hexString = "";
		HexFormat hex = HexFormat.of();
		hexString = hex.formatHex(inBytes).toUpperCase();

		return hexString;
	}

    /**
     * decodeBytes transforms a hex string to binary byte array
     * @param hexString: a hex string
     * @return binary byte array
     * @exception IllegalArgumentException is thrown when hexStr is null or empty
     */
	public byte[] decodeStringToBytes(String hexString) throws IllegalArgumentException {
        if (hexString == null || hexString.length() == 0)
            throw new IllegalArgumentException("public static byte[] decodeBytes(string hexString), hexString == NULL || hexString == \"\"");

        int len = hexString.length();
		byte[] data = new byte[len / 2];
		for (int i = 0; i < len; i += 2) {
			data[i / 2] = (byte) ((Character.digit(hexString.charAt(i), 16) << 4)
					+ Character.digit(hexString.charAt(i+1), 16));
		}

		return data;

		// byte[] decodedBytes  = hexString.getBytes(Charset.forName("UTF-8"));
		// return decodedBytes;
	}

    /**
     * decode transforms a hex string to a readable text String
     * @param hexString: a hex string
     * @return a readable text String
     * @exception IllegalArgumentException is thrown when hexStr is null or empty
     */
	public String decode(String hexString) {
		if (hexString == null ||  hexString.length() < 1)
			throw new IllegalArgumentException("public String decode(String hexString), hexString == NULL || hexString == \"\"");

		byte[] outBytes = decodeStringToBytes(hexString);
		String outString = outBytes.toString();
		return outString;
	}

	/**
	 * validate checks if a string is in valid base16 format
	 * @param inString the string to validate
	 * @return true, if String is a valid Base16 encoded String, otherwise false
	 */
	public boolean IsValid(String inString) {
		boolean valid = true;
		error = "";
		for (char ch : inString.toCharArray())
		{
			if (!VALID_CHARS.contains(String.valueOf(ch)))
			{
				error = error + String.valueOf(ch);
				valid = false;
			}
		}
		return valid;
	}

    /**
     * validate checks if a string is in valid base16 format
     * @param inString the string to validate
     * @return true, if String is a valid Base16 encoded String, otherwise false
     */
	public boolean validate(String inString) {
		for (char ch : inString.toCharArray()) {
            boolean isValid = false;
            for (char validCh : VALID_CHARS.toCharArray()) {
                if (validCh == ch) {
                    isValid = true; 
                    break;
                }
            }
            if (!isValid)
				return false;
		}
		return true;
	}

}

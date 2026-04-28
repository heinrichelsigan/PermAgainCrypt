/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.crypt.encoding.Hex16Coder
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 *
 * Thanx to the legion of <a href="https://bouncycastle.org/">bouncycastle.org/</a>
 */

package eu.cqrxs.crypt.encoding;

import java.nio.charset.Charset;
import java.util.HexFormat;


/**
 * Hex16Coder
 * 2026-01-21 changed encoding output toUpperCase
 *
 */
public class Hex16Coder implements IEncodable  {

	public final static String VALID_CHARS = "0123456789ABCDEF";
	public String error = "";

	public Hex16Coder() {
	}

	/**
	 * encode a byte[] into a hex string
	 * @param inBytes plain input byte array
	 * @return hexString
	 */
	public String encodeBytesToString(byte[] inBytes) {
		if (inBytes == null || inBytes.length < 1)
			throw new IllegalArgumentException("public String ToHex(byte[] inBytes == NULL)");

		String hexString = "";
        HexFormat hex = HexFormat.of();
        hexString = hex.formatHex(inBytes).toLowerCase();

		return hexString;
	}

	/**
	 * encode a String into a hex string
	 * @param inString: plain input string
	 * @return hexString
	 */
	public String encode(String inString) {
		byte[] inBytes = inString.getBytes(Charset.forName("UTF-8"));
		if (inBytes == null || inBytes.length == 0)
			throw new IllegalArgumentException("public static string hash(String instr) inBytes from instr is null!");

		String hexString = "";
		HexFormat hex = HexFormat.of();
		hexString = hex.formatHex(inBytes).toLowerCase();

		return hexString;
	}

	/**
	 * decodeStringToBytes transforms a hex string to binary byte array
	 * @param hexString: a hex string
	 * @return binary byte array
	 */
	public byte[] decodeStringToBytes(String hexString) throws IllegalArgumentException
	{
		if (hexString == null || hexString.length() == 0)
			throw new IllegalArgumentException("public static byte[] decodeBytes(string hexString), hexString == NULL || hexString == \"\"");

		int len = hexString.length();
		byte[] data = new byte[len / 2];
		for (int i = 0; i < len; i += 2) {
			data[i / 2] = (byte) ((Character.digit(hexString.charAt(i), 16) << 4)
					+ Character.digit(hexString.charAt(i+1), 16));
		}

		return data;

		// byte[] decodedBytes  = hexString.getBytes();
		// return decodedBytes;
	}

	public String decode(String hexString) {
		byte[] outBytes = decodeStringToBytes(hexString);
		String outString = outBytes.toString();
		return outString;
	}

	public boolean IsValid(String inString)
	{
		boolean valid = true;
		error = "";
		for (char ch : inString.toCharArray())
		{
			if (!VALID_CHARS.contains(String.valueOf(ch)))
			{
				error += String.valueOf(ch);
				valid = false;
			}                
		}
		return valid;
	}

}

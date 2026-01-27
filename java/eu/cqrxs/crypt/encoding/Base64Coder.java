/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.crypt.encoding;

import eu.cqrxs.crypt.encoding.IEncodable;
import java.util.Base64;
import java.nio.charset.StandardCharsets;
import java.nio.charset.Charset;


/**
 * Base64Coder is base64 / mime encoder + decoder
 *
 * Table 1: The Base 64 Alphabeta
 *
 *   Value Encoding  Value Encoding  Value Encoding  Value Encoding 
 *       0 A            17 R            34 i            51 z 
 *       1 B            18 S            35 j            52 0 
 *       2 C            19 T            36 k            53 1 
 *       3 D            20 U            37 l            54 2 
 *       4 E            21 V            38 m            55 3 
 *       5 F            22 W            39 n            56 4 
 *       6 G            23 X            40 o            57 5 
 *       7 H            24 Y            41 p            58 6 
 *       8 I            25 Z            42 q            59 7 
 *       9 J            26 a            43 r            60 8 
 *      10 K            27 b            44 s            61 9 
 *      11 L            28 c            45 t            62 + (plus)
 *      12 M            29 d            46 u            63 / (slash)
 *      13 N            30 e            47 v 
 *      14 O            31 f            48 w         (pad) = 
 *      15 P            32 g            49 x 
 *      16 Q            33 h            50 y
 *
 */
public class Base64Coder implements IEncodable  {

    public Base64Coder() {
    }

    /**
     * encodeBytesToString - converts a binary byte array into a base64 String
     * @param inBytes byte array
     * @return base64 String
     * @exception IllegalArgumentException is thrown when inBytes is null or empty
     */
    public String encodeBytesToString(byte[] inBytes) {
        if (inBytes == null || inBytes.length < 1)
            throw new IllegalArgumentException("public static string encodeBytesToString(byte[] inBytes == NULL)");

        String encodedString = Base64.getEncoder().encodeToString(inBytes);
        return encodedString;
    }

    /**
     * decodeStringToBytes transforms a base64 encoded string into an binary byte[] array
     * @param encodedString: a base64 encoded String
     * @return binary byte array
     * @exception IllegalArgumentException is thrown when encodedString is null or empty
     */
    public byte[] decodeStringToBytes(String encodedString) {
        if (encodedString == null || encodedString.length() == 0)
            throw new IllegalArgumentException("public static byte[] decodeStringToBytes(string encodedString), encodedString == NULL || encodedString == \"\"");

        byte[] decodedBytes = Base64.getDecoder().decode(encodedString);
        return decodedBytes;
    }


	/**
     * encodeBytes - encodess any binary byte array into a base64 byte array
     * @param inBytes byte array
     * @return byte[] base64 encoded
     * @exception IllegalArgumentException is thrown when inBytes is null or empty
     */
    public byte[] encodeBytes(byte[] inBytes) {
        if (inBytes == null || inBytes.length < 1)
            throw new IllegalArgumentException("public static string encodeBytes(byte[] inBytes == NULL)");

        String encodedString = Base64.getEncoder().encodeToString(inBytes);
		byte[] outBytes = encodedString.getBytes(Charset.forName("UTF-8"));
        return outBytes;
    }
	
	/**
     * decodeBytes transforms a base64 encoded string into an binary byte[] array
     * @param encodedBytes base64 encoded byte array {@link byte[]}
     * @return binary byte array
     * @exception IllegalArgumentException is thrown when encodedString is null or empty
     */
    public byte[] decodeBytes(byte[] encodedBytes) {
        if (encodedBytes == null || encodedBytes.length == 0)
            throw new IllegalArgumentException("public static byte[] decodeStringToBytes(byte[] encodedBytes), encodedBytes == NULL || encodedBytes.length == 0");

		String encodedString = new String(encodedBytes, StandardCharsets.UTF_8);
        byte[] decodedBytes = Base64.getDecoder().decode(encodedString);
        return decodedBytes;
    }


    /**
     * encode a String into a base64 String
     * @param inString: plain input string
     * @return base64 String
     * @exception IllegalArgumentException is thrown when hexStr is null or empty
     */
    public String encode(String inString) {
        if (inString == null || inString.length() == 0)
            throw new IllegalArgumentException("public static byte[] encode(String inString), inString == NULL || encodedString == \"\"");

        String encoded = Base64.getEncoder().encodeToString(inString.getBytes(Charset.forName("UTF-8")));
        return encoded;
    }

    /**
     * decode transforms a base64 encoded String to a readable text String
     * @param encodedString: a base64 encoded String
     * @return a readable plain text String
     * @exception IllegalArgumentException is thrown when base64 encoded String is null or empty
     */
    public String decode(String encodedString) {
        if (encodedString == null || encodedString.length() == 0)
            throw new IllegalArgumentException("public static byte[] decode(String encodedString), encodedString == NULL || encodedString == \"\"");

        byte[] decodedBytes = Base64.getDecoder().decode(encodedString);
        String decodedString = new String(decodedBytes);
        return decodedString;
    }

}

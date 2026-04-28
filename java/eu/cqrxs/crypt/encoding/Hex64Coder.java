/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.crypt.encoding.Hex64Coder
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 *
 * Thanx to the legion of <a href="https://bouncycastle.org/">bouncycastle.org/</a>
 */

package eu.cqrxs.crypt.encoding;

import java.util.Base64;
import java.nio.charset.StandardCharsets;
import java.nio.charset.Charset;

/**
 * Hex64Coder is hex64 mime encoder + decoder with different encoding table
 * Table 2: The "URL and Filename safe" Base 64 Alphabet
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
 *      14 O            31 f            48 w 
 *      15 P            32 g            49 x 
 *      16 Q            33 h            50 y         (pad) =
 *
 */
public class Hex64Coder implements IEncodable  {

    public Hex64Coder() {
    }

    /**
     * encodeBytesToString - converts a binary byte array into a Hex64 String
     * @param inBytes byte array
     * @return hex64 String
     * @exception IllegalArgumentException is thrown when inBytes is null or empty
     */
    public String encodeBytesToString(byte[] inBytes) {
        if (inBytes == null || inBytes.length < 1)
            throw new IllegalArgumentException("public static String encodeBytesToString(byte[] inBytes == NULL)");

        String encodedString = Base64.getMimeEncoder().encodeToString(inBytes);
        String transformedEncoded = encodedString.replace("+", "-").replace("/", "_");
        return transformedEncoded;
    }

    /**
     * decodeStringToBytes transforms a hex64 encoded String into an binary byte[] array
     * @param encodedString: a Hex64 encoded String
     * @return binary byte array
     * @exception IllegalArgumentException is thrown when encodedString is null or empty
     */
    public byte[] decodeStringToBytes(String encodedString) {
        if (encodedString == null || encodedString.length() == 0)
            throw new IllegalArgumentException("public static byte[] decodeStringToBytes(String encodedString), encodedString == NULL || encodedString == \"\"");
        String transformedEncoded = encodedString.replace("-", "+").replace("_", "/");
        byte[] decodedBytes = Base64.getMimeDecoder().decode(transformedEncoded);
        return decodedBytes;
    }


	/**
     * encodeBytes - encodess any binary byte array into a hex64 byte array
     * @param inBytes byte array
     * @return byte[] hex64 encoded
     * @exception IllegalArgumentException is thrown when inBytes is null or empty
     */
    public byte[] encodeBytes(byte[] inBytes) {
        if (inBytes == null || inBytes.length < 1)
            throw new IllegalArgumentException("public static string encodeBytes(byte[] inBytes == NULL)");

        String encodedString = Base64.getMimeEncoder().encodeToString(inBytes);
        String transformedEncoded = encodedString.replace("+", "-").replace("/", "_");

		byte[] outBytes = transformedEncoded.getBytes(Charset.forName("UTF-8"));
        return outBytes;
    }
	
	/**
     * decodeBytes transforms a hex64 encoded string into an binary byte[] array
     * @param encodedBytes hex64 encoded byte array {@see byte[]}
     * @return binary byte array
     * @exception IllegalArgumentException is thrown when encodedString is null or empty
     */
    public byte[] decodeBytes(byte[] encodedBytes) {
        if (encodedBytes == null || encodedBytes.length == 0)
            throw new IllegalArgumentException("public static byte[] decodeStringToBytes(byte[] encodedBytes), encodedBytes == NULL || encodedBytes.length == 0");

		String encodedString = new String(encodedBytes, StandardCharsets.UTF_8);
        String transformedEncoded = encodedString.replace("-", "+").replace("_", "/");
        byte[] decodedBytes = Base64.getMimeDecoder().decode(transformedEncoded);
        return decodedBytes;
    }


    /**
     * encode a String into a hex64 String
     * @param inString: plain input string
     * @return hex64 String
     * @exception IllegalArgumentException is thrown when hexStr is null or empty
     */
    public String encode(String inString) {
        if (inString == null || inString.length() == 0)
            throw new IllegalArgumentException("public static byte[] encode(String inString), inString == NULL || encodedString == \"\"");
    
        byte[] inBytes = inString.getBytes(Charset.forName("UTF-8"));
        String encoded = Base64.getMimeEncoder().encodeToString(inBytes);
        String transformedEncoded = encoded.replace("+", "-").replace("/", "_");
        
        return transformedEncoded;
    }

    /**
     * decode transforms a hex64 encoded String to a readable text String
     * @param hex64Encoded a hex64 encoded String
     * @return a readable plain text String
     * @exception IllegalArgumentException is thrown when hex64 encoded String is null or empty
     */
    public String decode(String hex64Encoded) {
        if (hex64Encoded == null || hex64Encoded.length() == 0)
            throw new IllegalArgumentException("public static byte[] decode(String hex64Encoded), hex64Encoded == NULL || hex64Encoded == \"\"");

        String transformedEncoded = hex64Encoded.replace("-", "+").replace("_", "/");

        byte[] decodedBytes = Base64.getMimeDecoder().decode(transformedEncoded);
        String decodedString = new String(decodedBytes, StandardCharsets.UTF_8);
        return decodedString;
    }

}

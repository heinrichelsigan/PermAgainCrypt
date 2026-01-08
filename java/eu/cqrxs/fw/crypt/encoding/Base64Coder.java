/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.fw.crypt.encoding;

import java.util.Base64; 
import java.util.Arrays;
import java.util.Map;
import java.nio.charset.StandardCharsets;
import java.nio.charset.Charset;
import java.util.stream.Collectors;
import java.util.stream.IntStream;
import eu.cqrxs.fw.util.*;
/**
 * Base64Coder is base64 / mime encoder + decoder
 *
 */
public class Base64Coder extends EnDeCoder  {

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
     * @param encodedString: a base64 encoded String
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

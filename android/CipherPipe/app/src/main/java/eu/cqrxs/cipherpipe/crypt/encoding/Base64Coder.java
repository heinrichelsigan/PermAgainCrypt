/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2027 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.cipherpipe.crypt.encoding;

import java.util.Base64; 
import java.util.Arrays;
import java.util.Map;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

public class Base64Coder extends EnDeCoder  {

    public Base64Coder() {
    }

    public String encodeBytesToString(byte[] inBytes) {
        String encodedString = Base64.getEncoder().encodeToString(inBytes);
        return encodedString;
    }

    public byte[] decodeStringToBytes(String encodedString) {
        byte[] decodedBytes = Base64.getDecoder().decode(encodedString);
        return decodedBytes;
    }

    public String encode(String inString) {
        String encoded = Base64.getEncoder().encodeToString(inString.getBytes());
        return encoded;
    }

    public String decode(String encodedString) {
        byte[] decodedBytes = Base64.getDecoder().decode(encodedString);
        String decodedString = new String(decodedBytes);
        return decodedString;
    }

}

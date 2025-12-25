/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 34
 *
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.fw.crypt.encoding;

import eu.cqrxs.fw.crypt.encoding.uu.CharacterEncoder;
import eu.cqrxs.fw.crypt.encoding.uu.CharacterDecoder;
import eu.cqrxs.fw.crypt.encoding.uu.CEFormatException;
import eu.cqrxs.fw.crypt.encoding.uu.CEStreamExhausted;
import eu.cqrxs.fw.crypt.encoding.uu.UUEncoder;
import eu.cqrxs.fw.crypt.encoding.uu.UUDecoder;
import java.util.*;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintStream;
import java.io.IOException;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.nio.ByteBuffer;
import java.io.PushbackInputStream;
import eu.cqrxs.fw.util.*;

/**
 * UuCoder provides UUEncoder and UUDecode
 *
 */
public class UuCoder extends EnDeCoder {

    public UuCoder() {
    }


    /**
     * encodeBytesToString - converts a binary byte array into a uu encoded String
     * @param inBytes byte array
     * @return an uu encoded String
     * @exception IllegalArgumentException is thrown when inBytes is null or empty
     * @exception IOException generic exception is thrown in uue.encode
     */
    public String encodeBytesToString(byte[] inBytes) throws IOException {
        if (inBytes == null || inBytes.length < 1)
            throw new IllegalArgumentException("public static string encodeBytesToString(byte[] inBytes == NULL)");

        eu.cqrxs.fw.crypt.encoding.uu.UUEncoder uue =
                new eu.cqrxs.fw.crypt.encoding.uu.UUEncoder();
        String uuEncString = uue.encode(inBytes);
        return uuEncString;
    }

    /**
     * decodeBytes transforms an uu encoded string into an binary byte[] array
     * @param encodedString: an uu encoded String
     * @return binary byte array
     * @exception IllegalArgumentException is thrown when uu encoded String is null or empty
     * @exception IOException is thrown when eu.cqrxs.fw.crypt.encoding.uu.UUDecoder.decodeBuffer failes.
     */
    public byte[] decodeStringToBytes(String encodedString) throws IOException {
        if (encodedString == null || encodedString.length() == 0)
            throw new IllegalArgumentException("public static byte[] decodeStringToBytes(String encodedString), encodedString == NULL || encodedString == \"\"");

        eu.cqrxs.fw.crypt.encoding.uu.UUDecoder uud =
                new eu.cqrxs.fw.crypt.encoding.uu.UUDecoder();
        /*
            uud.mode = 664;
            uud.bufferName = "aString";
        */
        byte[] plainBytes = new byte[0];
        try {
            plainBytes = uud.decodeBuffer(encodedString);
        } catch (IOException ioUuDecEx) {
            throw ioUuDecEx;
        }
        return plainBytes;
    }


    /**
     * encode a String into an uu encoded String
     * @param inString: plain input string
     * @return an uu encoded String
     * @exception IllegalArgumentException is thrown when inString is null or empty
     */
    public String encode(String inString) {
        if (inString == null || inString.length() == 0)
            throw new IllegalArgumentException("public static byte[] encode(String inString), inString == NULL || encodedString == \"\"");

        byte[] inBytes = inString.getBytes(StandardCharsets.UTF_8);
        eu.cqrxs.fw.crypt.encoding.uu.UUEncoder uue =
                new eu.cqrxs.fw.crypt.encoding.uu.UUEncoder();
        String uuEncString = uue.encode(inBytes);
        return uuEncString;
    }

    /**
     * decode transforms an uu encoded String to a readable text String
     * @param encodedString: an uu encoded String
     * @return a readable plain text String
     * @exception IllegalArgumentException is thrown when uu encoded String is null or empty
     * @exception IOException is thrown, when UUDecoder().decodeBuffer(uuEncString) fails
     */
    public String decode(String encodedString) throws IOException {
        if (encodedString == null || encodedString.length() == 0)
            throw new IllegalArgumentException("public static byte[] decode(String encodedString), encodedString == NULL || encodedString == \"\"");

        eu.cqrxs.fw.crypt.encoding.uu.UUDecoder uud =
                new eu.cqrxs.fw.crypt.encoding.uu.UUDecoder();
        /* 
            uud.mode = 664; 
            uud.bufferName = "aString"; 
        */
        byte[] uuDecBytes = new byte[0];
        try {
            uuDecBytes = uud.decodeBuffer(encodedString);
        } catch (IOException ioUuDecEx) {
            throw ioUuDecEx;
        }
        String uuDecString = new String(uuDecBytes);
        return uuDecString;
    }

}

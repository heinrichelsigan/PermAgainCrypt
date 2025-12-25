/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 33
 *
 * Coded 2021-2033 by <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.fw.crypt.encoding;

import java.io.IOException;
import eu.cqrxs.fw.util.*;

/*
 * abstract class EnDeCoder is abstract none instancible base class for all encoders
 */
public abstract class EnDeCoder {

    /**
     * encode a byte[] into a hex string
     * @param inBytes plain input byte array
     * @return hexString
     * @exception IllegalArgumentException is thrown when  encoded String is null or empty
     * @exception IOException is thrown when encoder encoding failed
     */
    public abstract String encodeBytesToString(byte[] inBytes) throws IOException;

    /**
     * decode an encoded String into  a byte[]
     * @param encodedString am ASCII ancoded String
     * @return plain  byte[] array
     * @exception IllegalArgumentException is thrown when  encoded String is null or empty
     * @exception IOException is thrown when dcoder decoding failes.
     */
    public abstract byte[] decodeStringToBytes(String encodedString) throws IOException;

    /**
     * encodes a plain String into  am encoded String
     * @param inString plain String
     * @return an ASCII encoded String
     */
    public abstract String encode(String inString);

    /**
     * decodes an encoded String into a plain text String
     * @param encodedString ASCII encoded String
     * @return plain text String
     * @exception IllegalArgumentException is thrown when uu encoded String is null or empty
     * @exception IOException is thrown when decoder decoding failed
     */
    public abstract String decode(String encodedString) throws IOException;

}

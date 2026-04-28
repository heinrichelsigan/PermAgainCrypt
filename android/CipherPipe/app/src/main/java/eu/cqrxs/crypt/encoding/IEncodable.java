/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.crypt.encoding.IEncodable
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 *
 * Thanx to the legion of <a href="https://bouncycastle.org/">bouncycastle.org/</a>
 */

package eu.cqrxs.crypt.encoding;

import java.io.IOException;

public interface IEncodable {

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

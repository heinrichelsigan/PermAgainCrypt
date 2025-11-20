/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2027 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.cipherpipe.crypt.encoding;

public abstract class EnDeCoder {

    public abstract String encodeBytesToString(byte[] inBytes);
    
    public abstract byte[] decodeStringToBytes(String inString);

    public abstract String encode(String inString);
    public abstract String decode(String inString);

}

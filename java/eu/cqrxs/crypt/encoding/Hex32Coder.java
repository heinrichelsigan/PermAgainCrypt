/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.crypt.encoding.Hex32Coder
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 *
 * Thanx to the legion of <a href="https://bouncycastle.org/">bouncycastle.org/</a>
 */

package eu.cqrxs.crypt.encoding;

import eu.cqrxs.util.CException;
import java.nio.charset.Charset;

/**
 * Hex32Coder encoding is a mapping for double hex from 0-9A-V (32 chiffers per digit), padding char is =
 * <a href="href="https://datatracker.ietf.org/doc/html/rfc4648#section-7">Hex32</a>
 */
public class Hex32Coder implements IEncodable {

    public final static String VALID_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUV=";
    public String error = "";
    final static int _mask = 31;
    final static int _shift = 5;

    public Hex32Coder() {
    }

    /**
     * encode
     * @param inString
     * @return {@link String}
     */
    public String encode(String inString)  {
        byte[] inBytes = inString.getBytes(Charset.forName("UTF-8"));
        return encodeBytesToString(inBytes);
    }

    public String encodeBytesToString(byte[] inBytes) {
        return ToHex32(inBytes, 0, inBytes.length, true);
    }

    public String ToHex32(byte[] data, int offset, int length, boolean padOutput)  {
        if (data == null)
            throw new IllegalArgumentException("data is null");

        if (offset < 0)
            throw new IllegalArgumentException("offset: " + offset);

        if (length < 0)
            throw new IllegalArgumentException("length: " + length);

        if ((offset + length) > data.length)
            throw new IllegalArgumentException();

        if (length == 0)
            return "";

        // SHIFT is the number of bits per output character, so the length of the
        // output is the length of the input multiplied by 8/SHIFT, rounded up.
        // The computation below will fail, so don't do it.
        if (length >= (1 << 28))
            throw new IndexOutOfBoundsException("data len=" + data.length);

        int outputLength = (length * 8 + _shift - 1) / _shift;
        StringBuilder result = new StringBuilder(outputLength);

        int last = offset + length;
        int buffer = data[offset++];
        int bitsLeft = 8;
        while (bitsLeft > 0 || offset < last)  {
            if (bitsLeft < _shift)  {
                if (offset < last)  {
                    buffer <<= 8;
                    buffer |= (data[offset++] & 0xff);
                    bitsLeft += 8;
                } else  {
                    int pad = _shift - bitsLeft;
                    buffer <<= pad;
                    bitsLeft += pad;
                }
            }
            int index = _mask & (buffer >> (bitsLeft - _shift));
            bitsLeft -= _shift;

            result.append(VALID_CHARS.charAt(index));
        }

        if (padOutput)  {
            int padding = 8 - (result.length() % 8);
            if (padding > 0 && padding < 8) {
                for (int pj = 0; pj < 8; pj++)
                    result.append('=');
            }
        }

        return result.toString();
    }

    /**
     * decode
     * @param hex32String ASCII encoded String
     * @return {@link String}
     */
    public String decode(String hex32String) {
        byte[] outBytes = decodeStringToBytes(hex32String);
        String outString = outBytes.toString();
        return outString;
    }


    /***
     * decodeStringToBytes
     * @param encoded
     * @return {@see byte[]}
     */
    public byte[] decodeStringToBytes(String encoded) {

        if (encoded == null || encoded.isEmpty())
            throw new IllegalArgumentException(encoded);

        // Remove whitespace and padding. Note: the padding is used as hint
        // to determine how many bits to decode from the last incomplete chunk
        // Also, canonicalize to all upper case
        encoded = encoded.trim().toUpperCase();
        while (encoded.endsWith("=")) {
            int idx = encoded.lastIndexOf("=");
            encoded = encoded.substring(0, idx);
        }
        if (encoded.length() == 0)
            return new byte[0];

        int outLength = encoded.length() * _shift / 8;
        byte[] result = new byte[outLength];
        int buffer = 0;
        int next = 0;
        int bitsLeft = 0;
        int charValue = 0;
        for (char c : encoded.toCharArray())  {
            charValue = (int)charToInt(c);
            if (charValue < 0)
                throw new CException("Illegal character: `" + c + "`");

            buffer <<= _shift;
            buffer |= charValue & _mask;
            bitsLeft += _shift;
            if (bitsLeft >= 8) {
                result[next++] = (byte)(buffer >> (bitsLeft - 8));
                bitsLeft -= 8;
            }
        }

        return result;
    }



    public boolean isValidHex32(String inString) {
        boolean valid = true;
        error = "";
        for (char ch : inString.toCharArray()) {
            if (!VALID_CHARS.contains(String.valueOf(ch))) {
                error += String.valueOf(ch);
                valid = false;
            }
        }
        return valid;
    }
	
	public static int charToInt(char c) {
		int iLetterUpperA = (int)'A',
			iLetterLowera = ((int)'a'),
			iNumDigitZero = ((int)'0'),
			iChar = ((int)c);

		if (Character.isDigit(c))
			return (iChar - iNumDigitZero);
		else if (Character.isLetter(c)) {
			if (Character.isUpperCase(c))
				return ((iChar - iLetterUpperA) + 10);
			if (Character.isLowerCase(c))
				return ((iChar - iLetterLowera) + 10);
		}

		return -1;
	}


}
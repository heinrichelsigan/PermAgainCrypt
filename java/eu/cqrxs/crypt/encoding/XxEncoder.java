/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 34
 *
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.crypt.encoding;

import java.util.Arrays;
import java.util.Map;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.util.stream.Collectors;
import java.util.stream.IntStream;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;

/**
 * Xx encode decode
 *
 * 2026-01-21 last functionality that works
 *
 */
public class XxEncoder extends java.beans.Encoder {

	final static byte[] XXEncMap = new byte[] {
		0x2B, 0x2D, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35,
		0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44,
		0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C,
		0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54,
		0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x61, 0x62,
		0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A,
		0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72,
		0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A
	};
	final static byte[] XXDecMap = new byte[] {
		0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
		0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
		0x0A, 0x0B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12,
		0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A,
		0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20, 0x21, 0x22,
		0x23, 0x24, 0x25, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C,
		0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34,
		0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C,
		0x3D, 0x3E, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00
	};
    
	
    String getEmptyDataSymbol() {
        return "+";
    }

	/**
	 * Encode - encodes a string to xx string
	 * @param inString string to encode
	 * @return xx string
	 */
    public static String Encode(String inString) {
		if (inString == null || inString.length() < 1)
            throw new IllegalArgumentException("public static string Encode(String inString == NULL)");
		
		byte[] inBytes = inString.getBytes(Charset.forName("UTF-8"));
		String xxEncode = EncodeBytesToString(inBytes);
		
		return xxEncode;
    }

	/**
	 * Decode - decodes a xx string to decoded plain String
	 * @param encoded xx String
	 * @return decoded plain String
	 */
	public static String Decode(String encodedString) {
	
		byte[] decodedBytes = DecodeStringToBytes(encodedString);
		String decoded = new String(decodedBytes, StandardCharsets.UTF_8);
		return decoded;
		
	}
	
	/**
	 * EncodeBytesToString - converts a binary byte array to xx string
	 * @param inBytes byte array
	 * @return xx string
	 * @exception IllegalArgumentException is thrown when inBytes is null or empty
	 */
	public static String EncodeBytesToString(byte[] inBytes) {
        if (inBytes == null || inBytes.length < 1)
            throw new IllegalArgumentException("public static string EncodeBytesToString(byte[] inBytes == NULL)");
		
		int len = inBytes.length;
		ByteArrayInputStream inStream = new ByteArrayInputStream(inBytes, 0, len);
		ByteArrayOutputStream outStream = new ByteArrayOutputStream();
		
		int sidx = 0;
		int line_len = 45;
		String newline = "\r\n";
		byte[] outBytes, nl = newline.getBytes(Charset.forName("UTF-8"));
		byte u0, u1, u2;
		
		// split into lines, adding line-length and line terminator
		while (sidx + line_len < len) {
			// line length
			outStream.write(XXEncMap[line_len]);

			// 3-byte to 4-byte conversion + 0-63 to ascii printable conversion
			for (int end = sidx + line_len; sidx < end; sidx += 3) {
				u0 = (byte)inStream.read();
				u1 = (byte)inStream.read();
				u2 = (byte)inStream.read();

				outStream.write(XXEncMap[(u0 >> 2) & 63]);
				outStream.write(XXEncMap[(u1 >> 4) & 15 | (u0 << 4) & 63]);
				outStream.write(XXEncMap[(u2 >> 6) & 3 | (u1 << 2) & 63]);
				outStream.write(XXEncMap[u2 & 63]);
			}

			// line terminator
			for (int idx = 0; idx < nl.length; idx++)
				outStream.write(nl[idx]);
		}

		// line length
		outStream.write(XXEncMap[len - sidx]);

		// 3-byte to 4-byte conversion + 0-63 to ascii printable conversion
		while (sidx + 2 < len) {
			u0 = (byte)inStream.read();
			u1 = (byte)inStream.read();
			u2 = (byte)inStream.read();

			outStream.write(XXEncMap[(u0 >> 2) & 63]);
			outStream.write(XXEncMap[(u1 >> 4) & 15 | (u0 << 4) & 63]);
			outStream.write(XXEncMap[(u2 >> 6) & 3 | (u1 << 2) & 63]);
			outStream.write(XXEncMap[u2 & 63]);
			sidx += 3;
		}

		if (sidx < len - 1) {
			u0 = (byte)inStream.read();
			u1 = (byte)inStream.read();

			outStream.write(XXEncMap[(u0 >> 2) & 63]);
			outStream.write(XXEncMap[(u1 >> 4) & 15 | (u0 << 4) & 63]);
			outStream.write(XXEncMap[(u1 << 2) & 63]);
			outStream.write(XXEncMap[0]);
		}
		else if (sidx < len) {
			u0 = (byte)inStream.read();

			outStream.write(XXEncMap[(u0 >> 2) & 63]);
			outStream.write(XXEncMap[(u0 << 4) & 63]);
			outStream.write(XXEncMap[0]);
			outStream.write(XXEncMap[0]);
		}

		// line terminator
		for (int idx = 0; idx < nl.length; idx++)
			outStream.write(nl[idx]);
		
		outBytes = outStream.toByteArray();
		String xxString = new String(outBytes, StandardCharsets.UTF_8);
		
		return xxString;
	}
	
	 /**
     * DecodeStringToBytes transforms a xx encoded string to binary byte array
     * @param xxString: a xx string
     * @return binary byte array
     * @exception IllegalArgumentException is thrown when hexStr is null or empty
     */
	public static byte[] DecodeStringToBytes(String xxString) throws IllegalArgumentException {
        if (xxString == null || xxString.length() == 0)
            throw new IllegalArgumentException("public static byte[] DecodeStringToBytes(string xxString), xxString == NULL || xxString == \"\"");

		byte[] inBytes = xxString.getBytes(Charset.forName("UTF-8"));
		byte[] outBytes = DecodeBytesToBytes(inBytes);
		return outBytes;
	}
		
	public static byte[] DecodeBytesToBytes(byte[] inBytes) throws IllegalArgumentException {
        if (inBytes == null || inBytes.length == 0)
            throw new IllegalArgumentException("public static byte[] DecodeBytesToBytes(byte[] inBytes), byte[] inBytes == NULL");

		byte[] outBytes = new byte[0];
		int len = inBytes.length;
		ByteArrayInputStream inStream = new ByteArrayInputStream(inBytes, 0, len);
		ByteArrayOutputStream outStream = new ByteArrayOutputStream();
		
		if (len == 0)
			return outBytes;

		long didx = 0;
		
		int nextByte = inStream.read();
		while (nextByte >= 0) {
			// get line length (in number of encoded octets)
			int line_len = XXDecMap[nextByte];
			// ascii printable to 0-63 and 4-byte to 3-byte conversion
			long end = didx + line_len;
			byte u0, u1, u2, u3;
			if (end > 2) {
				while (didx < end - 2) {
					u0 = XXDecMap[inStream.read()];
					u1 = XXDecMap[inStream.read()];
					u2 = XXDecMap[inStream.read()];
					u3 = XXDecMap[inStream.read()];

					outStream.write((byte)(((u0 << 2) & 255) | ((u1 >> 4) & 3)));
					outStream.write((byte)(((u1 << 4) & 255) | ((u2 >> 2) & 15)));
					outStream.write((byte)(((u2 << 6) & 255) | (u3 & 63)));
					didx += 3;
				}
			} 

			if (didx < end) {
				u0 = XXDecMap[inStream.read()];
				u1 = XXDecMap[inStream.read()];
				outStream.write((byte)(((u0 << 2) & 255) | ((u1 >> 4) & 3)));
				didx++;

				if (didx < end) {
					u2 = XXDecMap[inStream.read()];
					outStream.write((byte)(((u1 << 4) & 255) | ((u2 >> 2) & 15)));
					didx++;
				}
			}

			// skip padding
			do {
				nextByte = inStream.read();
			}
			while (nextByte >= 0 && nextByte != '\n' && nextByte != '\r');

			// skip end of line
			do {
				nextByte = inStream.read();
			}
			while (nextByte >= 0 && (nextByte == '\n' || nextByte == '\r'));

		}
		outBytes = outStream.toByteArray();
			
		return outBytes;
		
	}
	
}

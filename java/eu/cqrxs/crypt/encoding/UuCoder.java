/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 34
 *
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.crypt.encoding;

import java.io.IOException;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import eu.cqrxs.util.*;

/**
 * UuCoder provides UUEncoder and UUDecode
 *
 */
public class UuCoder extends java.beans.Encoder implements IEncodable  {

        final static byte[] UUEncMap = new byte[] {
          0x60, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27,
          0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F,
          0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
          0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
          0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
          0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F,
          0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57,
          0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F
        };

        final static byte[] UUDecMap = new byte[] {
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
          0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
          0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
          0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F,
          0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27,
          0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F,
          0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
          0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };



    public UuCoder() {
    }

    String getEmptyDataSymbol() {
        return "+";
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

        String uuEncString = "";
        eu.cqrxs.crypt.encoding.uu.UUEncoder uue =
                new eu.cqrxs.crypt.encoding.uu.UUEncoder();
        try { 
            uuEncString = uue.encode(inBytes);
        } catch (Exception uuEncodeEx)  {
            uuEncodeEx.printStackTrace();
            byte[] outBytes = encodeBytesToBytes(inBytes);
            uuEncString = new String(outBytes, StandardCharsets.UTF_8);
        }
        return uuEncString;
    }

    /**
     * decodeBytes transforms an uu encoded string into an binary byte[] array
     * @param encodedString: an uu encoded String
     * @return binary byte array
     * @exception IllegalArgumentException is thrown when uu encoded String is null or empty
     * @exception IOException is thrown when eu.cqrxs.crypt.encoding.uu.UUDecoder.decodeBuffer failes.
     */
    public byte[] decodeStringToBytes(String encodedString) throws IOException {
        if (encodedString == null || encodedString.length() == 0)
            throw new IllegalArgumentException("public static byte[] decodeStringToBytes(String encodedString), encodedString == NULL || encodedString == \"\"");

        eu.cqrxs.crypt.encoding.uu.UUDecoder uud =
                new eu.cqrxs.crypt.encoding.uu.UUDecoder();
        /*
            uud.mode = 664;
            uud.bufferName = "aString";
        */
        byte[] plainBytes = new byte[0];
        try {
            plainBytes = uud.decodeBuffer(encodedString);
        } catch (IOException ioUuDecEx) {
            ioUuDecEx.printStackTrace();
            byte[] inBytes = encodedString.getBytes(Charset.forName("UTF-8"));
            plainBytes = decodeBytesToBytes(inBytes);
        }
        return plainBytes;
    }


    /**
     * encode a String into an uu encoded String
     * @param inString: plain input string
     * @return an uu encoded String
     * @exception IllegalArgumentException is thrown when inString is null or empty
     * @exception IOException
     */
    public String encode(String inString) {
        if (inString == null || inString.length() == 0)
            throw new IllegalArgumentException("public static byte[] encode(String inString), inString == NULL || encodedString == \"\"");

        byte[] inBytes = inString.getBytes(StandardCharsets.UTF_8);
        String uuEncoded = "";
        try {
            eu.cqrxs.crypt.encoding.uu.UUEncoder uue =
                new eu.cqrxs.crypt.encoding.uu.UUEncoder();
            uuEncoded = uue.encode(inBytes);
        } catch (Exception exUu) {
            exUu.printStackTrace();
            try {
                uuEncoded = encodeBytesToString(inBytes);
            } catch (IOException ioex) {
                ioex.printStackTrace();
                uuEncoded = "";
            }
        }
        return uuEncoded;
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

        eu.cqrxs.crypt.encoding.uu.UUDecoder uud =
                new eu.cqrxs.crypt.encoding.uu.UUDecoder();
        /* 
            uud.mode = 664; 
            uud.bufferName = "aString"; 
        */
        byte[] uuDecBytes = new byte[0];
        try {
            uuDecBytes = uud.decodeBuffer(encodedString);
        } catch (IOException ioUuDecEx) {
            ioUuDecEx.printStackTrace();
            uuDecBytes = decodeStringToBytes(encodedString);
        }
        String uuDecString = new String(uuDecBytes, StandardCharsets.UTF_8);

        return uuDecString;
    }

     public byte[] encodeBytesToBytes(byte[] inBytes) {
        if (inBytes == null || inBytes.length < 1)
            throw new IllegalArgumentException("public static string encodeBytesToBytes(byte[] inBytes == NULL)");

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
            outStream.write(UUEncMap[line_len]);

            // 3-byte to 4-byte conversion + 0-63 to ascii printable conversion
            for (int end = sidx + line_len; sidx < end; sidx += 3) {
                u0 = (byte)inStream.read();
                u1 = (byte)inStream.read();
                u2 = (byte)inStream.read();

                outStream.write(UUEncMap[(u0 >> 2) & 63]);
                outStream.write(UUEncMap[(u1 >> 4) & 15 | (u0 << 4) & 63]);
                outStream.write(UUEncMap[(u2 >> 6) & 3 | (u1 << 2) & 63]);
                outStream.write(UUEncMap[u2 & 63]);
            }

            // line terminator
            for (int idx = 0; idx < nl.length; idx++)
                outStream.write(nl[idx]);
        }

        // line length
        outStream.write(UUEncMap[len - sidx]); 
                // 3-byte to 4-byte conversion + 0-63 to ascii printable conversion
        while (sidx + 2 < len) {
            u0 = (byte)inStream.read();
            u1 = (byte)inStream.read();
            u2 = (byte)inStream.read();

            outStream.write(UUEncMap[(u0 >> 2) & 63]);
            outStream.write(UUEncMap[(u1 >> 4) & 15 | (u0 << 4) & 63]);
            outStream.write(UUEncMap[(u2 >> 6) & 3 | (u1 << 2) & 63]);
            outStream.write(UUEncMap[u2 & 63]);
            sidx += 3;
        }

        if (sidx < len - 1) {
            u0 = (byte)inStream.read();
            u1 = (byte)inStream.read();

            outStream.write(UUEncMap[(u0 >> 2) & 63]);
            outStream.write(UUEncMap[(u1 >> 4) & 15 | (u0 << 4) & 63]);
            outStream.write(UUEncMap[(u1 << 2) & 63]);
            outStream.write(UUEncMap[0]);
        }
        else if (sidx < len) {
            u0 = (byte)inStream.read();

            outStream.write(UUEncMap[(u0 >> 2) & 63]);
            outStream.write(UUEncMap[(u0 << 4) & 63]);
            outStream.write(UUEncMap[0]);
            outStream.write(UUEncMap[0]);
        }

        // line terminator
        for (int idx = 0; idx < nl.length; idx++)
            outStream.write(nl[idx]);

        outBytes = outStream.toByteArray();
        return outBytes;
    }

    public byte[] decodeBytesToBytes(byte[] inBytes) throws IllegalArgumentException {
        if (inBytes == null || inBytes.length == 0)
            throw new IllegalArgumentException("public static byte[] decodeBytesToBytes(byte[] inBytes), byte[] inBytes == NULL");

        byte[] outBytes = new byte[0];
        int len = inBytes.length;
        ByteArrayInputStream inStream = new ByteArrayInputStream(inBytes, 0, len);
        ByteArrayOutputStream outStream = new ByteArrayOutputStream();
        long didx = 0;
        if (len == 0)
            return outBytes;

        int nextByte = inStream.read();
        while (nextByte >= 0) {
            int line_len = UUDecMap[nextByte];  // get line length (in number of encoded octets)
            long end = didx + line_len;         // ascii printable to 0-63 and 4-byte to 3-byte conversion
            byte u0, u1, u2, u3;
            if (end > 2) {
                while (didx < end - 2) {
                    u0 = UUDecMap[inStream.read()];
                    u1 = UUDecMap[inStream.read()];
                    u2 = UUDecMap[inStream.read()];
                    u3 = UUDecMap[inStream.read()];
                    outStream.write((byte)(((u0 << 2) & 255) | ((u1 >> 4) & 3)));
                    outStream.write((byte)(((u1 << 4) & 255) | ((u2 >> 2) & 15)));
                    outStream.write((byte)(((u2 << 6) & 255) | (u3 & 63)));
                    didx += 3;
                }
            }
            if (didx < end) {
                u0 = UUDecMap[inStream.read()];
                u1 = UUDecMap[inStream.read()];
                outStream.write((byte)(((u0 << 2) & 255) | ((u1 >> 4) & 3)));
                didx++;
                if (didx < end) {
                    u2 = UUDecMap[inStream.read()];
                    outStream.write((byte)(((u1 << 4) & 255) | ((u2 >> 2) & 15)));
                    didx++;
                }
            }
            do {    // skip padding
                nextByte = inStream.read();
            }
            while (nextByte >= 0 && nextByte != '\n' && nextByte != '\r');
            do {    // skip end of line
                nextByte = inStream.read();
            }
            while (nextByte >= 0 && (nextByte == '\n' || nextByte == '\r'));
        }
        outBytes = outStream.toByteArray();

        return outBytes;
    }



}

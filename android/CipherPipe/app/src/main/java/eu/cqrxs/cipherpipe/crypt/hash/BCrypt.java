/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */
 
package eu.cqrxs.cipherpipe.crypt.hash;

import org.bouncycastle.crypto.Digest;
import java.lang.String;
import java.nio.charset.StandardCharsets;
import java.util.HexFormat;

import eu.cqrxs.cipherpipe.crypt.encoding.EnDeCodeHelper;

public class BCrypt {

    final static int PASSWD_BYTE_LEN = 64;
    final static  int SALT_BYTE_LEN = 16;
    final static int AVG_COST = 4;


    /**
     * hashString bcrypts a String
     * @param instr String to bcrypt
     * @return bcrypted hex String
     */
    public static String hashString(String instr) {
        byte[] inBytes = instr.getBytes(StandardCharsets.UTF_8);
        if (inBytes == null || inBytes.length == 0)
            throw new IllegalArgumentException("public static String hashString(String instr) inBytes from instr is null!");

        if (inBytes.length > PASSWD_BYTE_LEN)
            throw new IllegalArgumentException("instr.length(" + instr.length() + ") > PASSWD_BYTE_LEN " + PASSWD_BYTE_LEN);
        byte[] salt = EnDeCodeHelper.keyBytesToHexBytesSalt(inBytes, SALT_BYTE_LEN);

        byte[] bcrypted = org.bouncycastle.crypto.generators.BCrypt.generate(inBytes, salt, AVG_COST);

        HexFormat hex = HexFormat.of();
        String hexString = hex.formatHex(bcrypted);

        return hexString;
    }

}

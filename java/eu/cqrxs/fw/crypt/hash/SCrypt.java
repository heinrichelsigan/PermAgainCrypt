/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 34
 * * Coded 2021-2028 by <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 * SCrypt a classic unix passwd crypt method
 * Thanx to the legion of <a href="https://bouncycastle.org/">bouncycastle.org/</a>
.*/
 
package eu.cqrxs.fw.crypt.hash;

import java.lang.String;
import java.nio.charset.StandardCharsets;
import java.util.HexFormat;

import eu.cqrxs.fw.crypt.encoding.EnDeCodeHelper;

public class SCrypt {


    final static int PASSWD_BYTE_LEN = 64;
    final static int SALT_BYTE_LEN = 16;
    final static int AVG_COST = 4;

    /**
     * hashString scrypts a String
     * @param instr String to crypt
     * @return scrypted hex String
     */
    public static String hashString(String instr) {
        byte[] inBytes = instr.getBytes(StandardCharsets.UTF_8);
        if (inBytes == null || inBytes.length == 0)
            throw new IllegalArgumentException("public static String hashString(String instr) inBytes from instr is null!");

        if (inBytes.length > PASSWD_BYTE_LEN)
            throw new IllegalArgumentException("instr.length(" + instr.length() + ") > PASSWD_BYTE_LEN " + PASSWD_BYTE_LEN);
        byte[] salt = EnDeCodeHelper.keyBytesToHexBytesSalt(inBytes, SALT_BYTE_LEN);

        byte[] scrypted = org.bouncycastle.crypto.generators.SCrypt.generate(inBytes, salt, AVG_COST, SALT_BYTE_LEN, 1, 32);

        HexFormat hex = HexFormat.of();
        String hexString = hex.formatHex(scrypted);

        return hexString;
    }
    
}

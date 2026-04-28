/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.crypt.hash.OpenBSDCrypt
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 *
 * Thanx to the legion of <a href="https://bouncycastle.org/">bouncycastle.org/</a>
 */

package eu.cqrxs.crypt.hash;

import java.lang.String;
import java.nio.charset.StandardCharsets;
import eu.cqrxs.crypt.encoding.EnDeCodeHelper;

public class OpenBSDCrypt {

    final static int PASSWD_BYTE_LEN = 64;
    final static  int SALT_BYTE_LEN = 16;
    final static int AVG_COST = 4;


    /**
     * hashString openBSD crypts a passwd instr
     * @param instr passwd to openBSD crypt
     * @return openBSD crypted String
     */
    public static String hashString(String instr) {
        byte[] inBytes = instr.getBytes(StandardCharsets.UTF_8);
        if (inBytes == null || inBytes.length == 0)
            throw new IllegalArgumentException("public static String hashString(String instr) inBytes from instr is null!");

        char[] passChars = instr.toCharArray();
        if (passChars == null || passChars.length < 1)
            throw new IllegalArgumentException("public  String hashString(String instr) passChars from instr is null or length 0.");

        if (passChars.length > PASSWD_BYTE_LEN)
            throw new IllegalArgumentException("instr.length(" + instr.length() + ") > PASSWD_BYTE_LEN " + PASSWD_BYTE_LEN);

        byte[] salt = EnDeCodeHelper.keyToHexBytesSalt(instr, SALT_BYTE_LEN);
        String openBsdPass = org.bouncycastle.crypto.generators.OpenBSDBCrypt.generate(passChars, salt, AVG_COST);
        return openBsdPass;
    }


}

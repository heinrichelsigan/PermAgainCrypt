/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 * BCrypt a classic unix passwd crypt method
 * Thanx to the legion of <a href="https://bouncycastle.org/">bouncycastle.org/</a>
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.fw.crypt.hash;

import org.bouncycastle.crypto.Digest;

import java.io.Serializable;
import java.lang.String;
import java.nio.charset.Charset;
import java.security.Key;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.HexFormat;
import java.util.List;
import java.util.Set;

public class SCrypt {


    final static int PASSWD_BYTE_LEN = 64;
    final static int SALT_BYTE_LEN = 16;
    final static int AVG_COST = 4;

    /*
        /// <summary>
        /// <see cref="Org.BouncyCastle.Crypto.Generators.SCrypt"/>
        /// Thanx to the legion of <see href="https://bouncycastle.org/" />
        /// </summary>
        /// <param name="keyBytes">keyBytes to hash encrypt</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] SCryptHash(byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length == 0)
            {
                string argExcMsg = "SCryptHash(keyBytes) => keyBytes";
                argExcMsg += (keyBytes == null) ? " is null." : string.Concat(".Length = ", keyBytes.Length, ".");
                throw new ArgumentException(argExcMsg, "keyBytes"); 
            }

            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"SCryptHash(keyBytes) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "keyBytes");

            byte[] salt = EnDeCodeHelper.KeyBytesToHexBytesSalt(keyBytes, SALT_BYTE_LEN);

            byte[] scrypted = Org.BouncyCastle.Crypto.Generators.SCrypt.Generate(keyBytes, salt, AVG_COST, SALT_BYTE_LEN, 1, 32);

            return scrypted;
        }


        public static byte[] SCryptHash(string passwd)
        {
            if (string.IsNullOrEmpty(passwd))
                throw new ArgumentNullException("passwd string is null or string.Empty.", "passwd");

            byte[] keyBytes = EnDeCodeHelper.GetBytes(passwd);

            return SCryptHash(keyBytes);
        }

        public static string Hash(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;
            
            if (System.IO.File.Exists(filePath))
                return Hash(File.ReadAllBytes(filePath));
                
            return HashString(filePath);            
        }


        public static string HashString(string string2Hash) => SCryptHash(string2Hash).ToHexString(false);



        public static string Hash(byte[] bytes) => SCryptHash(bytes).ToHexString(false);


        public static byte[] HashBytes(byte[] bytes) => SCryptHash(bytes);
    


    }
    */
    
}

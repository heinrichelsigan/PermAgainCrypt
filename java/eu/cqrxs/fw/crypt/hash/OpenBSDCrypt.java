/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.fw.crypt.hash;

import java.io.Serializable;
import java.lang.String;
import java.nio.charset.Charset;
import java.security.Key;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.HexFormat;
import java.util.List;
import java.util.Set;

public class OpenBSDCrypt {

    final static int PASSWD_BYTE_LEN = 64;
    final static  int SALT_BYTE_LEN = 16;
    final static int AVG_COST = 4;

    /*
        /// <summary>
        /// <see cref="Org.BouncyCastle.Crypto.Generators.OpenBsdBCrypt" />
        /// Thanx to the legion of <see href="https://bouncycastle.org/" />
        /// </summary>
        /// <param name="keyBytes">keyBytes to hash encrypt</param>
        /// <returns><see cref="Array{Byte}">byte[]</see></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] OpenBSDCryptHash(byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length == 0)
            {
                string argExcMsg = "OpenBSDCryptHash(keyBytes) => keyBytes";
                argExcMsg += (keyBytes == null) ? " is null." : string.Concat(".Length = ", keyBytes.Length, ".");
                throw new ArgumentException(argExcMsg, "keyBytes");
            }

            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"OpenBSDCryptHash(keyBytes) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "keyBytes");

            char[] passChars = EnDeCodeHelper.GetString(keyBytes).ToCharArray();

            byte[] salt = EnDeCodeHelper.KeyBytesToHexBytesSalt(keyBytes, SALT_BYTE_LEN);
            string bcdCrypted = Org.BouncyCastle.Crypto.Generators.OpenBsdBCrypt.Generate(passChars, salt, AVG_COST);

            return EnDeCodeHelper.GetBytes(bcdCrypted);
        }

        /// <summary>
        /// <see cref="Org.BouncyCastle.Crypto.Generators.OpenBsdBCrypt" />
        /// Thanx to the legion of <see href="https://bouncycastle.org/"" />
        /// </summary>
        /// <param name="passwd">string password</param>
        /// <returns>string encrypted password</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] OpenBSDCryptHash(string passwd)
        {
            if (string.IsNullOrEmpty(passwd))
                throw new ArgumentNullException("passwd");

            char[] passChars = passwd.ToCharArray();
            byte[] keyBytes = EnDeCodeHelper.GetBytes(passwd);

            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"BSDCrypt(passwd) => GetBytes(passwd) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "passwd");

            byte[] salt = EnDeCodeHelper.KeyToHexBytesSalt(passwd, SALT_BYTE_LEN);

            string bcdCrypted = Org.BouncyCastle.Crypto.Generators.OpenBsdBCrypt.Generate(passChars, salt, AVG_COST);
            
            return EnDeCodeHelper.GetBytes(bcdCrypted);
        }

        public static string Hash(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;
            
            if (System.IO.File.Exists(filePath))
                return Hash(File.ReadAllBytes(filePath));
                
            return HashString(filePath);            
        }


        public static string HashString(string string2Hash)
        {
            if (string.IsNullOrEmpty(string2Hash))
                throw new ArgumentNullException("string2Hash");

            char[] passChars = string2Hash.ToCharArray();
            byte[] keyBytes = EnDeCodeHelper.GetBytes(string2Hash);

            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"OpenBSDCrypt.HashString(string2hash) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "string2Hash");

            byte[] salt = EnDeCodeHelper.KeyToHexBytesSalt(string2Hash, SALT_BYTE_LEN);

            string bcdCrypted = Org.BouncyCastle.Crypto.Generators.OpenBsdBCrypt.Generate(passChars, salt, AVG_COST);

            return bcdCrypted;
        }



        public static string Hash(byte[] bytes) => OpenBSDCryptHash(bytes).ToHexString(true);

        public static byte[] HashBytes(byte[] bytes) => OpenBSDCryptHash(bytes);
    }
     */

}

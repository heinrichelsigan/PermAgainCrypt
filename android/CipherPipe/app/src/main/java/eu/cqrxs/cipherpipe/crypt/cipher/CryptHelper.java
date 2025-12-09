package eu.cqrxs.cipherpipe.crypt.cipher;

import androidx.core.content.res.TypedArrayUtils;

import com.google.common.primitives.Bytes;
import java.util.Arrays;
import java.util.List;
import java.io.ByteArrayOutputStream;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.HexFormat;
import java.util.List;

import eu.cqrxs.cipherpipe.crypt.hash.KeyHash;
import eu.cqrxs.cipherpipe.util.Constants;

/**
 * CryptHelper
 */
public class CryptHelper {


    public static byte[] tarBytes(byte[] byteArray0, byte[] byteArray1) {
        int tarByteSize = byteArray0.length + byteArray1.length;
        ByteBuffer byteBuffer = ByteBuffer.allocate(tarByteSize);
        byteBuffer.put(byteArray0);
        byteBuffer.put(byteArray1);

        return byteBuffer.array();
    }

    /**
     * PrivateUserKey
     * @param secretKey
     * @return private user key
     */
    public static String PrivateUserKey(String secretKey)
    {
        if (secretKey == null || secretKey.length() == 0)
            return Constants.AUTHOR_EMAIL;
        return  secretKey;
    }

    /***
     *
     * @param secKey users private secret key
     * @param hashedKey users private secret key hash
     * @return doubled concatendated string of (secretKey + hash)
     */
    public static String PrivateKeyWithUserHash(String secKey, String hashedKey)
    {
        if (secKey == null || secKey.length() < 1)
            throw new IllegalArgumentException("secKey");

        if (hashedKey == null || hashedKey.length() == 0)
            hashedKey = KeyHash.Hex.hash(secKey);

        String concatenation = String.format("%s%s", secKey, hashedKey);
        return concatenation;
    }


    /**
     *
     * @param key users private key
     * @param keyHash key hash
     * @param merge do merge
     * @return doubled concatendated string of (secretKey + hash)
     * @Exception IllegalArgumentException
     */
    public static byte[] KeyUserHashBytes(String key, String keyHash, boolean merge)
    {
        if (key == null || key.length() < 1)
            throw new IllegalArgumentException("key");

        if (keyHash == null || keyHash.length() == 0)
            keyHash = KeyHash.Hex.hash(key);

        byte[] keyBytes = key.getBytes(Charset.forName("UTF-8"));
        byte[] hashBytes = keyHash.getBytes(Charset.forName("UTF-8"));

        return KeyHashBytes(keyBytes, hashBytes, merge);
    }

    /***
     * KeyHashBytes
     * @param keyBytes user keyBytes
     * @param hashBytes user hashBytes
     * @param merge
     * @return merged byte array
     */
    public static byte[] KeyHashBytes(byte[] keyBytes, byte[] hashBytes, boolean merge) {
        if (keyBytes == null || keyBytes.length == 0)
            throw new IllegalArgumentException("keyBytes");

        if (hashBytes == null || hashBytes.length == 0)
            throw new IllegalArgumentException("hashBytes");

        if (!merge)
            return tarBytes(keyBytes, hashBytes);

        List<Byte> outBytes = new ArrayList<Byte>();

        int kb = 0, hb = 0;
        for (int ob = 0; (ob < (keyBytes.length + hashBytes.length)); ob++)
        {
            if (kb < keyBytes.length)
                outBytes.add(keyBytes[kb++]);
            if (hb < hashBytes.length)
                outBytes.add(hashBytes[hb++]);
            if (hb < hashBytes.length)
                outBytes.add(hashBytes[hashBytes.length - hb]);
            hb++;
            if (kb < keyBytes.length)
                outBytes.add(keyBytes[keyBytes.length - kb]);
            kb++;

            ob = outBytes.size();
        }

        byte[] outOut = new byte[outBytes.size()];
        System.arraycopy(outBytes.toArray(), 0, outOut, 0, outBytes.size());// Byte[] bytes = outBytes.toArray();

        return outOut;
    }


    public static byte[] GetKeyBytesSimple(String key, String keyHash, int keyLen) {
        if (key == null || key.length() == 0)
            throw new IllegalArgumentException("key");

        byte[] keyBytes = KeyHash.Hex.hash(key).getBytes();
        byte[] outBytes = new byte[keyLen];
        if (keyBytes.length >= keyLen) {
            ByteBuffer bb = ByteBuffer.wrap(keyBytes);
            bb.get(outBytes, 0, keyLen);
            // System.arraycopy()
            return outBytes;
        }

        byte[] smallBytes = tarBytes(keyBytes, keyHash.getBytes(Charset.forName("UTF-8")));

        if (smallBytes.length >= keyLen) {
            ByteBuffer bybu = ByteBuffer.wrap(smallBytes);
            bybu.get(outBytes, 0, keyLen);
            // System.arraycopy()
            return outBytes;
        }
        byte[] bigBytes = tarBytes(smallBytes,
                tarBytes(keyHash.getBytes(Charset.forName("UTF-8")), keyBytes));
        if (bigBytes.length >= keyLen) {
            ByteBuffer bytebuf = ByteBuffer.wrap(bigBytes);
            bytebuf.get(outBytes, 0, keyLen);
            // System.arraycopy()
            return outBytes;
        }

        byte[] hugeBytes = tarBytes(bigBytes, bigBytes);

        // return GetUserKeyBytes(key, keyHash, keyLen);
        return hugeBytes;
    }


        /// <summary>
        /// GetUserKeyBytes gets symmetric chiffre private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="key">user key, default email address</param>
        /// <param name="keyHash">user hash</param>
        /// <param name="keyLen">length of user key bytes, maximum length <see cref="Constants.MAX_KEY_LEN"/></param>
        /// <returns>Array of byte with length KeyLen</returns>
        /// <exception cref="ArgumentNullException"></exception>

    /***
     *
     * @param key
     * @param keyHash
     * @param keyLen
     * @return

    public static byte[] GetUserKeyBytes(String key, String keyHash, int keyLen)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            byte[] keyBytes = EnDeCodeHelper.GetBytes(key);
            // keyHash = (string.IsNullOrEmpty(keyHash)) ? EnDeCodeHelper.KeyToHex(key) : keyHash;
            byte[] hashBytes = string.IsNullOrEmpty(keyHash) ? EnDeCodeHelper.GetBytes(Hex16.ToHex16(keyBytes)) : EnDeCodeHelper.GetBytes(keyHash);

            int keyByteCnt = -1;
            keyLen = (keyLen > Constants.MAX_KEY_LEN) ? Constants.MAX_KEY_LEN : keyLen;
            string keyByteHashString = key;
            byte[] tmpKey = new byte[keyLen];

            byte[] keyHashBytes = KeyHashBytes(keyBytes, hashBytes);
            keyByteCnt = keyHashBytes.Length;
            byte[] keyHashTarBytes = new byte[keyByteCnt * 2 + 1];

            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(KeyHashBytes(hashBytes, keyBytes));
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(
                        KeyHashBytes(hashBytes, keyBytes),
                        KeyHashBytes(keyBytes, hashBytes)
                );
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            while (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(keyHashBytes);
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            if (keyLen <= keyByteCnt)
            {
                // Array.Copy(keyHashBytes, 0, tmpKey, 0, keyLen);
                for (int bytIdx = 0; bytIdx < keyLen; bytIdx++)
                    tmpKey[bytIdx] = keyHashBytes[bytIdx];
            }

            return tmpKey;

        }


        public static byte[] GetKeyBytesFromBytes(byte[] keyBytes, int keyLen = 32)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw new ArgumentNullException("keyBytes");

            byte[] hashBytes = EnDeCodeHelper.GetBytes(Hex16.ToHex16(keyBytes));

            int keyByteCnt = -1;
            keyLen = (keyLen > Constants.MAX_KEY_LEN) ? Constants.MAX_KEY_LEN : keyLen;
            byte[] tmpKey = new byte[keyLen];

            byte[] keyHashBytes = KeyHashBytes(keyBytes, hashBytes);
            keyByteCnt = keyHashBytes.Length;
            byte[] keyHashTarBytes = new byte[keyByteCnt * 2 + 1];

            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(KeyHashBytes(hashBytes, keyBytes));
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(
                        KeyHashBytes(hashBytes, keyBytes),
                        KeyHashBytes(keyBytes, hashBytes)
                );
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            while (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(keyHashBytes);
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            if (keyLen <= keyByteCnt)
            {
                // Array.Copy(keyHashBytes, 0, tmpKey, 0, keyLen);
                for (int bytIdx = 0; bytIdx < keyLen; bytIdx++)
                    tmpKey[bytIdx] = keyHashBytes[bytIdx];
            }

            return tmpKey;

        }
     */

        // #endregion GetUserKeyBytes
}


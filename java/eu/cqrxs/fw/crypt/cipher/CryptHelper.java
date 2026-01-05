/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.fw.crypt.cipher;

// import androidx.core.content.res.TypedArrayUtils;

// import com.google.common.primitives.Bytes;
import java.util.Arrays;
import java.util.List;
import java.io.ByteArrayOutputStream;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.HexFormat;
import java.util.List;

import eu.cqrxs.fw.crypt.encoding.Hex16Coder;
import eu.cqrxs.fw.crypt.hash.KeyHash;
import eu.cqrxs.fw.util.Constants;
import eu.cqrxs.fw.util.*;


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

    public static byte[] tarBytes(byte[] byteArray0, byte[] byteArray1, byte[] byteArray2) {
        int tarByteSize = byteArray0.length + byteArray1.length + byteArray2.length;
        ByteBuffer byteBuffer = ByteBuffer.allocate(tarByteSize);
        byteBuffer.put(byteArray0);
        byteBuffer.put(byteArray1);
        byteBuffer.put(byteArray2);

        return byteBuffer.array();
    }

    public static byte[] tarBytes(byte[] byteArray0, byte[] byteArray1,
                                  byte[] byteArray2, byte[] byteArray3) {
        int tarByteSize = byteArray0.length + byteArray1.length + byteArray2.length + byteArray3.length;
        ByteBuffer byteBuffer = ByteBuffer.allocate(tarByteSize);
        byteBuffer.put(byteArray0);
        byteBuffer.put(byteArray1);
        byteBuffer.put(byteArray2);
        byteBuffer.put(byteArray3);

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
    public static String PrivateKeyWithUserHash(String secKey, String hashedKey) {
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
    public static byte[] KeyUserHashBytes(String key, String keyHash, boolean merge)  {
        if (key == null || key.length() < 1)
            throw new IllegalArgumentException("key");

        if (keyHash == null || keyHash.length() == 0)
            keyHash = KeyHash.Hex.hash(key);

        byte[] keyBytes = key.getBytes(Charset.forName("UTF-8"));
        byte[] hashBytes = keyHash.getBytes(Charset.forName("UTF-8"));

        return keyHashBytes(keyBytes, hashBytes, merge);
    }

    /***
     * KeyHashBytes
     * @param keyBytes user keyBytes
     * @param hashBytes user hashBytes
     * @param merge
     * @return merged byte array
     */
    public static byte[] keyHashBytes(byte[] keyBytes, byte[] hashBytes, boolean merge) {
        if (keyBytes == null || keyBytes.length == 0)
            throw new IllegalArgumentException("keyBytes");

        if (hashBytes == null || hashBytes.length == 0)
            throw new IllegalArgumentException("hashBytes");

        if (!merge)
            return tarBytes(keyBytes, hashBytes);

        List<Byte> outBytes = new ArrayList<Byte>();

        int kb = 0, hb = 0;
        for (int ob = 0; (ob < (keyBytes.length + hashBytes.length)); ob++)  {
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
        for (int arrcp = 0; arrcp < outBytes.size(); arrcp++) // manually array copy
            outOut[arrcp] = ((Byte)outBytes.get(arrcp)).byteValue();

        return outOut;
    }




    /**
     * getKeyBytesSimple gets simplö user key bytes from users key and key hash
     * @param key users secret key
     * @param keyHash hashed users key
     * @param keyLen length that keybytes should have afterwards
     * @return generated user keybyte array from key and hash
     */
    public static byte[] getKeyBytesSimple(String key, String keyHash, int keyLen) {
        if (key == null || key.length() == 0)
            throw new IllegalArgumentException("key");

        byte[] keyBytes = key.getBytes(Charset.forName("UTF-8"));
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
            return outBytes;
        }

        byte[] hugeBytes = bigBytes;
        while (hugeBytes.length < keyLen)
            hugeBytes = tarBytes(hugeBytes, bigBytes);

        // if (hugeBytes.length > keyLen)
        System.arraycopy(hugeBytes, 0, outBytes, 0, keyLen);

        return outBytes;
    }


       /***
        * getUserKeyBytes
        * @param key users secret key
        * @param keyHash hashed key
        * @param keyLen total length of new generated key bytes
        * @return user keybytes {@link byte[]}
        */
        public static byte[] getUserKeyBytes(String key, String keyHash, int keyLen)  {
            if (key == null || key.length() == 0)
                throw new IllegalArgumentException("key");

            byte[] keyBytes = key.getBytes(Charset.forName("UTF-8"));
            // keyHash = (string.IsNullOrEmpty(keyHash)) ? EnDeCodeHelper.KeyToHex(key) : keyHash;
            byte[] hashBytes = new byte[0];
            if (keyHash == null || keyHash.length() == 0)
                hashBytes = ((new Hex16Coder()).encodeBytesToString(keyBytes)).getBytes(Charset.forName("UTF-8"));
            else
                hashBytes = keyHash.getBytes(Charset.forName("UTF-8"));

            int keyByteCnt = -1;
            keyLen = (keyLen > Constants.MAX_KEY_LEN) ? Constants.MAX_KEY_LEN : keyLen;
            String keyByteHashString = key;
            byte[] tmpKey = new byte[keyLen];

            byte[] keyHashBytes = keyHashBytes(keyBytes, hashBytes, true);
            keyByteCnt = keyHashBytes.length;
            byte[] keyHashTarBytes = new byte[keyByteCnt * 2 + 1];

            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = tarBytes(keyHashBytes,
                        keyHashBytes(hashBytes, keyBytes, true));
                keyByteCnt = keyHashTarBytes.length;
                keyHashBytes = new byte[keyByteCnt];
                System.arraycopy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = tarBytes(keyHashBytes,
                                    keyHashBytes(hashBytes, keyBytes, true),
                                    keyHashBytes(keyBytes, hashBytes, true));
                keyByteCnt = keyHashTarBytes.length;
                keyHashBytes = new byte[keyByteCnt];
                System.arraycopy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            while (keyByteCnt < keyLen)
            {
                keyHashTarBytes = tarBytes(keyHashBytes, keyHashBytes);
                keyByteCnt = keyHashTarBytes.length;
                keyHashBytes = new byte[keyByteCnt];
                System.arraycopy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            if (keyLen <= keyByteCnt)
            {
                // Array.Copy(keyHashBytes, 0, tmpKey, 0, keyLen);
                int bytIdx = 0;
                for (bytIdx = 0; bytIdx < keyLen; bytIdx++)
                    tmpKey[bytIdx] = keyHashBytes[bytIdx];
            }

            return tmpKey;

        }


        /**
         * getKeyBytesFromBytes
         * @param keyBytes users keybytes
         * @param keyLen maximum length, that wil be needed for stretching key bytes
         * @return key bytes stretched to length by adding one or many different key hashes
         */
        public static byte[] getKeyBytesFromBytes(byte[] keyBytes, int keyLen)  {
            if (keyBytes == null || keyBytes.length == 0)
                throw new IllegalArgumentException("keyBytes");

            byte[] hashBytes = ((new Hex16Coder()).encodeBytesToString(keyBytes)).getBytes(Charset.forName("UTF-8"));

            int keyByteCnt = -1;
            keyLen = (keyLen > Constants.MAX_KEY_LEN) ? Constants.MAX_KEY_LEN : keyLen;
            byte[] tmpKey = new byte[keyLen];

            byte[] keyHashBytes = keyHashBytes(keyBytes, hashBytes, true);
            keyByteCnt = keyHashBytes.length;
            byte[] keyHashTarBytes = new byte[keyByteCnt * 2 + 1];

            if (keyByteCnt < keyLen) {
                keyHashTarBytes = tarBytes(keyHashBytes, keyHashBytes(hashBytes, keyBytes, true));
                keyByteCnt = keyHashTarBytes.length;
                keyHashBytes = new byte[keyByteCnt];
                System.arraycopy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen) {
                keyHashTarBytes = tarBytes(keyHashBytes,
                        keyHashBytes(hashBytes, keyBytes, true),
                        keyHashBytes(keyBytes, hashBytes, true)
                );
                keyByteCnt = keyHashTarBytes.length;
                keyHashBytes = new byte[keyByteCnt];
                System.arraycopy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            while (keyByteCnt < keyLen)  {
                keyHashTarBytes = tarBytes(keyHashBytes, keyHashBytes);
                keyByteCnt = keyHashTarBytes.length;
                keyHashBytes = new byte[keyByteCnt];
                System.arraycopy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            if (keyLen <= keyByteCnt)  {
                // Array.Copy(keyHashBytes, 0, tmpKey, 0, keyLen);
                int bytIdx = 0;
                for (bytIdx = 0; bytIdx < keyLen; bytIdx++)
                    tmpKey[bytIdx] = keyHashBytes[bytIdx];
            }

            return tmpKey;
        }

        // #endregion GetUserKeyBytes
}


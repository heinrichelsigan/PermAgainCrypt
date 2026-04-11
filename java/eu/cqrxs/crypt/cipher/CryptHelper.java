/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
package eu.cqrxs.crypt.cipher;

// import androidx.core.content.res.TypedArrayUtils;

// import com.google.common.primitives.Bytes;
import eu.cqrxs.crypt.encoding.EncodeEnum;
import eu.cqrxs.crypt.encoding.Hex16Coder;
import eu.cqrxs.crypt.hash.KeyHash;
import eu.cqrxs.util.Constants;
import eu.cqrxs.util.DbgWriter;
import eu.cqrxs.util.NotImplementedError;
import eu.cqrxs.zip.ZipType;

import java.util.List;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.util.ArrayList;

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
     * @throws IllegalArgumentException key
     */
	 @Deprecated
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
     * getKeyBytesSingle gets single user key bytes from users key
     * @param key users secret key
     * @param keyLen length that keybytes should have afterwards
     * @return generated user keybyte array from key and hash
     */
    public static byte[] getKeyBytesSingle(String key, int keyLen) {
        if (key == null || key.length() == 0)
            throw new IllegalArgumentException("key");

        byte[] keyBytes = key.getBytes(Charset.forName("UTF-8"));
		return getKeyBytesSingle(keyBytes, keyLen);
	}
	
	/**
     * getKeyBytesSingle gets single user key bytes from users keyBytes
     * @param keyBytes users secret keyBytes
     * @param keyLen length that keybytes should have afterwards
     * @return generated user keybyte array from keyBytes
     */
	public static byte[] getKeyBytesSingle(byte[] keyBytes, int keyLen) {
        
		byte[] outBytes = new byte[keyLen];
        if (keyBytes.length >= keyLen) {
            System.arraycopy(keyBytes, 0, outBytes, 0, keyLen);
            return outBytes;
        }

        byte[] smallBytes = keyHashBytes(keyBytes, keyBytes, true);

        if (smallBytes.length >= keyLen) {
            System.arraycopy(smallBytes, 0, outBytes, 0, keyLen);
            // System.arraycopy()
            return outBytes;
        }
        byte[] bigBytes = tarBytes(smallBytes, keyBytes);                
        if (bigBytes.length >= keyLen) {
            System.arraycopy(bigBytes, 0, outBytes, 0, keyLen);
            // System.arraycopy()
            return outBytes;
        }

        // return outBytes;
        return getKeyHashBytes(smallBytes, bigBytes, keyLen);
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
		byte[] hashBytes = keyHash.getBytes(Charset.forName("UTF-8"));
        byte[] outBytes = new byte[keyLen];
        if (keyBytes.length >= keyLen) {            
			System.arraycopy(keyBytes, 0, outBytes, 0, keyLen);			
            return outBytes;
        }

        byte[] smallBytes = keyHashBytes(keyBytes, hashBytes, true);

        if (smallBytes.length >= keyLen) {
            System.arraycopy(smallBytes, 0, outBytes, 0, keyLen);			
            // System.arraycopy()
            return outBytes;
        }
        byte[] bigBytes = tarBytes(smallBytes, tarBytes(keyBytes, hashBytes));
        if (bigBytes.length >= keyLen) {
            System.arraycopy(bigBytes, 0, outBytes, 0, keyLen);			
            // System.arraycopy()
            return outBytes;
        }

		// return outBytes;
		return getKeyHashBytes(smallBytes, bigBytes, keyLen);                
    }


       /***
        * getUserKeyBytes
        * @param key users secret key
        * @param keyHash hashed key
        * @param keyLen total length of new generated key bytes
        * @return user key hash bytes
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
						
			return getKeyHashBytes(keyBytes, hashBytes, keyLen);
		}

		/**
		 * getKeyHashBytes
         * @param keyBytes users secret keyBytes
		 * @param hashBytes 
         * @param keyHash hashed key
         * @param keyLen total length of new generated key bytes
         * @return user key hash bytes
         */
        public static byte[] getKeyHashBytes(byte[] keyBytes, byte[] hashBytes, int keyLen)  {
			
			if (keyBytes == null || keyBytes.length == 0)
                throw new IllegalArgumentException("keyBytes");
			if (hashBytes == null || hashBytes.length == 0)
				hashBytes = ((new Hex16Coder()).encodeBytesToString(keyBytes)).getBytes(Charset.forName("UTF-8"));

            int keyByteCnt = -1;
            keyLen = (keyLen > Constants.MAX_KEY_LEN) ? Constants.MAX_KEY_LEN : keyLen;
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


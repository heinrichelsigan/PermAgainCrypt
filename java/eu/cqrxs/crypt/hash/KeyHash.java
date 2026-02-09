/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 34
 *
 * Coded 2021-2028 by <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
.*/

package eu.cqrxs.crypt.hash;

import java.io.Serializable;
import java.lang.String;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.util.HexFormat;
import java.util.*;

import eu.cqrxs.crypt.cipher.CipherEnum;
import eu.cqrxs.crypt.cipher.CryptHelper;
import eu.cqrxs.crypt.encoding.Hex16Coder;
import eu.cqrxs.util.Constants;
import eu.cqrxs.util.DbgWriter;
import eu.cqrxs.util.CException;
import org.bouncycastle.crypto.Digest;

/**
 * KeyHash represents the enumerator for all Encoding to ascii algorithms
 */
public enum KeyHash {
    
	Hex(0x0),
	Sha1(0x1),
	OpenBSDCrypt(0x2),
	BCrypt(0x3),
	SCrypt(0x4),
	MD5(0x5),	
	Sha256(0x6),
	Sha384(0x7),
	Oct(0x8),
	Sha512(0x9),
	Whirlpool(0xa),
	Blake2xs(0xb),
	CShake(0xc),
	Dstu7564(0xd),
	RipeMD256(0xe),
	TupleHash(0xf);
	

    /**
     * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
     */
    KeyHash(int value) {
        this.value = value;
    }

    private final int value;

    /**
     * getValue
     * @return (@link int) value
     */
    public int getValue() { return value; }

	final static KeyHash[] orderedHashes = { KeyHash.BCrypt, KeyHash.Blake2xs, KeyHash.CShake, KeyHash.Dstu7564, 
		KeyHash.Hex, KeyHash.MD5, KeyHash.Oct, KeyHash.OpenBSDCrypt, KeyHash.RipeMD256, KeyHash.SCrypt, 
		KeyHash.Sha1, KeyHash.Sha256, KeyHash.Sha384, KeyHash.Sha512, KeyHash.TupleHash,  KeyHash.Whirlpool };

	final static KeyHash[] secureHashes = {
			KeyHash.BCrypt, KeyHash.Blake2xs,  KeyHash.CShake, KeyHash.Dstu7564,
			KeyHash.OpenBSDCrypt, KeyHash.SCrypt, KeyHash.RipeMD256, KeyHash.Whirlpool };

	/**
     * getName
     * @return name of enum
     */
    public String getName() {
		int xval = getValue();
		switch (xval) {
			case 0x3: 	return "BCrypt";
			case 0xb: 	return "Blake2xs";
			case 0xc: 	return "CShake";
			case 0xd: 	return "Dstu7564";
			case 0x0: 	return "Hex";					
			case 0x5: 	return "MD5";			
			case 0x8: 	return "Oct";					
			case 0x2: 	return "OpenBSDCrypt";					
			case 0xe:  	return "RipeMD256";
			case 0x4: 	return "SCrypt";
			case 0x1: 	return "Sha1";
			case 0x6: 	return "Sha256";
			case 0x7: 	return "Sha384";
			case 0x9: 	return "Sha512";
			case 0xf: 	return "TupleHash";
			case 0xa: 	return "Whirlpool";
			default:
				break;
		}
		return toString();
    }

	public static String[] getNames() {
		int cnt = 0;
		List<String> keyHashList = new ArrayList<>();
		for (KeyHash keyHash : orderedHashes)  {
			keyHashList.add(keyHash.getName());
			cnt++;
		}
		
		return keyHashList.toArray(new String[cnt]);		
    }

 	/**
     * hash hashes a {@link String} with {@link KeyHash}
     * @return hashed {@link String}
     */
    public String hash(String instr) {
		try {
			int xval = getValue();
            if (Constants.DEBUG)
                System.out.println("KeyHash: " + xval + " " + getName());
			if (instr == null || instr.isEmpty())
				throw new IllegalArgumentException("public static string hash(String instr) inBytes from instr is null!");
			byte[] inBytes = instr.getBytes(StandardCharsets.UTF_8);
			byte[] resBuf = new byte[inBytes.length];
			HexFormat hex = HexFormat.of();
			Digest digest = new org.bouncycastle.crypto.digests.NullDigest();
			String hexs = "";

			switch (getEnum(getName())) {
				
				case KeyHash.BCrypt:
					return eu.cqrxs.crypt.hash.BCrypt.hashString(instr);
					
				case KeyHash.Blake2xs:
					digest = new org.bouncycastle.crypto.digests.Blake2xsDigest(32, inBytes);
					int dgsize = digest.getDigestSize();
					
					resBuf = new byte[dgsize];
					digest.update(inBytes, 0, inBytes.length);
					digest.doFinal(resBuf, 0);

					try {
						hexs = hex.formatHex(resBuf);
					} catch (Exception ex) {
						ex.printStackTrace();
						hexs = (new eu.cqrxs.crypt.encoding.Hex16Coder()).encodeBytesToString(resBuf);
					}
					return hexs;
					
				case KeyHash.CShake:
					DbgWriter.msg((getName() + ": instr=" +instr + " \tinBytes.length=" + inBytes.length + " \t"), false);
					digest = new org.bouncycastle.crypto.digests.CSHAKEDigest(256, inBytes, CryptHelper.getKeyBytesFromBytes(inBytes, 32));
					resBuf = new byte[digest.getDigestSize()];
					digest.update(inBytes, 0, inBytes.length);
					digest.doFinal(resBuf, 0);

					try {
						hexs = hex.formatHex(resBuf);
					} catch (Exception ex) {
						ex.printStackTrace();
						hexs = (new eu.cqrxs.crypt.encoding.Hex16Coder()).encodeBytesToString(resBuf);
					}
					return hexs;
					
				case KeyHash.Dstu7564:
					DbgWriter.msg((getName() + ": instr=" +instr + " \tinBytes.length=" + inBytes.length + " \t"), false);
					digest = new org.bouncycastle.crypto.digests.DSTU7564Digest(256);
					resBuf = new byte[digest.getDigestSize()];					
					digest.update(inBytes, 0, inBytes.length);
					digest.doFinal(resBuf, 0);

					hexs = hex.formatHex(resBuf);
					return hexs;		
				
				case KeyHash.Hex:
					hexs = hex.formatHex(inBytes);
					return hexs;

				case KeyHash.MD5:
					return eu.cqrxs.crypt.hash.MD5.hashString(instr);
				
				case KeyHash.Oct:
					String hexString = hex.formatHex(inBytes);
					for (int wco = 0; wco < inBytes.length; wco++)
						hexs +=  eu.cqrxs.util.Constants.decimalToOctal(inBytes[wco]);
					return hexs;
				
				case KeyHash.OpenBSDCrypt:
					return eu.cqrxs.crypt.hash.OpenBSDCrypt.hashString(instr);				
				
				case KeyHash.RipeMD256:
					digest = new org.bouncycastle.crypto.digests.RIPEMD256Digest();
					resBuf = new byte[digest.getDigestSize()];
					digest.update(inBytes, 0, inBytes.length);
					digest.doFinal(resBuf, 0);
					hexs = hex.formatHex(resBuf);
					return hexs;
				
				case KeyHash.SCrypt:
					return eu.cqrxs.crypt.hash.SCrypt.hashString(instr);
				
				case KeyHash.Sha1:
					digest = new org.bouncycastle.crypto.digests.SHA1Digest();
					resBuf = new byte[digest.getDigestSize()];
					digest.update(inBytes, 0, inBytes.length);
					digest.doFinal(resBuf, 0);
					hexs = hex.formatHex(resBuf);					
					return hexs;
					
				case KeyHash.Sha256:
					return eu.cqrxs.crypt.hash.Sha256.hashString(instr);

				case KeyHash.Sha384:
					digest = new org.bouncycastle.crypto.digests.SHA384Digest();
					resBuf = new byte[digest.getDigestSize()];
					digest.update(inBytes, 0, inBytes.length);
					digest.doFinal(resBuf, 0);
					hexs = hex.formatHex(resBuf);					
					return hexs;					
				
				case KeyHash.Sha512:
					return eu.cqrxs.crypt.hash.Sha512.hashString(instr);

				case KeyHash.Whirlpool:
					digest = new org.bouncycastle.crypto.digests.WhirlpoolDigest();
					resBuf = new byte[digest.getDigestSize()];	
					digest.update(inBytes, 0, inBytes.length);
					digest.doFinal(resBuf, 0);
					hexs = hex.formatHex(resBuf);					
					return hexs;

				case KeyHash.TupleHash:
					DbgWriter.msg((getName() + ": instr=" +instr + " \tinBytes.length=" + inBytes.length + " \t"), false);
					digest = new org.bouncycastle.crypto.digests.TupleHash(256, inBytes, 32);
					resBuf = new byte[digest.getDigestSize()];
					digest.update(inBytes, 0, inBytes.length);
					digest.doFinal(resBuf, 0);

					try {
						hexs = hex.formatHex(resBuf);
					} catch (Exception ex) {
						ex.printStackTrace();
						hexs = (new Hex16Coder()).encodeBytesToString(resBuf);
					}
					return hexs;
								
				default:
					break;
			}
		} catch (Exception exi) { 
			exi.printStackTrace();
		}
		return "";
    }

	public static KeyHash[] getOrderedHashes() { return orderedHashes; }

	public static KeyHash[] getSecureHashes() { return secureHashes; }

	public static KeyHash[] getKeyHashes() {
		Set<KeyHash> allElementsInKeyHash =  EnumSet.allOf(KeyHash.class);
		return allElementsInKeyHash.toArray(KeyHash[]::new);
	}

	public static Set<KeyHash> getSecureHashSet() {
		return new HashSet<KeyHash>(Arrays.asList(secureHashes));
	}

	public static Set<KeyHash> getKeyHashSet() {
		Set<KeyHash> allElementsInKeyHash =  EnumSet.allOf(KeyHash.class);
		return allElementsInKeyHash;
	}
	public static KeyHash getKeyHashFromString(String stringToHash) {
		if (stringToHash != null && stringToHash != "") {
			switch (stringToHash.toLowerCase()) {
				case "scrypt":	 		return KeyHash.SCrypt;
				case "bcrypt":			return KeyHash.BCrypt;
				case "openbsd":
				case "openbsdcrypt": 	return KeyHash.OpenBSDCrypt;
				case "octal":
				case "oct": 			return KeyHash.Oct;
				case "md5":			 	return KeyHash.MD5;
				case "sha1": 			return KeyHash.Sha1;
				case "sha256":  		return KeyHash.Sha256;
				case "sha384":  		return KeyHash.Sha384;
				case "sha512": 			return KeyHash.Sha512;
				case "whirlpool": 		return KeyHash.Whirlpool;
				case "blake2":
				case "blake2xs":   		return KeyHash.Blake2xs;
				case "cshake": 			return KeyHash.CShake;
				case "dstu7564":
				case "Dstu7564":
				case "dstu756":			return KeyHash.Dstu7564;
				case "ripe":
				case "ripe256":
				case "ripemd256": 		return KeyHash.RipeMD256;
				case "tuplehash":		return KeyHash.TupleHash;

				case "hex16":
				case "hex":				return KeyHash.Hex;
				default:
					break;
			}
		}

		KeyHash kHash = Enum.valueOf(KeyHash.class, stringToHash);
		return kHash;
	}



	public static String getKeyHashExtension(KeyHash khash) {
		int xval = khash.getValue();
		String retext = "." + khash.toString().toLowerCase();
		switch (xval) {
			case 0x2: 	return ".openbsdcrypt";
			case 0xa: 	return ".whirlpool";
			case 0xe: 	return ".ripemd256";
			case 0xd: 	return ".dstu7564";
			case 0xf: 	return ".tuplehash";
			default:	break;
		}
		return retext;
	}

    /**
     * getEnum
     * @param khash String 
     * @return the enum {@link KeyHash}
     */
    public static KeyHash getEnum(String khash) {
        for (KeyHash keyHash : KeyHash.values()) {
            if (keyHash.getName() == khash)
                return keyHash;
        }
        return KeyHash.Hex;
    }

 
}


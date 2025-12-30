/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 34
 *
 * Coded 2021-2028 by <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
.*/

package eu.cqrxs.fw.crypt.hash;

import java.io.Serializable;
import java.lang.String;
import java.security.Key;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;
import java.util.Set;
import eu.cqrxs.fw.util.*;

/**
 * KeyHash represents the enumerator for all Encoding to ascii algorithms
 */
public enum KeyHash implements Serializable {
    Hex(0x0),
	OpenBSDCrypt(0x1),
	BCrypt(0x2),
	SCrypt(0x3),
	MD5(0x4),
	Sha1(0x5),
	Sha256(0x6),
	Sha384(0x7),
	Sha512(0x8),
	Whirlpool(0x9),
	Ascon256(0xa),
	Blake2xs(0xb),
	CShake(0xc),
	Dstu7564(0xd),
	RipeMD256(0xe),
	Xoodyak(0xf);
	

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


    /**
     * getName
     * @return name of enum
     */
    public String getName() {
		int xval = getValue();
		switch (xval) {
			case 0x0:
				return "Hex";
			case 0x1:
				return "OpenBSDCrypt";
			case 0x2:
				return "BCrypt";
			case 0x3:
				return "SCrypt";
			case 0x4:
				return "MD5";
			case 0x5:
				return "Sha1";
			case 0x6:
				return "Sha256";
			case 0x7:
				return "Sha384";
			case 0x8:
				return "Sha512";
			case 0x9:
				return "Whirlpool";
			case 0xa:	
				return "Ascon256";
			case 0xb:
				return "Blake2xs";
			case 0xc:
				return "CShake";
			case 0xd:
				return "Dstu7564";
			case 0xe:
				return "RipeMD256";
			case 0xf:
				return "Xoodyak";
			default:
				break;
		}
		return "Hex";
    }

	public static String[] getNames() {
		int cnt = 0;
		List<String> keyHashList = new ArrayList<>();
		for (KeyHash keyHash : KeyHash.values())  {
			keyHashList.add(keyHash.getName());
			cnt++;
		}
		
		return keyHashList.toArray(new String[cnt]);		
    }

 /**
     * getName
     * @return name of enum
     */
    public String hash(String instr) {
		try {
			int xval = getValue();
            if (Constants.DEBUG)
                System.out.
                            println("KeyHash: " + xval + " " + getName());
			switch (xval) {
				case 0x0:
					return eu.cqrxs.fw.crypt.hash.Hex.hashString(instr);
				case 0x1:
					return eu.cqrxs.fw.crypt.hash.OpenBSDCrypt.hashString(instr);
				case 0x2:
					return eu.cqrxs.fw.crypt.hash.BCrypt.hashString(instr);
				case 0x3:
					return eu.cqrxs.fw.crypt.hash.SCrypt.hashString(instr);
				case 0x4:
					return eu.cqrxs.fw.crypt.hash.MD5.hashString(instr);
				case 0x5:
					return eu.cqrxs.fw.crypt.hash.Sha1.hashString(instr);
				case 0x6:
					return eu.cqrxs.fw.crypt.hash.Sha256.hashString(instr);
				case 0x7:
					return eu.cqrxs.fw.crypt.hash.Sha384.hashString(instr);
				case 0x8:
				  return eu.cqrxs.fw.crypt.hash.Sha512.hashString(instr);
				case 0x9:
					return eu.cqrxs.fw.crypt.hash.Whirlpool.hashString(instr);
				// case 0xa:
				// 	return "Ascon256";
				case 0xb:
					return eu.cqrxs.fw.crypt.hash.Blake2xs.hashString(instr);
				case 0xc:
                    return eu.cqrxs.fw.crypt.hash.CShake.hashString(instr);
				case 0xd:
					return eu.cqrxs.fw.crypt.hash.Dstu7564.hashString(instr);
				case 0xe:
					return eu.cqrxs.fw.crypt.hash.RipeMD256.hashString(instr);
				// case 0xf:
				// return "Xoodyak";
				default:
					break;
			}
		} catch (Exception exi) { }
		return "";
    }



	public static KeyHash getKeyHashFromString(String stringToHash) {
		if (stringToHash != null && stringToHash != "") {
			switch (stringToHash) {
				case "scrypt": 
				case "SCrypt": return KeyHash.SCrypt;
				
				case "bcrypt": 
				case "BCrypt": return KeyHash.BCrypt;
				
				case "openbsd": 
				case "bsdcrypt": 
				case "openbsdcrypt": 
				case "OpenBSDCrypt": return KeyHash.OpenBSDCrypt;
				
				case "md5": 
				case "Md5": 
				case "MD5": return KeyHash.MD5;
				
				case "sha1": 
				case "Sha1": 
				case "SHA1": return KeyHash.Sha1;
				
				case "sha256": 
				case "Sha256": 
				case "SHA256": return KeyHash.Sha256;
				
				case "sha384": 
				case "Sha384":
				case "SHA384": return KeyHash.Sha384;
				
				case "sha512": 
				case "Sha512": 
				case "SHA512": return KeyHash.Sha512;
				
				case "whirlpool":
				case "Whirlpool":
				case "WhirlPool":return KeyHash.Whirlpool;
				
				case "ascon":
				case "Ascon":
				case "ascon256":
				case "Ascon256":				
				case "asconhash":
				case "Asconhash":
				case "AsconHash":
				case "asconhash256": 
				case "Asconhash256": 
				case "AsconHash256": return KeyHash.Ascon256;
				
				case "blake2":
				case "Blake2":
				case "blake2xs": 
				case "Blake2xs": 
				case "Blake2XS": return KeyHash.Blake2xs;
				
				case "shake":
				case "cshake": 
				case "Cshake": 
				case "CShake": return KeyHash.CShake;
				
				case "dstu7564": 
				case "Dstu7564": return KeyHash.Dstu7564;
				
				case "ripe":
				case "Ripe":
				case "ripe256":
				case "Ripe256":
				case "ripemd256": 
				case "RipeMD256": return KeyHash.RipeMD256;
				
				case "zodiak":
				case "Zodiak":
				case "xoodyac":
				case "Xoodyac":
				case "xoodyak":
				case "Xoodyak": return KeyHash.Xoodyak;
				
				case "hex16":
				case "Hex16":
				case "hex": 
				case "Hex":
					return KeyHash.Hex;

				default:
					break;
			}
		}
		return KeyHash.Hex;
	}



	public static Set<KeyHash> getKeyHashes() {
		Set<KeyHash> allElementsInKeyHash = EnumSet.allOf(KeyHash.class);
		return allElementsInKeyHash;
	}

	public static String getKeyHashExtension(KeyHash khash) {
		int xval = khash.getValue();
		switch (xval) {
			case 0x0:
				return ".hex";
			case 0x1:
				return ".openbsdcrypt";
			case 0x2:
				return ".bcrypt";
			case 0x3:
				return ".scrypt";
			case 0x4:
				return ".md5";
			case 0x5:
				return ".sha1";
			case 0x6:
				return ".sha256";
			case 0x7:
				return ".sha384";
			case 0x8:
				return ".sha512";
			case 0x9:
				return ".whirlpool";
			case 0xa:
				return ".ascon256";
			case 0xb:
				return ".blake2xs";
			case 0xc:
				return ".cshake";
			case 0xd:
				return ".dstu7564";
			case 0xe:
				return ".ripemd256";
			case 0xf:
				return ".xoodyak";
			default:
				break;
		}
		return ".hex";
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


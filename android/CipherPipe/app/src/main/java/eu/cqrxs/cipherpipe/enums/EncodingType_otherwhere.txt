/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.cipherpipe.enums;

import java.io.Serializable;
import java.lang.String;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;
import java.util.Set;

/**
 * EncodingType represents the enumerator for all Encoding to ascii algorithms
 */
public enum EncodingType implements Serializable {
    None(0),
	Base16(0x200),
	Hex16(0x300),
	Base32(0x400),
	Hex32(0x500),
	Uu(0x600),
	Base58(0x700),
	Base64(0x800),
	Xx(0x900);


    /**
     * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
     */
    EncodingType(int value) {
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
		int xvalue = getValue();
		switch (xvalue) {
			case 0:
				return "None";
			case 0x200:
				return "Base16";
			case 0x300:
				return "Hex16";
			case 0x400:
				return "Base32";
			case 0x500:
				return "Hex32";
			case 0x600:
				return "Uu";
			case 0x700:
				return "Base58";
			case 0x800:
				return "Base64";
			case 0x900:
				return "Xx";
			default:
				break;
		}
		return  "None";
    }

	public static String[] getNames() {
		int cnt = 0;
		List<String> encodingTypeList = new ArrayList<>();
		for (EncodingType encodingType : EncodingType.values())  {
			encodingTypeList.add(encodingType.getName());
			cnt++;
		}
		
		return encodingTypeList.toArray(new String[cnt]);		
    }

	public static Set<EncodingType> getEncodingTypes() {
		Set<EncodingType> allElementsInEncodingType = EnumSet.allOf(EncodingType.class);
		return allElementsInEncodingType;
	}

	public static String getEnCodingExtension(EncodingType etype) {
		int xvalue = etype.getValue();
		switch (xvalue) {
			case 0:
				return "";
			case 0x200:
				return ".base16";
			case 0x300:
				return ".hex16";
			case 0x400:
				return ".base32";
			case 0x500:
				return ".hex32";
			case 0x600:
				return ".uu";
			case 0x700:
				return ".base58";
			case 0x800:
				return ".base64";
			case 0x900:
				return ".xx";
			default:
				break;
		}
		return "";
	}


	public static EncodingType getEncodingTypeFromString(String enCodingString) {
		if (enCodingString != null && enCodingString != "") {
			switch (enCodingString) {
				case "raw":
				case "Raw":
				case "none":
				case "None":
				case "NONE":
				case "null":
				case "Null":
				case "0":
					return EncodingType.None;

				case "hex16":
				case "Hex16":
				case "HEX16":
				case "hex":
				case "Hex":
				case "h16":
				case "H16":
				case "16":
					return EncodingType.Hex16;

				case "base16":
				case "Base16":
				case "BASE16":
				case "b16":
				case "N16":
					return EncodingType.Base16;

				case "base32":
				case "Base32":
				case "BASE32":
				case "b32":
				case "B32":
					return EncodingType.Base32; 

				case "hex32":
				case "Hex32":
				case "HEX32":
				case "h32":
				case "H32":
				case "32":
					return EncodingType.Hex32; 

				case "uu":
				case "Uu":
				case "UU":
				case "uue":
				case "Uue":
				case "UUE":
				case "uud":
				case "Uud":
				case "UUD":
				case "uuencode":
				case "UuEncode":
				case "UUENCODE":
				case "uudecode":
				case "UuDecode":
				case "UUDECODE":
					return EncodingType.Uu;

				case "xx":
				case "Xx":
				case "XX":
				case "xxe":
				case "Xxe":
				case "XXE":
				case "xxd":
				case "Xxd":
				case "XXD":
				case "xxencode":
				case "XxEncode":
				case "XXENCODE":
				case "xxdecode":
				case "XxDecode":
				case "XXDECODE":
					return EncodingType.Xx;

				case "base64":
				case "Base64":
				case "BASE64":
				case "mime":
				case "Mime":
				case "MIME":
				case "b64":
				case "B64":
				case "64":
					return EncodingType.Base64;
				default:
					break;
			}
		}
		return EncodingType.None;
	}


 
    /**
     * getEnum
     * @param eName String
     * @return the enum {@link EncodingType}
     */
    public static EncodingType getEnum(String eName) {
        for (EncodingType encodingType : EncodingType.values()) {
            if (encodingType.getName() == eName)
                return encodingType;
        }
        return EncodingType.None;
    }

}


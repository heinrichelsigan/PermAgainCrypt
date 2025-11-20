/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.cipherpipe.crypt.encoding;

import java.io.Serializable;
import java.lang.String;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;
import java.util.Set;

import kotlin.NotImplementedError;

/**
 * EncodeEnum represents the enumerator for all Encoding to ascii algorithms
 */
public enum EncodeEnum implements Serializable {
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
    EncodeEnum(int value) {
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


    public String encode(String inString) {
        int xvalue = getValue();
        switch (xvalue) {
            case 0:
                return inString;
            case 0x200:
                return new Base16Coder().encode(inString);
            case 0x300:
                return new Hex16Coder().encode(inString);
            case 0x400:
                throw new NotImplementedError("base32 not implemented");
            case 0x500:
                throw new NotImplementedError("hex32 not implemented");
            case 0x600:
                return new UuCoder().encode(inString);
            case 0x700:
                throw new NotImplementedError("base58 not implemented");
            case 0x800:
                return new Base64Coder().encode(inString);
            case 0x900:
                throw new NotImplementedError("xx not implemented");
                // return new XxEncoder().encode(inString);
            default:
                break;
        }
        return "";
    }
    public String decode(String hexString) {
        int xvalue = getValue();
        switch (xvalue) {
            case 0:
                return hexString;
            case 0x200:
                return new Base16Coder().decode(hexString);
            case 0x300:
                return new Hex16Coder().decode(hexString);
            case 0x400:
                throw new NotImplementedError("base32 not implemented");
            case 0x500:
                throw new NotImplementedError("hex32 not implemented");
            case 0x600:
                return new UuCoder().decode(hexString);
            case 0x700:
                throw new NotImplementedError("base58 not implemented");
            case 0x800:
                return new Base64Coder().decode(hexString);
            case 0x900:
                throw new NotImplementedError("xx not implemented");
                // return new XxEncoder().encode(inString);
            default:
                break;
        }
        return hexString;
    }


    public String encodeBytesToString(byte[] inBytes) {
        int xvalue = getValue();
        switch (xvalue) {
            case 0:
                return inBytes.toString();
            case 0x200:
                return new Base16Coder().encodeBytesToString(inBytes);
            case 0x300:
                return new Hex16Coder().encodeBytesToString(inBytes);
            case 0x400:
                throw new NotImplementedError("base32 not implemented");
            case 0x500:
                throw new NotImplementedError("hex32 not implemented");
            case 0x600:
                return new UuCoder().encodeBytesToString(inBytes);
            case 0x700:
                throw new NotImplementedError("base58 not implemented");
            case 0x800:
                return new Base64Coder().encodeBytesToString(inBytes);
            case 0x900:
                throw new NotImplementedError("xx not implemented");
                // return new XxEncoder().encode(inString);
            default:
                break;
        }
        return inBytes.toString();
    }

    public byte[] decodeStringToBytes(String inString) {
        int xvalue = getValue();
        switch (xvalue) {
            case 0:
                return inString.getBytes();
            case 0x200:
                return new Base16Coder().decodeStringToBytes(inString);
            case 0x300:
                return new Hex16Coder().decodeStringToBytes(inString);
            case 0x400:
                throw new NotImplementedError("base32 not implemented");
            case 0x500:
                throw new NotImplementedError("hex32 not implemented");
            case 0x600:
                return new UuCoder().decodeStringToBytes(inString);
            case 0x700:
                throw new NotImplementedError("base58 not implemented");
            case 0x800:
                return new Base64Coder().decodeStringToBytes(inString);
            case 0x900:
                throw new NotImplementedError("xx not implemented");
                // return new XxEncoder().encode(inString);
            default:
                break;
        }
        return inString.getBytes();
    }



    public static String[] getNames() {
        int cnt = 0;
        List<String> encodingTypeList = new ArrayList<>();
        for (EncodeEnum encodingType : EncodeEnum.values())  {
            encodingTypeList.add(encodingType.getName());
            cnt++;
        }

        return encodingTypeList.toArray(new String[cnt]);
    }

    public static Set<EncodeEnum> getEncodingTypes() {
        Set<EncodeEnum> allElementsInEncodingType = EnumSet.allOf(EncodeEnum.class);
        return allElementsInEncodingType;
    }

    public static String getEnCodingExtension(EncodeEnum etype) {
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


    public static EncodeEnum getEncodingTypeFromString(String enCodingString) {
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
                    return EncodeEnum.None;

                case "hex16":
                case "Hex16":
                case "HEX16":
                case "hex":
                case "Hex":
                case "h16":
                case "H16":
                case "16":
                    return EncodeEnum.Hex16;

                case "base16":
                case "Base16":
                case "BASE16":
                case "b16":
                case "N16":
                    return EncodeEnum.Base16;

                case "base32":
                case "Base32":
                case "BASE32":
                case "b32":
                case "B32":
                    return EncodeEnum.Base32;

                case "hex32":
                case "Hex32":
                case "HEX32":
                case "h32":
                case "H32":
                case "32":
                    return EncodeEnum.Hex32;

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
                    return EncodeEnum.Uu;

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
                    return EncodeEnum.Xx;

                case "base64":
                case "Base64":
                case "BASE64":
                case "mime":
                case "Mime":
                case "MIME":
                case "b64":
                case "B64":
                case "64":
                    return EncodeEnum.Base64;
                default:
                    break;
            }
        }
        return EncodeEnum.None;
    }



    /**
     * getEnum
     * @param eName String
     * @return the enum {@link EncodeEnum}
     */
    public static EncodeEnum getEnum(String eName) {
        for (EncodeEnum encodEnum : EncodeEnum.values()) {
            if (encodEnum.getName() == eName)
                return encodEnum;
        }
        return EncodeEnum.None;
    }

}


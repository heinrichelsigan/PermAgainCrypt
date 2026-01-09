/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.crypt.cipher;

import java.io.Serializable;
import java.lang.String;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;
import java.util.Set;
import eu.cqrxs.util.*;

import javax.crypto.Cipher;

/**
 * SymmCipherEnum represents the enumerator for all symmetric cipher algorithms
 */
public enum SymmCipherEnum implements Serializable {
    Aes(0),
    BlowFish(1),
    Camellia(2),
    Cast6(3),
    Des3(4),
    Fish2(5),
    Fish3G(6),
    Gost28147(7),
    Idea(8),
    RC532(9),
    Seed(10),
    SkipJack(11),
    Serpent(12),
    Tea(13),
    XTea(14),
    SM4(15);

    /**
     * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
     */
    SymmCipherEnum(int value) {
        this.value = value;
    }

    private final int value;

    /**
     * getValue
     *
     * @return (@ link int) value
     */
    public int getValue() {
        return value;
    }

    /**
     * getSymmCipherChar
     *
     * @return upper letter {@link char}
     */
    public char getSymmCipherChar() {
        int value = this.getValue();
        switch (value) {
            case 0:
                return 'A';    // Aes
            case 1:
                return 'b';    // BlowFish
            case 2:
                return 'C';    // Camellia
            case 3:
                return '6';    // Cast6
            case 4:
                return 'D';    // Des3
            case 5:
                return 'f';    // Fish2
            case 6:
                return 'F';    // Fish3
            case 7:
                return 'g';    // Gost28147

            case 8:
                return 'I';        // Idea
            case 9:
                return '5';        // RC532
            case 10:
                return 's';    // Seed
            case 11:
                return 'S';    // Serpent
            case 12:
                return '4';    // SM4
            case 13:
                return 'J';    // SkipJack
            case 14:
                return 't';    // Tea
            case 15:
                return 'X';    // XTea
            default:
                break; // Aes
        }
        return 'A';    // Aes
    }

    /**
     * getChar
     *
     * @return (by default upper case) letter of {@link char}
     */
    public char getChar() {
        return getSymmCipherChar();
    }

    public CipherEnum toCipherEnum() {
        return SymmCipherEnum.toCipherEnum(this);
    }

    public static CipherEnum toCipherEnum(SymmCipherEnum symmCipher) {
        switch (symmCipher.getValue()) {

            case 0x1: return CipherEnum.BlowFish;
            case 0x5:
                return CipherEnum.Fish2;
            case 0x6:
                return CipherEnum.Fish3;

            case 0x2:
                return CipherEnum.Camellia;
            case 0x3:
                return CipherEnum.Cast6;
            case 0x4:
                return CipherEnum.Des3;

            case 0x7:
                return CipherEnum.Gost28147;
            case 0x8:
                return CipherEnum.Idea;
            case 0x9:
                return CipherEnum.RC532;

            case 0xa:
                return CipherEnum.Seed;
            case 0xc:
                return CipherEnum.Serpent;
            case 0xb:
                return CipherEnum.SkipJack;

            case 0xd:
                return CipherEnum.Tea;
            case 0xe:
                return CipherEnum.XTea;
            case 0xf:
                return CipherEnum.SM4;

            default: break;
        }
        return CipherEnum.Aes;
    }

    public static Set<SymmCipherEnum> getSymmCipherEnums() {
        Set<SymmCipherEnum> allElementsInSymmCipherEnum = EnumSet.allOf(SymmCipherEnum.class);
        return allElementsInSymmCipherEnum;
    }

    /**
     * getName
     *
     * @return name of enum
     */
    public String getName() {
        int value = this.getValue();
        switch (value) {
            case 0:
                return "Aes";    // Aes
            case 1:
                return "BlowFish";    // BlowFish
            case 2:
                return "Camellia";    // Camellia
            case 3:
                return "Cast6";    // Cast6
            case 4:
                return "Des3";    // Des3
            case 5:
                return "Fish2";    // Fish2
            case 6:
                return "Fish3";    // Fish3
            case 7:
                return "Gost28147";    // Gost28147

            case 8:
                return "Idea";        // Idea
            case 9:
                return "RC532";        // RC532
            case 10:
                return "Seed";    // Seed
            case 11:
                return "Serpent";    // Serpent
            case 12:
                return "SM4";    // SM4
            case 13:
                return "SkipJack";    // SkipJack
            case 14:
                return "Tea";    // Tea
            case 15:
                return "XTea";    // XTea

            default:
                break;
        }
        return "Aes";    // Aes
    }

    public static String[]  getNames() {
		int cnt = 0;
		List<String> symmCipherEnumList = new ArrayList<>();
        for (SymmCipherEnum symmCipherEnum : SymmCipherEnum.values()) {			
            symmCipherEnumList.add(symmCipherEnum.getName());
			cnt++;
		}                
        return symmCipherEnumList.toArray(new String[cnt]);
    }

 
    /**
     * getEnum
     * @param ch column character
     * @return the enum {@link COLUMN}
     */
    public static SymmCipherEnum getEnum(char ch) {
        for (SymmCipherEnum symmCipherEnum : SymmCipherEnum.values()) {
            if (symmCipherEnum.getChar() == ch)
                return symmCipherEnum;
        }
        return SymmCipherEnum.Aes;
    }

    public static SymmCipherEnum getEnum(String symmciphername) {
        for (SymmCipherEnum symmCipherEnum : SymmCipherEnum.values()) {
            if (symmCipherEnum.getName() == symmciphername)
                return symmCipherEnum;
        }
        return SymmCipherEnum.Aes;
    }


}


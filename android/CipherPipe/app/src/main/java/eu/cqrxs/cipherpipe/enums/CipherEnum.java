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

import androidx.annotation.NonNull;

import java.io.Serializable;
import java.lang.String;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;
import java.util.Set;

/**
 * CipherEnum represents the enumerator for all cipher algorithms
 * implements Serializable
 */
public enum CipherEnum implements Serializable {
    Aes(0),
    BlowFish(1),
    Camellia(2),
    Cast6(3),
    Des3(4),
    Fish2(5),
    Fish3(6),
    Gost28147(7),
    Idea(8),
    RC532(9),
    Seed(10),
    SkipJack(11),
    Serpent(12),
    Tea(13),
    XTea(14),
	SM4(15),
	
	Cast5(16),
	Rijndael(17),
	Noekeon(18),
	RC2(19),
	RC564(20),
	RC6(21),
	Tnepres(22),
	Des(23),
	Aria(24),
	CamelliaLight(25),
	Dstu7624(26),
	AesLight(27),
	ThreeFish256(28),

	Des3Net(29),
	AesNet(30),
	ZenMatrix(31),
	ZenMatrix2(32),

	Rsa(33);

    /**
     * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
     */
    CipherEnum(int value) {
        this.value = value;
    }

    private final int value;

    /**
     * getValue
     * @return (@link int) value
     */
    public int getValue() { return value; }

    /**
     * getCipherChar
     * @return upper letter {@link char}
     */
    public char getCipherChar() {
        int xvalue = this.getValue();
		switch (xvalue) {
			case 0: return 'A'; 	// Aes
			case 27: return 'L'; 	// AesLight
			case 30: return 'E'; 	// AesNet
			case 17:  return 'j'; 	// Rijndael
			case 24: return 'a'; 	// Aria
			
			case 1: return 'b'; 	// BlowFish
			case 5: return 'f'; 	// Fish2
			case 6: return 'F'; 	// Fish3
			case 28: return '3'; 	// ThreeFish256
			
			case 2: return 'C'; 	// Camellia
			case 25: return 'l';	// CamelliaLight
			case 16: return 'c'; 	// Casz5
			case 3: return '6'; 	// Cast6
			
			case 23: return '$'; 	// Des
			case 4: return 'D'; 	// Des3
			case 29: return 'e';	// Des3Net
			case 26: return 'd';	// Dstu7624
					
			case 7: return 'g'; 	// Gost28147
			case 8: return 'I';	 	// Idea
			case 18: return 'N'; 	// Noekeon
			
			case 19: return '2';  	// RC2
			case 9: return '5';		// RC532
			case 20: return 'R'; 	// RC564
			case 21: return 'r'; 	// RC6
			
			case 10: return 's';	// Seed
			case 11: return 'S'; 	// Serpent
			case 12: return '4'; 	// SM4
			case 13: return 'J';	// SkipJack
			
			case 14: return 't'; 	// Tea
			case 22: return 'T'; 	// Tnepres
			case 15: return 'X'; 	// XTea
			
			case 31: return 'z';	// ZenMatrix
            case 32: return 'Z'; 	// ZenMatrix2
			
			case 33: return '%'; 	// RSA asymmetric cipher
			
			default: break;  		// Aes
		}
        return 'A'; 	// Aes
    }

    /**
     * getChar
     * @return (by default upper case) letter of {@link char}
     */
    public char getChar() {
        return getCipherChar();
    }


	public static Set<CipherEnum> getCipherEnums() {
		Set<CipherEnum> allElementsInCipherEnum = EnumSet.allOf(CipherEnum.class);
		return allElementsInCipherEnum;
	}


    /**
     * getName
     * @return name of enum
     */
    public String getName() {
		int xvalue = this.getValue();
		switch (xvalue) {
			case 0:
				return "Aes";    // Aes
			case 27:
				return "AesLight";    // AesLight
			case 30:
				return "AesNet";            // AesNet
			case 17:
				return "Rijndael";   // Rijndael
			case 24:
				return "Aria";    // Aria

			case 1:
				return "BlowFish";    // BlowFish
			case 5:
				return "Fish2";    // Fish2
			case 6:
				return "Fish3";    // Fish3
			case 28:
				return "ThreeFish256";    // ThreeFish256

			case 2:
				return "Camellia";    // Camellia
			case 25:
				return "CamelliaLight";    // CamelliaLight
			case 16:
				return "Cast5";    // Cast5
			case 3:
				return "Cast6";    // Cast6

			case 23:
				return "Des";    // Des
			case 4:
				return "Des3";   // Des3
			case 29:
				return "Des3Net";    // Des3Net
			case 26:
				return "Dstu7624";    // Dstu7624

			case 7:
				return "Gost28147";    // Gost28147
			case 8:
				return "Idea";        // Idea
			case 18:
				return "Noekeon";    // Noekeon

			case 19:
				return "RC2";    // RC2
			case 9:
				return "RC532";        // RC532
			case 20:
				return "R564";    // RC564
			case 21:
				return "RC6";    // RC6

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
			case 22:
				return "Tnepres";    // Tnepres
			case 15:
				return "XTea";    // XTea

			case 31:
				return "ZenMatrix";    // ZenMatrix
			case 32:
				return "ZenMatrix2";    // ZenMatrix2

			case 33:
				return "RSA";    // RSA asymmetric cipher

			default:
				break;     		// Aes
		}
		return "Aes";    		// Aes
	}

   @NonNull
   public static String[] getNames() {
		int cnt = 0;
		List<String> cnames = new ArrayList<String>();
		for (CipherEnum cipherEnum : CipherEnum.values())  {
			cnames.add(cipherEnum.getName());
			cnt++;
		}
		
		return cnames.toArray(new String[cnt]);		
    }

 

 
    /**
     * getEnum
     * @param ch column character
     * @return the enum {@link COLUMN}
     */
    public static CipherEnum getEnum(char ch) {
        for (CipherEnum cipherEnum : CipherEnum.values()) {
            if (cipherEnum.getChar() == ch)
                return cipherEnum;
        }
        return CipherEnum.Aes;
    }

    public static CipherEnum getEnum(String ciphername) {
        for (CipherEnum cipherEnum : CipherEnum.values()) {
            if (cipherEnum.getName() == ciphername)
                return cipherEnum;
        }
        return CipherEnum.Aes;
    }

}


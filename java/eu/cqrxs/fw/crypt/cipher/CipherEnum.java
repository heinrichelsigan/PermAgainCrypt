/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.fw.crypt.cipher;


import java.io.Serializable;
import java.lang.String;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.HashMap;
import java.util.List;
import java.util.Comparator;
import java.util.Collections;
import java.util.Map;
import java.util.Set;
import eu.cqrxs.fw.util.*;

import eu.cqrxs.fw.util.Constants;

/**
 * CipherEnum represents the enumerator for all cipher algorithms
 * implements Serializable
 */
public enum CipherEnum implements Serializable {
    Aes(0x0),
    BlowFish(0x1),
    Camellia(0x2),
    Cast6(0x3),
    Des3(0x4),
    Fish2(0x5),
    Fish3(0x6),
    Gost28147(0x7),
    Idea(0x8),
    RC532(0x9),
    Seed(0xa),
    SkipJack(0xb),
    Serpent(0xc),
    Tea(0xd),
    XTea(0xe),
	SM4(0xf),
	
	Cast5(0x10),
	Rijndael(0x11),
	Noekeon(0x12),
	RC2(0x13),
	RC564(0x14),
	RC6(0x15),
	Tnepres(0x16),
	Des(0x17),
	Aria(0x18),
	CamelliaLight(0x19),
	Dstu7624(0x1a),
	AesLight(0x1b),
	ZenMatrix(0x1c),

	Des3Net(0x1d),
	AesNet(0x1e),
	ZenMatrix2(0x1f),

    Rsa(0x21)
	/* DH(0x22) */
	;


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

    public byte getByteValue() { return ((byte)value); }

    /**
     * getCipherChar
     * @return upper letter {@link char}
     */
    public char getCipherChar() {
        int xvalue = this.getValue();
		switch (xvalue) {
			case 0x0: 	return 'A'; 	// Aes
			case 0x1b: 	return 'L'; 	// AesLight
			case 0x11: 	return 'j'; 	// Rijndael
			case 0x18: 	return 'a'; 	// Aria
			
			case 0x1: 	return 'b'; 	// BlowFish
			case 0x5: 	return 'f'; 	// Fish2
			case 0x6: 	return 'F'; 	// Fish3			
			
			case 0x2: 	return 'C'; 	// Camellia
			case 0x19: 	return 'l';		// CamelliaLight
			case 0x10: 	return 'c'; 	// Casz5
			case 0x3: 	return '6'; 	// Cast6
			
			case 0x17: 	return '$'; 	// Des
			case 0x4: 	return 'D'; 	// Des3
			
			case 0x1a: 	return 'd';		// Dstu7624
					
			case 0x7: 	return 'g'; 	// Gost28147
			case 0x8: 	return 'I';	 	// Idea
			case 0x12: 	return 'N'; 	// Noekeon
			
			case 0x13: 	return '2';  	// RC2
			case 0x9: 	return '5';		// RC532
			case 0x14: 	return 'R'; 	// RC564
			case 0x15: 	return 'r'; 	// RC6
			
			case 0xa: 	return 's';		// Seed
			case 0xc:	return 'S'; 	// Serpent
			case 0xf: 	return '4'; 	// SM4
			case 0xb: 	return 'J';		// SkipJack
			
			case 0xd: 	return 't'; 	// Tea
			case 0x16: 	return 'T'; 	// Tnepres
			case 0xe: 	return 'X'; 	// XTea
			
			case 0x1c: 	return 'z';		// ZenMatrix
			case 0x1d: 	return 'e';		// Des3Net
			case 0x1e: 	return 'E'; 	// AesNet
            case 0x1f: 	return 'Z'; 	// ZenMatrix2
			
			case 0x21: return '%'; 		// RSA asymmetric cipher
			
			default: break;  			// Aes
		}
        return 'A'; 	// Aes
    }

    public static CipherEnum fromString(String algo) {
        String alg = (algo != null && algo.length() > 0) ? algo : "Aes";
        CipherEnum cEnum = CipherEnum.Aes;
        try {
            cEnum = CipherEnum.valueOf(CipherEnum.class, alg);
        }  catch (Exception exEnum) {
            cEnum = CipherEnum.Aes;
        }
        return cEnum;
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
		// allElementsInCipherEnum.stream().sorted().collect(Collectors.toList());
		return allElementsInCipherEnum;
	}


    /**
     * getName
     * @return name of enum
     */
    public String getName() {
		int xvalue = this.getValue();
		switch (xvalue) {
			case 0x0: 	return "Aes"; 			// Aes
			case 0x1b: 	return "AesLight"; 		// AesLight		
			case 0x11: 	return "Rijndael";		// Rijndael
			case 0x18: 	return "Aria"; 			// Aria
			
			case 0x1: 	return "BlowFish"; 		// BlowFish
			case 0x5: 	return "Fish2"; 		// Fish2
			case 0x6: 	return "Fish3"; 		// Fish3
			
			case 0x2: 	return "Camellia"; 		// Camellia
			case 0x19: 	return "CamelliaLight";	// CamelliaLight
			case 0x10: 	return "Cast5"; 		// Cast5
			case 0x3: 	return "Cast6"; 		// Cast6
			
			case 0x17: 	return "Des"; 			// Des
			case 0x4: 	return "Des3"; 			// Des3			
			case 0x1a: 	return "Dstu7624";		// Dstu7624
					
			case 0x7: 	return "Gost28147"; 	// Gost28147
			case 0x8: 	return "Idea";	 		// Idea
			case 0x12: 	return "Noekeon"; 		// Noekeon
			
			case 0x13: 	return "RC2"; 		 	// RC2
			case 0x9: 	return "RC532";			// RC532
			case 0x14: 	return "RC564"; 		// RC564
			case 0x15: 	return "RC6"; 			// RC6
			
			case 0xa: 	return "Seed";			// Seed
			case 0xc:	return "Serpent"; 		// Serpent
			case 0xf: 	return "SM4"; 			// SM4
			case 0xb: 	return "SkipJack";		// SkipJack
			
			case 0xd: 	return "Tea";		 	// Tea
			case 0x16: 	return "Tnepres"; 		// Tnepres
			case 0xe: 	return "XTea"; 			// XTea
			
			case 0x1c: 	return "ZenMatrix";		// ZenMatrix
			case 0x1d: 	return "Des3Net";		// Des3Net
            case 0x1e: 	return "AesNet"; 		// AesNet
			case 0x1f: 	return "ZenMatrix2"; 	// ZenMatrix2
			
			case 0x21: 	return "Rsa"; 			// Rsa asymmetric cipher
			
			default: break;  			// Aes						
		}
		return "Aes";    		// Aes
	}

   
   public static String[] getNames() {
		int cnt = 0;
		List<String> cnames = new ArrayList<String>();
		for (CipherEnum cipherEnum : CipherEnum.values())  {
			cnames.add(cipherEnum.getName());
			cnt++;
		}
		Collections.sort(cnames, new Comparator<String>() {
            @Override
            public int compare(String s0, String s1) {                
                return s0.compareToIgnoreCase(s1);
            }
        });
		
		return cnames.toArray(new String[cnt]);		
    }

    /***
     * getByteCipherDict()
     * @return a HashMap Map<byte,CipherEnum>
     */
    public static HashMap<Byte,CipherEnum> getByteCipherDict() {

        HashMap<Byte,CipherEnum> map = new HashMap<Byte,CipherEnum> ();
        for (CipherEnum cipherEnum : CipherEnum.values())  {
            Byte b = Byte.valueOf(((byte)cipherEnum.getByteValue()));
            map.put(b, cipherEnum);
        }
        return map;
    }


    /***
     * parsePipeText parses a ;, concatenated string into it's parts and converts
     * substrings to CipherEnums
     * @param pipeText concatenated pipe text
     * @return an arreay of CipherEnum
     */
    public static CipherEnum[] parsePipeText(String pipeText) {
        CipherEnum cipher = CipherEnum.Aes;
        List<CipherEnum> cipherList = new ArrayList<CipherEnum>();
        pipeText = (pipeText == null) ? "" : pipeText;

        int pipeCnt = 0;
        String[] algos = pipeText.split(Constants.COOL_CRYPT_SPLIT);
        for (String algo : algos)
        {
            cipher = CipherEnum.fromString(algo);
            cipherList.add(cipher);
            if ((++pipeCnt) >= 8)
                break;
        }

        CipherEnum[] cipherEnums = new CipherEnum[cipherList.size()];
        for (int ci = 0; ci < cipherList.size(); ci ++) {
            cipherEnums[ci] = cipherList.get(ci);
        }
        return cipherEnums;
    }

    public static CipherEnum fromSymmCipherEnum(SymmCipherEnum symmCipherEnum) {
        return symmCipherEnum.toCipherEnum();
    }


    /**
     * getEnum
     * @param ch column character
     * @return the enum {@link CipherEnum}
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


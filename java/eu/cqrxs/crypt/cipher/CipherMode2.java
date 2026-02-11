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

import eu.cqrxs.util.Constants;

import java.util.*;


/**
 * CipherMode2 represents the enumerator for all cipher modes to en-/decrypt
 */
public enum CipherMode2 {
	ECB(0x0),
	CBC(0x1),
	CFB(0x2),
	CCM(0x3),
	CTS(0x4),
	EAX(0x5),
	GOFB(0x6)
    ;


    /**
     * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
     */
    CipherMode2(int value) {
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
	 * fromString
	 * @param stringToParse String text to parse
	 * @return {@link CipherMode2}
	 */
    public static CipherMode2 fromString(String stringToParse) {
        String textForEnum = (stringToParse != null && !stringToParse.isEmpty()) ? stringToParse : "CFB";
        CipherMode2 cMode2 = CipherMode2.CFB;
        try {
			cMode2 = CipherMode2.valueOf(CipherMode2.class, textForEnum);
        }  catch (Exception exEnum) {
			cMode2 = CipherMode2.CFB;
        }
        return cMode2;
    }


	/**
	 * getCopherModes2()
	 * @return a {@link Set} of {@link CipherMode2}
	 */
	public static Set<CipherMode2> getCipherModes2() {
		Set<CipherMode2> allElementsInCipherEnum = EnumSet.allOf(CipherMode2.class);
		// allElementsInCipherEnum.stream().sorted().collect(Collectors.toList());
		return allElementsInCipherEnum;
	}


    /**
     * getName
     * @return name of enum
     */
    public String getName() {
		CipherMode2 cMode2 = CipherMode2.getCipherMode2(getByteValue());;
		switch (cMode2) {
			case CBC: 	return "CBC";
			case CFB: 	return "CFB";
			case CCM: 	return "CCM";
			case CTS: 	return "CTS";
			case EAX: 	return "EAX";
			case ECB: 	return "ECB";
			case GOFB: 	return "GOFB";
			default: 	break;
		}
		return "CFB";    		// CFB_DEFAULT
	}

	/**
	 * getNames()
	 * @return  all enum states as {@link String[]}
	 */
   	public static String[] getNames() {
		int cnt = 0;
		List<String> cnames = new ArrayList<String>();
		for (CipherMode2 cipherMode2 : CipherMode2.values())  {
			cnames.add(cipherMode2.getName());
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
     * getMap()
     * @return HashMap{byte,CipherMode2}
     */
    public static HashMap<Byte, CipherMode2> getMap() {

        HashMap<Byte, CipherMode2> map = new HashMap<Byte, CipherMode2> ();
        for (CipherMode2 cipherEnum : CipherMode2.values())  {
            Byte b = Byte.valueOf(((byte)cipherEnum.getByteValue()));
            map.put(b, cipherEnum);
        }
        return map;
    }




    /**
     * getCipherMode2
     * @param bvalue byte value
     * @return the enum {@link CipherMode2}
     */
    public static CipherMode2 getCipherMode2(byte bvalue) {
        for (CipherMode2 cipherEnum : CipherMode2.values()) {
            if (cipherEnum.getByteValue() == bvalue)
                return cipherEnum;
        }
        return CipherMode2.CFB;
    }

	/**
	 * getEnum
	 * @param cipherMode2 string
	 * @return the enum {@link CipherMode2}
	 */
    public static CipherMode2 getEnum(String cipherMode2) {
        for (CipherMode2 cipherEnum : CipherMode2.values()) {
            if (cipherEnum.getName() == cipherMode2)
                return cipherEnum;
        }
        return CipherMode2.CFB;
    }

}


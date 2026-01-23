/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.console;

import java.io.Serializable;
import java.lang.String;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;
import java.util.Set;

/**
 * OptEnum different option types
 */
public enum OptEnum {

	Usage(0x0),
	InParam(0x1), 
    Key(0x2),
	Hash(0x3),
	Zip(0x4),
    CipherAlgos(0x5),
	Encode(0x6),
	OutP(0x7),
	Crypt(0x7),
	Decrypt(0x8),
	SymmCipher(0x9),
    Verbose(0xe),
    Help(0xf);

    /** 
     * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier. 
     */ 
     OptEnum(int value) { 
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
		int evalue = getValue(); 
        for (OptEnum optEnum : OptEnum.values()) 
            if (optEnum.getValue() == evalue) 
                return optEnum.toString();
		return "Usage";
	}


	public static String[] getNames() {
		int cnt = 0;
		List<String> optEnumList = new ArrayList<>();
		for (OptEnum optEnum : OptEnum.values()) {
			optEnumList.add(optEnum.getName());
			cnt++;
		}
		
		return optEnumList.toArray(new String[cnt]);		
	}

	public static OptEnum getOptionFromString(String stringOp) {
		if (stringOp != null && stringOp != "") 
			return getEnum(stringOp);
		return OptEnum.Usage;
	}


	public static Set<OptEnum> getOptions() {
		Set<OptEnum> allElementsInOptions = EnumSet.allOf(OptEnum.class);
		return allElementsInOptions;
	}


    /**
     * getEnum
     * @param eName
     * @return the enum {@link OptEnum}
     */
    public static OptEnum getEnum(String eName) {
        for (OptEnum optEnum : OptEnum.values()) {
            if (optEnum.getName() == eName)
                return optEnum;
        }
        return OptEnum.Usage;
    }

}


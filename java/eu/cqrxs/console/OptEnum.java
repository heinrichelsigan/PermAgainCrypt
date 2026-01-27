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


	/***
	 * getOptArg gets an option by argument
	 * @param argument the argument
	 * @return {@link String[]}
	 */
	public static String[] getOptArg(String argument) {
		String[] optArgs = new String[2];
		optArgs[0] = OptEnum.Usage.toString();
		optArgs[1] = "";

		// System.out.println("getOptArg(String argument = " + argument +  ") ...");
		if (argument == null || argument.length() < 2)   {
			return optArgs;
		}
		String optArg = argument;

		String arg = (argument.charAt(1) == '-') ? argument.substring(2) :
				argument.substring(1);

		if (arg.contains("="))
			optArg = arg.substring(arg.indexOf('=') + 1);
		// else if (arg.contains(":"))
		//     optArg =  arg.substring(arg.indexOf(':') + 1);

		// System.out.println("arg=" + arg +  " optArg=" + optArg);

		optArgs[1] = optArg;
		switch (arg.charAt(0)) {
			case 'I':
			case 'i':
				optArgs[0] = OptEnum.InParam.toString();
				return optArgs;
			case 'O':
			case 'o':
				optArgs[0] =  OptEnum.OutP.toString();
				return optArgs;
			case 'Z':
			case 'z':
				optArgs[0] = OptEnum.Zip.toString();
				return optArgs;
			case 'E':
			case 'e':
				optArgs[0] = OptEnum.Encode.toString();
				return optArgs;
			case 'D':
			case 'd':
				optArgs[0] = OptEnum.Decrypt.toString();
				return optArgs;
			case 'C':
			case 'c':
				optArgs[0] = OptEnum.Crypt.toString();
				return optArgs;
			case 'k':
			case 'K':
				optArgs[0] = OptEnum.Key.toString();
				return optArgs;
			case 'h':
			case 'H':
				optArgs[0] = OptEnum.Hash.toString();
				return optArgs;
			case 'S':
				optArgs[0] = OptEnum.SymmCipher.toString();
				return optArgs;
			case 'v':
			case 'V':
				optArgs[0] = OptEnum.Verbose.toString();
				return optArgs;
			case 'g':
			case 'G':
			case '?':
			default:
				optArgs[0] = OptEnum.Usage.toString();
				optArgs[1] = "unrecognized option: " + argument + ".";
				return optArgs;
		}
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


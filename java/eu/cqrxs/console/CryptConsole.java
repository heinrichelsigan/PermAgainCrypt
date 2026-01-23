/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.console;

import eu.cqrxs.console.OptEnum;
import eu.cqrxs.crypt.cipher.CipherEnum;
import eu.cqrxs.crypt.cipher.CipherPipe;
import eu.cqrxs.crypt.encoding.EncodeEnum;
import eu.cqrxs.crypt.hash.KeyHash;
import eu.cqrxs.util.Constants;
import eu.cqrxs.zip.ZipType;

import org.bouncycastle.crypto.InvalidCipherTextException;
import org.bouncycastle.crypto.*;

import java.io.IOException;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.HashMap;


/***
 *
 *  ConsoleMain app pipe for crypt/decrypt zip/unzip encode/decode md5sum/shaSum
 *  ConsoleMain
 *  -i | --inFile= | --inText={string|EnviromentVariable} | --inStd
 *  -o | --outFile= | --outText=EnviromentVariable | --outStd
 *  -z | --zip={gzip|bzip2|zip}
 *  -e | --encode={raw|hex16|hex32|base32|base64|uu}
 *  -C | --crypt={[aes,des3,blowfish,fish2,fish3]|key}
 *  -k | --key=mykey
 *  -H | --hash={Ascon256|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|Xoodyak}
 *  -S | --SymmCipher
 *  -D | --Decrypt 
 *  -? | --gethelp
 *
 */
public class CryptConsole  {
    static boolean useSymmCipher = false;
    final static String sepChar = java.nio.file.FileSystems.getDefault().getSeparator();
    static String progName = "";
    final static String dirPath = Path.of("").toAbsolutePath().toString();
    static String inName = null, outName = null, outEnviron = null, key = null;
    static boolean reverseDirection = false;
    static java.io.File inFile = null;
    static java.io.File outFile = null;
    static byte[] inBytes = null, outBytes = null;
    static String passKey = "";
    static ZipType zipType = ZipType.None;
    static EncodeEnum encodingType = EncodeEnum.None;
    static KeyHash keyHash = KeyHash.Hex;

    /**
     * Main entry method
     * @oaram args {@link String[]}
     */
    public static void main(String[] args) {
        try {
            progName = CryptConsole.class
                    .getProtectionDomain()
                    .getCodeSource()
                    .getLocation()
                    .toURI()
                    .getPath();
        } catch (Exception e) {
            e.printStackTrace();
            progName = "CryptConsole.java";
        }

        if (args.length < 1)
             usage("");
        encodingType = EncodeEnum.None;
        Constants.DirCreate = false;
        Constants.NOLog = true;
        OptEnum optEnum = OptEnum.Usage;
        HashMap<OptEnum, String> dict = new HashMap<OptEnum, String>();
        String[] algos = new String[0];
        String[] optArgs = new String[2];
        for (int i = 0; i < args.length; i++) {
            // string optStr = GetOption(... => out OptEnum optEnum)
            optArgs = getOptArg(args[i]);
            optEnum = OptEnum.getOptionFromString(optArgs[0]);
            String optStr = optArgs[1];
            // System.out.println("argv[" + i+ "] = " + args[i] + " " + optEnum.toString() + " option=" +  optStr);
            // Nothing todo on io params
            if (optEnum == OptEnum.OutP || optEnum == OptEnum.InParam) ;
            else if (optEnum == OptEnum.Help) // Help => usage()
                usage("");
            else if (optEnum == OptEnum.Usage) // usage with error message 
                usage(optStr); 
            // fetch passphrase or Key (decrypt key) from optEnum and optStr
            else if (optEnum == OptEnum.Key)
                passKey = optStr;
            else if (optEnum == OptEnum.SymmCipher) // prefetch SymmCipherMode 
                useSymmCipher = true; 
            else // otherwise add optEnum and optStr to Dictionary<OptEnum, string>(); 
                dict.put(optEnum, optStr);
        }
        // read from stdin, when no inName specified
        if (inName.isEmpty()) {
            System.out.println("Reading from stdin, enter \r\n^Z (Enter Strg - z Enter) to stop reading from stdin");
            byte[] buf = new byte[Constants.MAX_BYTE_BUFFEER];
            int buflen = 0;
            byte b;
            try {
                buf = System.in.readAllBytes();
                buflen = buf.length;
                // while (true) {
                //     b = (byte)System.in.read();
                //     if (b == -1)
                //         break;
                //     buf[buflen++] = (byte) b;
                // }
                System.arraycopy(buf, 0, inBytes, 0, buflen);
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        // iterate all option keys
        for (OptEnum optVar : dict.keySet()) {
            String optStr = dict.get(optVar);
            switch (optVar) {
                case OptEnum.Zip:
                    if (optStr.toLowerCase().contains("gz") ||
                            optStr.toLowerCase().contains("gunzip"))
                        zipType = ZipType.GZip;
                    else
                    if (optStr.toLowerCase().contains("bz") ||
                            optStr.toLowerCase().contains("bunzip") ||
                            optStr.toLowerCase().contains("2"))
                        zipType = ZipType.BZip2;
                    else
                    if (optStr.toLowerCase().contains("zip") ||
                            optStr.toLowerCase().contains("unzip"))
                        zipType = ZipType.Zip;
                    else
                        usage("unrecognized zip option: " + optStr);

                    break;
                case OptEnum.Encode:
                    encodingType = EncodeEnum.getEncodingTypeFromString(optStr);
                    System.out.println("optVar=" + optVar + " optStr=" + optStr + " encodingType = " + encodingType.toString());
                    break;
                case OptEnum.Hash:
                    keyHash = KeyHash.getKeyHashFromString(optStr);
                    break;
                case OptEnum.Crypt: 
                    // Usage on not existing or empty passphrase / key
                    if (passKey == null || passKey.isEmpty())
                        usage("unrecognized crypt option \"" + optStr + "\" without --pass=passPhrase ");

                    // when string / array is not null, fetch array for crypt pipe
                    if (!optStr.isEmpty()) {
                        optStr = optStr.replace("(", "").replace("{", "").replace("[", "").replace("]", "").replace("}", "").replace(")", "");
                        algos = optStr.split(",;:");
                    }
                    break;
                default: break;
            }
        }

        CipherPipe pipe;
        // Create cipher pipe for en-/decryption
        if (passKey == null || passKey.isEmpty() || algos.length > 0) {
            pipe =  new CipherPipe(algos, Constants.MAX_PIPE_LEN, encodingType, zipType, keyHash);
            System.out.println("Created pipe without passkey: " + pipe.getPipeString());
        } else {
            pipe = new CipherPipe(passKey, keyHash.hash(passKey), encodingType, zipType, keyHash);
            System.out.println("Created pipe with passkey=" + passKey + " pipe=" + pipe.getPipeString());
        }

        String outString = "";
        if (!reverseDirection) { // encrypt

            PrintPipe(pipe, reverseDirection);
            // CipherPipe encrypt encode
            try {
                passKey = (passKey.length() == 0) ? " " : passKey;
                outBytes = pipe.encryptEncodeBytes(inBytes,
                            passKey,  keyHash.hash(passKey), encodingType, zipType, keyHash);
            } catch (Exception exi) {
                exi.printStackTrace();
            }
            outString = new String(outBytes);
        } else { // decrypt

            String inString = new String(inBytes);
            PrintPipe(pipe, reverseDirection);
            // CipherPipe decode decrypt
            try {
                passKey = (passKey.length() == 0) ? " " : passKey;
                outBytes = pipe.decodeDecrpytBytes(inBytes,
                        passKey, keyHash.hash(passKey),
                        encodingType, zipType, keyHash);
            } catch (Exception exi) {
                exi.printStackTrace();
            }
        }

        inBytes = outBytes;

        if (outName != null && outName.length() > 0)
            System.out.println(outName.getBytes(Charset.forName("UTF-8")));
        else
        if (outFile != null) {
            try {
                Path fpath = outFile.toPath();
                Files.write(fpath, outBytes);
            } catch (Exception exIO) {
                exIO.printStackTrace();
            }
        }
        else
        if (outEnviron != null && outEnviron.length() > 0) {
            String os = System.getProperty("os.name").toLowerCase();
            Runtime rt = Runtime.getRuntime();
            try {
                if (os.indexOf("win") >= 0)
                    rt.exec("set " + outEnviron + "=" + new String(outBytes));
                    // else if (os.indexOf("mac") >= 0)
                    //     rt.exec("open " + url);
                else // if (os.indexOf("x") >=0 || os.indexOf("bsd") >= 0)
                    rt.exec(outEnviron + "="  + new String(outBytes) +
                            "; export $" + outEnviron.toString());
            } catch (Exception rtException) {
                rtException.printStackTrace();
            }
            // System.setenv(outEnviron, new String(outBytes));
        }

        return;
    }

    /**
     * PrintPipe prints out a CipherPipe
     * @param cpipe
     * @param decryptDirection
     */
    public static void PrintPipe(CipherPipe cpipe, boolean decryptDirection) {
        String pipeDirection = (decryptDirection) ? "OutPipe: " : " InPipe: ";
        CipherEnum[] ciphers = (decryptDirection) ? cpipe.getOutPipe() : cpipe.getInPipe();
        String prOut = "CipherPipe: " + pipeDirection +
                "\n\tKeyHash    \t= " + cpipe.getKeyHash() +
                "\n\tZipType    \t= " + cpipe.getZipType() +
                "\n\tEncodeEnum \t= " + cpipe.getEncodeType() +
                "\n\tPipeString \t= " + cpipe.getPipeString() +
                "\n\t";
        for (CipherEnum cipher : ciphers)
            prOut = prOut + cipher + "=>";
        System.out.print(prOut);
    }

    /***
     * getOptArg gets an option by argument
     * @param argument the argument
     * @return {@link String[]n}
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

        switch (arg.charAt(0)) {
            case 'I':
            case 'i':
                optArgs[0] = OptEnum.InParam.toString();
                inName = optArg;
                if (inName == null || inName.length() == 0)
                    ; // Else
                else
                    if (arg.toLowerCase().contains("file") || Files.exists(Paths.get(inName)) ||
                            Files.exists(Paths.get(dirPath + sepChar + inName))) {
                    if (Files.exists(Paths.get(dirPath + sepChar + inName))) {
                        inFile = new java.io.File(dirPath + sepChar + inName);
                        try {
                            inBytes = Files.readAllBytes(inFile.toPath());
                        } catch (Exception exx) {
                            exx.printStackTrace();
                        }
                    } else if (Files.exists(Paths.get(inName))) {
                        inFile = new java.io.File(inName);
                        try {
                            inBytes = Files.readAllBytes(inFile.toPath());
                        } catch (Exception exx) {
                            exx.printStackTrace();
                        }
                    }
                }
                else
                    if (arg.toLowerCase().contains("text") || inName.length() > 0) {
                        String inStr = System.getenv(inName.replace("$", "").replace("%", ""));
                    if (inStr == null || inStr.length() == 0)
                        inStr = inName;
                    inBytes = inStr.getBytes(Charset.forName("UTF-8"));
                }
                else
                    usage("unrecognized option: " + argument + ".");
                optArgs[1] = optArg;
                return optArgs;
            case 'O':
            case 'o':
                optArgs[0] =  OptEnum.OutP.toString();
                outName = optArg;
                if (outName == null || outName.length() == 0)
                    ; // to stdout
                else
                    if (arg.toLowerCase().contains("file") ||
                            optArg.contains(sepChar) ||
                            optArg.contains(".") ||
                            outName.length() > 0)
                        outFile = new java.io.File(outName);
                else
                    if (outName.length() > 0 || arg.toLowerCase().contains("text") ||
                        optArg.charAt(0) == '$' || optArg.charAt(0) == '%')
                    outEnviron = optArg;
                optArgs[1] = optArg;
                return optArgs;
            case 'Z':
            case 'z':
                optArgs[0] = OptEnum.Zip.toString();
                optArgs[1] = optArg;
                return optArgs;
            case 'E':
            case 'e':
                optArgs[0] = OptEnum.Encode.toString();
                optArgs[1] = optArg;
                return optArgs;
            case 'D':
            case 'd':
                reverseDirection = true;
                optArgs[0] = OptEnum.Decrypt.toString();
                optArgs[1] = optArg;
                return optArgs;
            case 'C':
            case 'c':
                optArgs[0] = OptEnum.Crypt.toString();
                optArgs[1] = optArg;
                return optArgs;
            case 'k':
            case 'K':
                optArgs[0] = OptEnum.Key.toString();
                optArgs[1] = optArg;
                return optArgs;
            case 'h':
            case 'H':
                optArgs[0] = OptEnum.Hash.toString();
                optArgs[1] = optArg;
                return optArgs;
            case 'S':
                optArgs[0] = OptEnum.SymmCipher.toString();
            optArgs[1] = optArg;
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



    /***
     * usage shows the usage of console application
     * @param errMsg error message
     */
    public static void usage(String errMsg) {
        if (errMsg != null && errMsg.length() > 0)
            System.err.println(errMsg);

        System.out.println("Usage:\t" + progName + " \n" +
                "\t-i | --inFile= | --inText={string|EnviromentVariable} | --inStd \n" +
                "\t-o | --outFile= | --outText=EnviromentVariable | --outStd \n" +
                "\t-z | --zip={gzip|bzip2}  \n" +
                "\t-e | --encode={raw|hex16|hex32|base32|base64|uu} \n" +
                "\t-C | --crypt={algo1,algo2,...} \n" +
                "\talgo: \n" +
                "\t\tAes,AesLight,Rijndael,Des,Des3,Dstu7624, \n" +
                "\t\tAria,Camellia,CamelliaLight,Cast5,Cast6, \n" +
                "\t\tBlowFish,Fish2,Fish3,ThreeFish256, \n" +
                "\t\tGost28147,Idea,Noekeon, \n" +
                "\t\tRC2,RC532,RC564,RC6, \n" +
                "\t\tSeed,SkipJack,Serpent,SM4, \n" +
                "\t\tTea,Tnepres,XTea, \n" +
                "\t\tZenMatrix,ZenMatrix2 \n" +
                "\tsymmAlgo: \n" +
                "\t\tAes,BlowFish,Camellia,Cast6,Des3,Fish2,Fish3,Gost28147,Idea,RC532,Seed,SkipJack,Serpent,Tea,XTea,SM4\n" +
                "\t-k | --key=passKey encrypt \n" +
                "\t-h | --hash={Ascon256|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|Xoodyak} \n" +
                "\t-D | --Decrypt \n" +
                "\t-S | --SymmCipher \n" +
                "\t-? | --gethelp\n");

        String uout = "Examples: \n" +
            "\tEU.CqrXs.Console.exe -i=README.MD -e=base16 -o=READ_MD.base16 \n" +
            "\tEU.CqrXs.Console.exe -D  -i=READ_MD.base16 -e=base16 -o=README_MD.txt \n" +
            "\tEU.CqrXs.Console.exe -i=README.MD -k=Hallo -z=gzip  -C=BlowFish,Fish2,Fish3 -e=base64 -o=README_MD.gz.BfF.base64 \n" +
            "\tEU.CqrXs.Console.exe -D -i=README_MD.gz.BfF.base64 -e=base64 -C=BlowFish,Fish2,Fish3 -p=Hallo -z=gzip -o=README_GUNZIP.txt \n" +
            "\tEU.CqrXs.Console.exe -i=README.MD -z=bz -k=heinrichelsigan.area23.at -H=Whirlpool -e=hex32 -o=README_MD.Whirlpool.bz.Hex32 \n" +
            "\tEU.CqrXs.Console.exe -D -i=README_MD.Whirlpool.bz.Hex32 -e=hex32 -k=heinrichelsigan.area23.at -H=Whirlpool -z=bz -o=README.BUNZIP.txt \n " +
            "\tEU.CqrXs.Console.exe -i=README.MD -z=zip -k=io.cqrxs.eu -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4 -H=SCrypt -e=uu -o=README_MD.SCrypt.zip.uu \n " +
            "\tEU.CqrXs.Console.exe -D -i=README_MD.SCrypt.zip.uu -e=uu -k=io.cqrxs.eu -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4 -H=SCrypt -z=zip -o=README_MD_UNZIP.txt \n" +
            "\tEU.CqrXs.Console.exe -i=README.MD -S -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=README_MD.BCrypt.zip.xx \n" +
            "\tEU.CqrXs.Console.exe -D -i=README_MD.BCrypt.zip.xx -S -e=xx -k=io.cqrxs.eu -H=BCrypt -z=zip -o=README_SYM_BCRYPT_UNZIP.txt \n";
        System.out.println(uout);

        System.exit(0);
    }

}



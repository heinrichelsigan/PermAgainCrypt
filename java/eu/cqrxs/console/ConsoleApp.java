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
import eu.cqrxs.crypt.cipher.CryptHelper;
import eu.cqrxs.crypt.cipher.CryptParams;
import eu.cqrxs.crypt.cipher.CipherEnum;
import eu.cqrxs.crypt.cipher.CipherPipe;
import eu.cqrxs.crypt.encoding.EncodeEnum;
import eu.cqrxs.crypt.hash.KeyHash;
import eu.cqrxs.util.Constants;
import eu.cqrxs.util.NotImplementedError;
import eu.cqrxs.util.CException;
import eu.cqrxs.util.DbgWriter;
import eu.cqrxs.zip.ZipType;
import eu.cqrxs.zip.GZ;

import java.io.File;

import java.io.IOException;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashMap;

import org.bouncycastle.crypto.*;
import org.bouncycastle.crypto.engines.*;
import org.bouncycastle.crypto.BlockCipher;


/***
 *
 *  ConsoleMain app pipe for crypt/decrypt zip/unzip encode/decode md5sum/shaSum
 *  ConsoleMain
 *  -i | --inFile= | --inText={string|EnviromentVariable} | --inStd
 *  -o | --outFile= | --outText=EnviromentVariable | --outStd
 *  -u | --unzip={gzip|bzip2|zip}
 *  -z | --zip={gzip|bzip2|zip}
 *  -d | --decode={raw|hex16|hex32|base32|base64|uu}
 *  -e | --encode={raw|hex16|hex32|base32|base64|uu}
 *  -C | --crypt={[aes,des3,blowfish,fish2,fish3]|key}
 *     |  -p --pass=Passphrase
 *  -D | --decrypt={[aes,des3,blowfish,fish2,fish3]|key}
 *     |  -p --pass=Passphrase
 *  -k | --key=mykey
 *  -q | --qey=myqey
 *  -H | --hash={Ascon256|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|Xoodyak}
 *  -S | --SymmCipher
 *  -? | --gethelp
 *
 */
public class ConsoleApp {
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
    static eu.cqrxs.zip.ZipType zipType = ZipType.None;
    static eu.cqrxs.crypt.encoding.EncodeEnum encodingType = EncodeEnum.None;
    static eu.cqrxs.crypt.hash.KeyHash keyHash = KeyHash.Hex;

    /**
     * Main entry method
     * @oaram args {@link String[]}
     */
    static void Main(String[] args) {
        try {
            progName = ConsoleApp.class
                    .getProtectionDomain()
                    .getCodeSource()
                    .getLocation()
                    .toURI()
                    .getPath();
        } catch (Exception e) {
            e.printStackTrace();
        }


        if (args.length <= 1)
            usage(null);
        encodingType = EncodeEnum.None;
        Constants.DirCreate = false;
        Constants.NOLog = true;
        OptEnum optEnum = OptEnum.Usage;
        String[] optArgs = new String[2];
        HashMap<OptEnum, String> dict = new HashMap<OptEnum, String>();
        String[] algos = new ArrayList<String>().toArray(new String[0]);

        for (int i = 0; i < args.length; i++) {
            // string optStr = GetOption(... => out OptEnum optEnum)
            optEnum = getOptArg(args[i], optArgs);
            String optStr = optArgs[0];
            // Nothing todo on io params
            if (optEnum == OptEnum.OutP || optEnum == OptEnum.InParam) ;
            else // Help => usage()
                if (optEnum == OptEnum.Help)
                    usage("");
                else // usage with error message
                    if (optEnum == OptEnum.Usage)
                        usage(optStr);
                    else // fetch passphrase or Key or Qey (decrypt key) from optEnum and optStr
                        if (optEnum == OptEnum.Pass || optEnum == OptEnum.Key || optEnum == OptEnum.Qey)
                            passKey = optStr;
                        else // prefetch SymmCipherMode
                            if (optEnum == OptEnum.SymmCipher)
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
                case OptEnum.Unzip:
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
                case OptEnum.Decode:
                    encodingType = EncodeEnum.getEncodingTypeFromString(optStr);
                    break;
                case OptEnum.Hash:
                    keyHash = KeyHash.getKeyHashFromString(optStr);
                    break;
                case OptEnum.Crypt:
                case OptEnum.Decrypt: // Usage on not existing or empty passphrase / key
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

        // Create cipher pipe for en-/decryption
        CipherPipe pipe = (algos.length > 0) ?
                new CipherPipe(algos, Constants.MAX_PIPE_LEN, encodingType, zipType, keyHash) :
                new CipherPipe(passKey, keyHash.hash(passKey), encodingType, zipType, keyHash);

        String outString = "";
        if (!reverseDirection) { // encrypt

            System.out.print("InPipe: ");
            for (CipherEnum cipher : pipe.getInPipe())
                System.out.print(cipher + "=>");

            // CipherPipe encrypt encode
            try {
                outString = pipe.encrpytEncode(inBytes, passKey, encodingType, zipType, keyHash);
            } catch (Exception exi) {
                exi.printStackTrace();
            }
            System.out.println("\r\nCipherPipe:" +
                    "\n\tKeyHash    \t= " + pipe.getKeyHash() +
                    "\n\tZipType    \t= " + pipe.getZipType() +
                    "\n\tEncodeEnum \t= " + pipe.getEncodeType() +
                    "\n\tPipeString \t= " + pipe.getPipeString());
            outBytes = outString.getBytes(Charset.forName("UTF-8"));

        } else { // decrypt

            String inString = new String(inBytes);
            System.out.print("OutPipe: ");
            for (CipherEnum cipher : pipe.getOutPipe())
                System.out.print(cipher + "=>");

            // CipherPipe decode decrypt
            try {
                outBytes = pipe.decodeDecrpyt(inString, passKey, encodingType, zipType, keyHash);
            } catch (Exception exi) {
                exi.printStackTrace();
            }

            System.out.println("\r\nCipherPipe:" +
                    "\n\tKeyHash    \t= " + pipe.getKeyHash() +
                    "\n\tZipType    \t= " + pipe.getZipType() +
                    "\n\tEncodeEnum \t= " + pipe.getEncodeType() +
                    "\n\tPipeString \t= " + pipe.getPipeString());
        }

        inBytes = outBytes;

        if (outName != null && !outName.isEmpty())
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
        if (outEnviron != null && !outEnviron.isEmpty()) {
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

    /***
     * getOptArg gets an option by argument
     * @param argument the argument
     * @param optArgs array of option arguments
     * @return {@link OptEnum}
     */
    public static OptEnum getOptArg(String argument, String[] optArgs) {
        if (optArgs == null || optArgs.length == 0)
            optArgs = new String[1];
        OptEnum optEnum = OptEnum.Usage;
        if (argument == null || argument.length() < 2 ||
                argument.charAt(0) != '-' || argument.charAt(0) != '/')   {
            optEnum = OptEnum.Usage;
            return optEnum;
        }
        optArgs[0] = argument;
        String arg = argument.substring(1);
        String optArg = optArgs[0];

        if (arg.contains("="))
            optArg = arg.substring(arg.indexOf('=' + 1));
        else if (arg.contains(":"))
            optArg =  arg.substring(arg.indexOf(':' + 1));

        switch (arg.charAt(0)) {
            case 'I':
            case 'i':
                optEnum = OptEnum.InParam;
                inName = optArg;
                if (inName.isEmpty())
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
            if (arg.toLowerCase().contains("text") || !inName.isEmpty()) {
                String inStr = System.getenv(inName.replace("$", "").replace("%", ""));
                if (inStr == null || inStr.length() == 0 || inStr.isEmpty())
                    inStr = inName;
                inBytes = inStr.getBytes(Charset.forName("UTF-8"));
            }
            else
                usage("unrecognized option: " + argument + ".");
            optArgs[0] = optArg;
            return optEnum;
            case 'O':
            case 'o':
                optEnum = OptEnum.OutP;
                outName = optArg;
                if (outName.isEmpty())
                    ; // to stdout
                else
                if (arg.toLowerCase().contains("file") ||
                        optArg.contains(sepChar) ||
                        optArg.contains(".") ||
                        !outName.isEmpty())
                    outFile = new java.io.File(outName);
                else
                if (!outName.isEmpty() || arg.toLowerCase().contains("text") ||
                        optArg.charAt(0) == '$' || optArg.charAt(0) == '%')
                    outEnviron = optArg;
                optArgs[0] = optArg;
                return optEnum;
            case 'Z':
            case 'z':
                optEnum = OptEnum.Zip;
                optArgs[0] = optArg;
                return optEnum;
            case 'U':
            case 'u':
                reverseDirection = true;
                optEnum = OptEnum.Unzip;
                optArgs[0] = optArg;
                return optEnum;
            case 'E':
            case 'e':
                optEnum = OptEnum.Encode;
                optArgs[0] = optArg;
                return optEnum;
            case 'd':
                reverseDirection = true;
                optEnum = OptEnum.Decode;
                optArgs[0] = optArg;
                return optEnum;
            case 'C':
            case 'c':
                optEnum = OptEnum.Crypt;
                optArgs[0] = optArg;
                return optEnum;
            case 'D':
                reverseDirection = true;
                optEnum = OptEnum.Decrypt;
                optArgs[0] = optArg;
                return optEnum;
            case 'k':
            case 'K':
                optEnum = OptEnum.Key;
                optArgs[0] = optArg;
                return optEnum;
            case 'p':
            case 'P':
                optEnum = OptEnum.Pass;
                optArgs[0] = optArg;
                return optEnum;
            case 'q':
            case 'Q':
                reverseDirection = true;
                optEnum = OptEnum.Qey;
                optArgs[0] = optArg;
                return optEnum;
            case 'h':
            case 'H':
                optEnum = OptEnum.Hash;
                optArgs[0] = optArg;
                return optEnum;
            case 'S':
                optEnum = OptEnum.SymmCipher;
                optArgs[0] = optArg;
                return optEnum;
            case 'g':
            case 'G':
            case '?':
            default:
                optEnum = OptEnum.Usage;
                optArg = "unrecognized option: " + argument + ".";
                optArgs[0] = optArg;
                return optEnum;
        }
    }



    /***
     * usage shows the usage of console application
     * @param errMsg error message
     */
    static void usage(String errMsg) {
        if (errMsg != null || !errMsg.isEmpty())
            System.out.println(errMsg);

        System.out.println("Usage:\t" + progName + "\n" +
                "\t-i | --inFile= | --inText={string|EnviromentVariable} | --inStd \n" +
                "\t-o | --outFile= | --outText=EnviromentVariable | --outStd \n" +
                "\t-u | --unzip={gzip|bzip2} \n" +
                "\t-z | --zip={gzip|bzip2}  \n" +
                "\t-d | --decode={raw|hex16|hex32|base32|base64|uu} \n" +
                "\t-e | --encode={raw|hex16|hex32|base32|base64|uu} \n" +
                "\t-c | --crypt={algo1,algo2,...} \n" +
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
                "\t\tAes,BlowFish,Camellia,Cast6,Des3,Fish2,Fish3,Gost28147,Idea,RC532,Seed,SkipJack,Serpent,Tea,XTea,SM4 \n" +
                "\t-p --pass=Passphrase \n" +
                "\t-D | --decrypt=={algo1,algo2,...} \n" +
                "\t\t-p --pass=Passphrase \n" +
                "\t-k | --key=passKey encrypt \n" +
                "\t-q | --qey=passKey decrypt \n" +
                "\t-h | --hash={Ascon256|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|Xoodyak} \n" +
                "\t-S | --SymmCipher \n" +
                "\t-? | --gethelp");

        System.out.println("\nExamples: \n" +
                "\t" + dirPath + " -i=test.jpg -z=bzip2 -e=base32 -o=test.jpg.bz2.base32 \n" +
                "\t" + dirPath + " -i=test.jpg.bz2.base32 -d=base32 -u=bzip2 -o=test1.jpg \n" +
                "\t" + dirPath + " --inFile=test.jpg --zip=gzip --crypt=AesLight,Fish3 -k=MySecretKey -e=base64 -o=test.jpg.gz.aeslight.fish3.base64 \n"+
                "\t" + dirPath + " -i=test.jpg.gz.aeslight.fish3.base64 -d=base64  -D=AesLight,Fish3 -k=MySecretKey -e=base64  --unzip=gzip  -o=test2.jpg \n" +
                "\t" + dirPath + " -i=README.MD -z=zip -k=io.cqrxs.eu -H=SCrypt -e=uu -o=README.MD.SCrypt.zip.uu \n" +
                "\t" + dirPath + " -i=README.MD.SCrypt.zip.uu -d=uu -q=io.cqrxs.eu -H=SCrypt -u=zip -o=README_UNZIP.txt");

        System.exit(0);
    }

}



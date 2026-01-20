using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.Cipher.Symmetric;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using System.Linq;
using System.Text;


namespace EU.CqrXs.SpoolTest
{

    /// <summary>
    /// OptEnum different option types
    /// </summary>
    public enum OptEnum
    {        
        Usage = 0x0,
        InParam = 0x1,
        Key = 0x2,
        Hash = 0x3,
        Zip = 0x4,
        CipherAlgos = 0x5,
        Encode = 0x6,
        OutP = 0x7,

        DeCrypt = 0x8, 
        SymmCipher = 0x9,
        
        YankeeBatchTest = 0xa,
        Verbose = 0xe,
        Help = 0xf
    }


    /// <summary>
    /// Console app pipe for crypt/decrypt zip/unzip encode/decode md5sum/shaSum
    /// 
    /// EU.CqrXs.Console.Program 
    /// -i | --inFile= | --inText={string|EnviromentVariable} | --inStd    
    /// -k | --key=mykey
    /// -H | --hash={Oct|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|TupleHash}
    /// -z | --zip={gzip|bzip2|zip}
    /// -C | --CipherAlgos={[aes,des3,blowfish,fish2,fish3]|key}
    /// -e | --encode={raw|hex16|hex32|base32|base64|uu}
    /// -o | --outFile= | --outText=EnviromentVariable | --outStd        
    /// 
    /// -D | --Decrypt 
    /// -S | --SymmCipher 
    /// 
    /// -Y | --YankeeTest
    /// -? | --gethelp
    /// </summary>
    internal class Program
    {
        const string BATCH_FILE_TEST = "Console_Test.bat";
        const string README_FILE = "README.MD";
        static bool useSymmCipher = false;
        static readonly string? progName = System.Environment.ProcessPath;
        static readonly string? progDirectory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        static string? inName = null, outName = null, outEnviron = null, key = null;
        static bool reverseDirection = false;
        static FileInfo? inFile = null, outFile = null;
        static byte[]? inBytes = null, outBytes = null;
        static string passKey = "";
        static ZipType zipType = ZipType.None;
        static EncodingType encodingType = EncodingType.None;
        static KeyHash keyHash = KeyHash.Hex;

        /// <summary>
        /// Console app pipe for crypt/decrypt zip/unzip encode/decode md5sum/shaSum
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length <= 0)
                Usage();
            encodingType = EncodingType.None;
            Constants.DirCreate = false;
            Constants.NOLog = true;

            Dictionary<OptEnum, string> dict = new Dictionary<OptEnum, string>();
            string[] algos = new List<string>().ToArray();

            GenConsoleAddítionalFiles();

            for (int i = 0; i < args.Length; i++)
            {
                // string optStr = GetOption(... => out OptEnum optEnum)
                string optStr = GetOption(args[i], out OptEnum optEnum);

                // Help => Usage("") Usage => Usage(optStr)
                if (optEnum == OptEnum.Help || optEnum == OptEnum.Usage)
                    Usage((optEnum == OptEnum.Help) ? "" : optStr);
                // Nothing todo on io params
                else if (optEnum == OptEnum.OutP || optEnum == OptEnum.InParam) ;
                // fetch  or Key or Qey (decrypt key) from optEnum and optStr
                else if (optEnum == OptEnum.Key) 
                    passKey = optStr;
                // prefetch decrypt reverse direction
                else if (optEnum == OptEnum.DeCrypt)
                    reverseDirection = true;
                // prefetch SymmCipherMode
                else if (optEnum == OptEnum.SymmCipher)
                    useSymmCipher = true;
                // otherwise add optEnum and optStr to Dictionary<OptEnum, string>();  
                else
                    dict.Add(optEnum, optStr);
            }
            // read from stdin, when no inName specified
            if (string.IsNullOrEmpty(inName))
            {
                System.Console.WriteLine("Reading from stdin, enter \r\n^Z (Enter Strg - z Enter) to stop reading from stdin");
                using (Stream stdin = System.Console.OpenStandardInput())
                {
                    List<byte> listBytes = new List<byte>();
                    byte[] buffer = new byte[2048];
                    int bytes;
                    while ((bytes = stdin.Read(buffer, 0, buffer.Length)) > 0)
                        listBytes.AddRange(buffer);

                    outBytes = EnDeCodeHelper.GetBytesTrimCrLfNulls(listBytes.ToArray());
                    inBytes = new byte[outBytes.Length];
                    Array.Copy(outBytes, 0, inBytes, 0, outBytes.Length);
                }
            }

            // iterate all option keys
            foreach (OptEnum optVar in dict.Keys)
            {
                string optStr = dict[optVar];
                switch (optVar)
                {
                    case OptEnum.Hash:
                        keyHash = KeyHash_Extensions.GetKeyHashFromString(optStr);
                        break;
                    case OptEnum.Zip:
                        if (optStr.ToLower().Contains("gz") || optStr.ToLower().Contains("gunzip"))
                            zipType = ZipType.GZip;
                        else
                            if (optStr.ToLower().Contains("bz") || optStr.ToLower().Contains("bunzip") || optStr.ToLower().Contains("2"))
                            zipType = ZipType.BZip2;
                        else
                            if (optStr.ToLower().Contains("zip") || optStr.ToLower().Contains("unzip"))
                            zipType = ZipType.Zip;
                        else
                            Usage("urecognized zip option: " + optStr);

                        break;
                    case OptEnum.CipherAlgos:
                        if (string.IsNullOrEmpty(passKey) || string.IsNullOrWhiteSpace(passKey))
                            Usage($"urecognized crypt option \"{optStr}\" without --pass=passPhrase ");

                        // when string / array is not null, fetch array for crypt pipe
                        if (!string.IsNullOrEmpty(optStr))
                        {
                            optStr = optStr.Replace("(", "").Replace("{", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace(")", "");
                            algos = optStr.Split(",;:".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (useSymmCipher)
                            {
                                List<string> algoList = new List<string>();
                                for (int ali = 0; ali < algos.Length; ali++)
                                {
                                    if (!Enum.TryParse<SymmCipherEnum>(algos[ali], out SymmCipherEnum symmCipherEnum))
                                        continue;

                                    foreach (SymmCipherEnum sci in SymmCipherEnumExtensions.GetSymmCipherTypes())
                                    {
                                        if (sci == symmCipherEnum)
                                        {
                                            algoList.Add(sci.ToString());
                                            break;
                                        }
                                    }

                                }
                                algos = algoList.ToArray();
                            }
                        }
                        break;
                    case OptEnum.Encode:
                        encodingType = EncodingTypesExtensions.GetEnum(optStr);
                        break;
                    
                    case OptEnum.YankeeBatchTest:
                        string batrun = Path.Combine(progDirectory, BATCH_FILE_TEST);
                        if (System.IO.File.Exists(batrun)) 
                            ProcessCmd.Execute(batrun, " ", false);
                        System.Environment.Exit(0);
                        break;
                    default: break;
                }
            }

            // Create cipher pipe for en-/decryption
            CipherPipe pipe = (algos.Length > 0 || string.IsNullOrEmpty(passKey)) ?
                            new CipherPipe(algos, Constants.MAX_PIPE_LEN, encodingType, zipType, keyHash) :
                            new CipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
            SymmCipherPipe symmPipe;
            if (useSymmCipher) { } // TODO: only ctor symmPipe in useSymmCipher
            symmPipe = (algos.Length > 0 || string.IsNullOrEmpty(passKey)) ?
                        new SymmCipherPipe(algos, 8, encodingType, zipType, keyHash) :
                        new SymmCipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);            
            // System.Console.WriteLine($"CipherPipe: KeyHash={pipe.KHash} ZipTyoe={pipe.ZType} EncodeType={pipe.EncodeType} PipeString={pipe.PipeString}");

            string outString = "";
            if (!reverseDirection)
            {
                System.Console.Write($" InPipe: ");
                if (useSymmCipher)
                {                    
                    PrintSymmCipherPipe(symmPipe, reverseDirection);                    
                    outString = symmPipe.EncrpytEncode(inBytes, passKey, encodingType, zipType, keyHash);
                }
                else
                {
                    PrintCipherPipe(pipe, reverseDirection);
                    outString = pipe.EncrpytEncode(inBytes, passKey, encodingType, zipType, keyHash);
                }

                // Encrypt process;
                outBytes = System.Text.Encoding.UTF8.GetBytes(outString);
            }
            else
            {
                // Decrypt process
                string inString = System.Text.Encoding.UTF8.GetString(inBytes);

                System.Console.Write($"OutPipe: ");
                if (useSymmCipher)
                {                    
                    PrintSymmCipherPipe(symmPipe, reverseDirection);
                    outBytes = symmPipe.DecodeDecrpyt(inString, passKey, encodingType, zipType, keyHash);
                }
                else
                {
                    PrintCipherPipe(pipe, reverseDirection);                    
                    outBytes = pipe.DecodeDecrpyt(inString, passKey, encodingType, zipType, keyHash);
                }
            }

            inBytes = outBytes;

            if (string.IsNullOrEmpty(outName))
                System.Console.WriteLine(Encoding.UTF8.GetString(outBytes));
            else
                if (outFile != null)
                File.WriteAllBytes(outFile.FullName, outBytes);
            else
                if (!string.IsNullOrEmpty(outEnviron))
                System.Environment.SetEnvironmentVariable(outEnviron, Encoding.UTF8.GetString(outBytes));


            return;
        }

        /// <summary>
        /// Gets an option by argument
        /// </summary>
        /// <param name="argument">cmd line argument</param>
        /// <param name="optEnum"><see cref="OptEnum">OptEnum cmd arg option enum</see></param>
        /// <returns></returns>
        public static string GetOption(string argument, out OptEnum optEnum)
        {
            string optArg = "";
            if (string.IsNullOrEmpty(argument) || argument.Length < 2 || argument[0] != '-')
            {
                optEnum = OptEnum.Usage;
                return optArg;
            }
            optArg = argument;
            string arg = argument.TrimStart("-/".ToCharArray());

            if (arg.Contains("="))
                optArg = arg.GetSubStringByPattern("=", true, "", " ", true, StringComparison.CurrentCultureIgnoreCase);
            // else if (arg.Contains(":"))
            //     optArg = arg.GetSubStringByPattern(":", true, "", " ", true, StringComparison.CurrentCultureIgnoreCase);

            switch (arg[0])
            {
                case 'I':
                case 'i':
                    optEnum = OptEnum.InParam;
                    inName = optArg;
                    if (string.IsNullOrEmpty(inName))
                        ; // Else
                    else
                        if (arg.ToLower().Contains("file") || File.Exists(inName) || File.Exists(Path.Combine(progDirectory, inName)))
                    {
                        if (System.IO.File.Exists(Path.Combine(progDirectory, inName)))
                        {
                            inFile = new FileInfo(Path.Combine(progDirectory, inName));
                            inBytes = File.ReadAllBytes(Path.Combine(progDirectory, inName));
                        }
                        else if (File.Exists(inName))
                        {
                            inFile = new FileInfo(inName);
                            inBytes = File.ReadAllBytes(inName);
                        }
                    }
                    else
                        if (arg.ToLower().Contains("text") || !string.IsNullOrEmpty(inName))
                    {
                        string? inStr = Environment.GetEnvironmentVariable(inName.TrimStart("$".ToCharArray()));
                        if (inStr == null || inStr.Length == 0)
                            inStr = inName;
                        inBytes = Encoding.UTF8.GetBytes(inStr);
                    }
                    else
                        Usage($"unrecognized option: {argument}.");

                    return optArg;
                case 'O':
                case 'o':
                    optEnum = OptEnum.OutP;
                    outName = optArg;
                    if (string.IsNullOrEmpty(outName))
                        ; // to stdout                    
                    else
                        if (arg.ToLower().Contains("file") || optArg.Contains(LibPaths.SepChar) || optArg.Contains('.') || !string.IsNullOrEmpty(outName))
                        outFile = new FileInfo(outName);
                    else
                        if (!string.IsNullOrEmpty(outName) || arg.ToLower().Contains("text") || optArg.StartsWith("$"))
                        outEnviron = optArg;

                    return optArg;
                case 'Z':
                case 'z':
                    optEnum = OptEnum.Zip;
                    return optArg;                
                case 'E':
                case 'e':
                    optEnum = OptEnum.Encode;
                    return optArg;
                case 'C':
                case 'c':
                    optEnum = OptEnum.CipherAlgos;
                    return optArg;                
                case 'k':
                case 'K':
                    optEnum = OptEnum.Key;
                    return optArg;              
                case 'h':
                case 'H':
                    optEnum = OptEnum.Hash;
                    return optArg;
                case 'S':
                    optEnum = OptEnum.SymmCipher;
                    return optArg;
                case 'D':
                    optEnum = OptEnum.DeCrypt;
                    return optArg;
                case 'y':
                case 'Y':
                    optEnum = OptEnum.YankeeBatchTest;
                    return optArg;
                case 'g':
                case 'G':
                case '?':
                default:
                    optEnum = OptEnum.Usage;
                    optArg = $"unrecognized option: {argument}.";
                    return optArg;
            }
        }

        /// <summary>
        /// generates additional needed files for Crypt Console
        /// </summary>
        public static void GenConsoleAddítionalFiles()
        {
            if (!File.Exists(Path.Combine(progDirectory, README_FILE)))
                File.WriteAllText(Path.Combine(progDirectory, README_FILE), readmeMD);
            if (!File.Exists(Path.Combine(progDirectory, BATCH_FILE_TEST)))
                File.WriteAllText(Path.Combine(progDirectory, BATCH_FILE_TEST), console_test);

        }

        /// <summary>
        /// Usage shows the usage of console application
        /// </summary>
        static void Usage(string errMsg = "")
        {
            if (!string.IsNullOrEmpty(errMsg))
                System.Console.Error.WriteLine(errMsg);

            System.Console.Out.WriteLine("Usage:\t" + Path.GetFileName(progName) + @"
    -i  | --inFile= | --inText={string|EnviromentVariable} | --inStd    
        |
    -k  | --key=passKey encrypt    
    -H  | --Hash={Oct|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|TupleHash}        
    -z  | --zip={gzip|bzip2|zip}
    -C  | --CipherAlgost={algo1,algo2,...}
         algo:
            Aes,AesLight,Rijndael,Des,Des3,Dstu7624,
            Aria,Camellia,CamelliaLight,Cast5,Cast6,
            BlowFish,Fish2,Fish3,
            Gost28147,Idea,Noekeon,
            RC2,RC532,RC564,RC6,
            Seed,SkipJack,Serpent,SM4,
            Tea,Tnepres,XTea,
            ZenMatrix,ZenMatrix2
        symmAlgo: 
            Aes,BlowFish,Camellia,Cast6,Des3,Fish2,Fish3,Gost28147,Idea,RC532,Seed,SkipJack,Serpent,Tea,XTea,SM4        
    -S  | --SymmCipher 
    -e  | --encode={raw|hex16|hex32|base32|base64|uu}
    -D  | --Decrypt=Inverse_Pipe_Direction
        |
    -o  | --outFile= | --outText=EnviromentVariable | --outStd            
        |
    -Y  | --YankeeTest
    -?  | --gethelp");

            System.Console.Out.WriteLine($"\nExamples: " + @"

    EU.CqrXs.Console.exe -i=.\README.MD -e=base16 -o=.\README_MD.base16
    EU.CqrXs.Console.exe -D  -i=.\README_MD.base16 -e=base16 -o=.\READ_MD.txt

    EU.CqrXs.Console.exe -i=.\README.MD -k=Hallo -z=gzip  -C=BlowFish,Fish2,Fish3 -e=base64 -o=.\README.MD.gz.BfF.base64
    EU.CqrXs.Console.exe -D -i=.\README.MD.gz.BfF.base64 -e=base64 -C=BlowFish,Fish2,Fish3 -p=Hallo -z=gzip -o=.\READ_GUNZIP.txt

    EU.CqrXs.Console.exe -i=.\README.MD -z=bz -k=heinrichelsigan.area23.at -H=Whirlpool -e=hex32 -o=.\README.MD.Whirlpool.bz.Hex32
    EU.CqrXs.Console.exe -D -i=.\README.MD.Whirlpool.bz.Hex32 -e=hex32 -k=heinrichelsigan.area23.at -H=Whirlpool -z=bz -o=.\READ_BUNZIP.txt

    REM EU.CqrXs.Console.exe -i=.\README.MD -z=zip -k=io.cqrxs.eu -H=SCrypt -e=uu -o=.\README.MD.SCrypt.zip.uu
    REM EU.CqrXs.Console.exe -D -i=.\README.MD.SCrypt.zip.uu -e=uu -k=io.cqrxs.eu -H=SCrypt -z=zip -o=.\READ_UNZIP.txt
    EU.CqrXs.Console.exe -i=.\README.MD -z=zip -k=io.cqrxs.eu -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4 -H=SCrypt -e=uu -o=.\README.MD.SCrypt.zip.uu
    EU.CqrXs.Console.exe -D -i=.\README.MD.SCrypt.zip.uu -e=uu -k=io.cqrxs.eu -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4 -H=SCrypt -z=zip -o=.\READ_UNZIP.txt

    EU.CqrXs.Console.exe -i=.\README.MD -S -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\README.MD.BCrypt.zip.xx
    EU.CqrXs.Console.exe -D -i=.\README.MD.BCrypt.zip.xx -S -e=xx -k=io.cqrxs.eu -H=BCrypt -z=zip -o=.\README_SYM_BCRYPT_UNZIP.txt\n\n");

            System.Environment.Exit(0);
        }

        #region print only debug info
        public static void PrintSymmCipherPipe(SymmCipherPipe symmPipe, bool outPipe = false)
        {
            SymmCipherEnum[] symmCiphers = (outPipe) ? symmPipe.OutPipe : symmPipe.InPipe;
            System.Console.Write((string)((outPipe) ? "Out:\t" : " In:\t"));
            foreach (var symmCipher in symmCiphers)
                System.Console.Write($"{symmCipher}=>");

            System.Console.WriteLine($"\r\nSymmCipherPipe: KeyHash={symmPipe.KHash} ZipType={symmPipe.ZType} " +
                $"EncodeType={symmPipe.EncodeType} PipeString={symmPipe.PipeString}");

        }

        public static void PrintCipherPipe(CipherPipe cipherPipe, bool outPipe = false)
        {
            CipherEnum[] ciphers = (outPipe) ? cipherPipe.OutPipe : cipherPipe.InPipe;
            System.Console.Write((string)((outPipe) ? "Out:\t" : " In:\t"));
            foreach (CipherEnum cipher in ciphers)
                System.Console.Write($"{cipher}=>");
            System.Console.WriteLine($"\r\nCipherPipe: KeyHash={cipherPipe.KHash} ZipType={cipherPipe.ZType} " +
                $"EncodeType={cipherPipe.EncodeType} PipeString={cipherPipe.PipeString}");
        }
        #endregion print only debug info

        #region static readonly strings

        static readonly string console_test = @"@echo off

echo Staring console EU.CqrXs.Console.exe tests
echo deleting README_MD.base16 README.MD.gz.BfF.base64 README.MD.Whirlpool.bz.Base32 README.MD.SCrypt.zip.uu README.MD.BCrypt.zip.xx READ_MD.txt READ_GUNZIP.txt READ_UNZIP.txt READ_BUNZIP.txt README_SYM_BCRYPT_UNZIP.txt

del /q README_MD.base16 README.MD.gz.BfF.base64 README.MD.Whirlpool.bz.Base32 README.MD.SCrypt.zip.uu README.MD.BCrypt.zip.xx READ_MD.txt READ_GUNZIP.txt READ_UNZIP.txt READ_BUNZIP.txt README_SYM_BCRYPT_UNZIP.txt
@echo on

EU.CqrXs.Console.exe -i=.\README.MD -e=base16 -o=.\README_MD.base16
EU.CqrXs.Console.exe -D  -i=.\README_MD.base16 -e=base16 -o=.\READ_MD.txt

EU.CqrXs.Console.exe -i=.\README.MD -k=Hallo -z=gzip  -C=BlowFish,Fish2,Fish3 -e=base64 -o=.\README.MD.gz.BfF.base64
EU.CqrXs.Console.exe -D -i=.\README.MD.gz.BfF.base64 -k=Hallo -e=base64 -C=BlowFish,Fish2,Fish3 -z=gzip -o=.\READ_GUNZIP.txt

EU.CqrXs.Console.exe -i=.\README.MD -z=bz -k=heinrichelsigan.area23.at -H=Whirlpool -e=hex32 -o=.\README.MD.Whirlpool.bz.Hex32
EU.CqrXs.Console.exe -D -i=.\README.MD.Whirlpool.bz.Hex32 -e=hex32 -k=heinrichelsigan.area23.at -H=Whirlpool -z=bz -o=.\READ_BUNZIP.txt

EU.CqrXs.Console.exe -i=.\README.MD  -k=io.cqrxs.eu -H=SCrypt -z=zip -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4  -e=uu -o=.\README.MD.SCrypt.zip.uu
EU.CqrXs.Console.exe -D -i=.\README.MD.SCrypt.zip.uu -e=uu -k=io.cqrxs.eu -H=SCrypt -z=zip -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4  -o=.\READ_UNZIP.txt

EU.CqrXs.Console.exe -i=.\README.MD -S -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\README.MD.BCrypt.zip.xx
EU.CqrXs.Console.exe -D -i=.\README.MD.BCrypt.zip.xx -S -e=xx -k=io.cqrxs.eu -H=BCrypt -z=zip -o=.\README_SYM_BCRYPT_UNZIP.txt

start notepad READ_MD.txt
start notepad READ_GUNZIP.txt
start notepad READ_BUNZIP.txt
start notepad READ_UNZIP.txt
start notepad README_SYM_BCRYPT_UNZIP.txt

echo finished, waiting 30 seconds to close
timeout 30 > NUL
REM pause\n\n";

        static readonly string readmeMD = @"https://cqrxs.eu/download/EU.CqrXs.Console/README.MD

EU.CqrXs.Console.exe -?
Usage:  EU.CqrXs.Console.exe
        -i  | --inFile= | --inText={string|EnviromentVariable} | --inStd    
        -k  | --key=mykey
        -H  | --hash={Oct|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|TupleHash}
        -z  | --zip={gzip|bzip2|zip}
        -C  | --CipherAlgos={[aes,des3,blowfish,fish2,fish3]|key}
        algo:
            Aes,AesLight,Rijndael,Des,Des3,Dstu7624,
            Aria,Camellia,CamelliaLight,Cast5,Cast6,
            BlowFish,Fish2,Fish3,
            Gost28147,Idea,Noekeon,
            RC2,RC532,RC564,RC6,
            Seed,SkipJack,Serpent,SM4,
            Tea,Tnepres,XTea,
            ZenMatrix,ZenMatrix2
        symmAlgo: 
            Aes,BlowFish,Camellia,Cast6,Des3,Fish2,Fish3,Gost28147,Idea,RC532,Seed,SkipJack,Serpent,Tea,XTea,SM4        
        -S  | --SymmCipher   
        -e  | --encode={raw|hex16|hex32|base32|base64|uu}
        -o  | --outFile= | --outText=EnviromentVariable | --outStd            
        -D  | --Decrypt           
        -Y  | --YankeeTest
        -?  | --gethelp

Examples:
    
    EU.CqrXs.Console.exe -i=.\README.MD -e=base16 -o=.\README_MD.base16
    EU.CqrXs.Console.exe -D  -i=.\README_MD.base16 -e=base16 -o=.\READ_MD.txt

    EU.CqrXs.Console.exe -i=.\README.MD -k=Hallo -z=gzip  -C=BlowFish,Fish2,Fish3 -e=base64 -o=.\README.MD.gz.BfF.base64
    EU.CqrXs.Console.exe -D -i=.\README.MD.gz.BfF.base64 -k=Hallo -e=base64 -C=BlowFish,Fish2,Fish3 -z=gzip -o=.\READ_GUNZIP.txt

    EU.CqrXs.Console.exe -i=.\README.MD -z=bz -k=heinrichelsigan.area23.at -H=Whirlpool -e=hex32 -o=.\README.MD.Whirlpool.bz.Hex32
    EU.CqrXs.Console.exe -D -i=.\README.MD.Whirlpool.bz.Hex32 -e=hex32 -k=heinrichelsigan.area23.at -H=Whirlpool -z=bz -o=.\READ_BUNZIP.txt

    EU.CqrXs.Console.exe -i=.\README.MD  -k=io.cqrxs.eu -H=SCrypt -z=zip -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4  -e=uu -o=.\README.MD.SCrypt.zip.uu
    EU.CqrXs.Console.exe -D -i=.\README.MD.SCrypt.zip.uu -e=uu -k=io.cqrxs.eu -H=SCrypt -z=zip -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4  -o=.\READ_UNZIP.txt

    EU.CqrXs.Console.exe -i=.\README.MD -S -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\README.MD.BCrypt.zip.xx
    EU.CqrXs.Console.exe -D -i=.\README.MD.BCrypt.zip.xx -S -e=xx -k=io.cqrxs.eu -H=BCrypt -z=zip -o=.\README_SYM_BCRYPT_UNZIP.txt\n\n";

        #endregion static readonly strings

    }

}

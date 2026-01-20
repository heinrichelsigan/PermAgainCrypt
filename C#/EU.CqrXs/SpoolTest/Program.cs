using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.Cipher.Symmetric;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;


namespace EU.CqrXs.SpoolTest
{

    /// <summary>
    /// OptEnum different option types
    /// </summary>
    public enum OptEnum
    {
        Usage = 0x0,
        InDir = 0x1,
        OutDir = 0x2,
        Decrypt = 0x3,
        KeyFile = 0x4,
        Symmetric = 0x5,
        YankeeTest = 0x6,
        GetHelp = 0x7
    }



    /// <summary>
    /// Console app for spooling large amount of
    /// 1. plain files and encrypt them into another directory
    /// 2. encrypted files and decrypt them into another directory
    /// 
    /// EU.CqrXs.Console.Program 
    /// -i | --InDir={path to incoming dir} 
    /// -o | --OutDir={path to outcoming dir}
    /// -k | --KeyFile={file with 10000 of keys}     
    /// -D | --Decrypt 
    /// -S | --SymmCipher // use symmetric chipher only to encrypt 
    /// -Y | --YankeeTest // this is a yankee test
    /// -? | --gethelp
    /// </summary>
    internal class Program
    {
        const string BATCH_FILE_TEST = "Spooler_Test.bat";
        const string README_FILE = "README.MD";
        static readonly string? progName = System.Environment.ProcessPath;
        static readonly string? progDirectory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        // generic spooler variables
        static bool useSymmCipher = false, decryptDirection = false;
        static string inDir = "", outDir = "", keyFile = "";
        static string[] keys = new string[0], files = new string[0];

        // specific encrypt/decrypt process variables
        static string? inName = null, outName = null, outEnviron = null, key = null;
        static FileInfo? inFile = null, outFile = null;
        static byte[]? inBytes = null, outBytes = null;
        static string passKey = "";
        static readonly ZipType[] ZipTypes = ZipTypeExtensions.ZipTypes;
        static ZipType zipType = ZipType.None;
        static readonly EncodingType[] AsciiEncoders = EncodingTypesExtensions.GetEncodingTypes();
        static EncodingType encodingType = EncodingType.None;
        static readonly KeyHash[] KeyHashes = KeyHash_Extensions.GetHashes();
        static KeyHash keyHash = KeyHash.Hex;

        /// <summary>
        /// Console spooler app for en-/decrypting a huge amount of files
        /// </summary>
        /// <param name="args">command line arguments</param>
        static void Main(string[] args)
        {
            if (args.Length <= 0)
                Usage();

            encodingType = EncodingType.None;
            Constants.DirCreate = false;
            Constants.NOLog = true;

            Dictionary<OptEnum, string> dict = new Dictionary<OptEnum, string>();
            string[] algos = new List<string>().ToArray();

            for (int i = 0; i < args.Length; i++)
            {
                // string optStr = GetOption(... => out OptEnum optEnum)
                string optStr = GetOption(args[i], out OptEnum optEnum);
            }
            if (string.IsNullOrEmpty(outDir) || !Directory.Exists(outDir))
            {
                outDir = Path.Combine(LibPaths.TempDir, "spool_out");
                try
                {
                    if (!Directory.Exists(outDir))
                        Directory.CreateDirectory(outDir);
                }
                catch (Exception exOutDir)
                {
                    Usage($"{exOutDir.GetType()}: {exOutDir.Message}\n{exOutDir.StackTrace}\n");
                }
            }

            files = Directory.GetFiles(inDir);
            foreach (string file in files)
            {

            }

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
            if (string.IsNullOrEmpty(argument) || argument.Length < 2 || argument[0] != '-' || argument[0] != '/')
            {
                optEnum = OptEnum.Usage;
                return optArg;
            }
            optArg = argument;
            string arg = argument.TrimStart("-/".ToCharArray());

            if (arg.Contains("="))
                optArg = arg.GetSubStringByPattern("=", true, "", " ", true, StringComparison.CurrentCultureIgnoreCase);

            switch (arg[0])
            {
                case 'I':
                case 'i':
                    optEnum = OptEnum.InDir;
                    inDir = optArg;
                    if (string.IsNullOrEmpty(inName))
                        Usage($"{progName}: --InDir needs not null or empty parameter for incoming directory.");
                    else if (!Directory.Exists(inDir))
                    {
                        if (Constants.DirCreate)
                            Directory.CreateDirectory(inDir);
                        else
                            Usage($"{progName}: InDir=${inDir} doesn't exist.");
                    }
                    return optArg;

                case 'O':
                case 'o':
                    optEnum = OptEnum.OutDir;
                    outDir = optArg;
                    if (string.IsNullOrEmpty(outDir))
                        Usage($"{progName}: --OutDir needs not null or empty parameter for outgoing directory.");                    
                    else if (!Directory.Exists(outDir))
                    {
                        if (Constants.DirCreate)
                            Directory.CreateDirectory(outDir);
                        else
                            Usage($"{progName}: OutDir=${outDir} doesn't exist.");
                    }                    
                    return optArg;

                case 'D':
                case 'd':
                    optEnum = OptEnum.Decrypt;
                    decryptDirection = true;
                    return optArg;

                case 'k':
                case 'K':
                    optEnum = OptEnum.KeyFile;
                    keyFile = optArg;
                    if (string.IsNullOrEmpty(keyFile) || !File.Exists(keyFile)) 
                    {
                        string warn = string.IsNullOrEmpty(keyFile) ? "(NULL)" : keyFile;
                        Usage($"{progName}: KeyFile={warn} doesn't exist.");
                    }
                    keys = File.ReadAllLines(keyFile, Encoding.UTF8);
                    return optArg;      
                    
                case 'S':
                    optEnum = OptEnum.Symmetric;
                    Program.useSymmCipher = true;
                    return optArg;  
                    
                case 'y':
                case 'Y':
                    optEnum = OptEnum.YankeeTest;
                    return optArg;

                case 'g':
                case 'G':
                case '?':
                default:
                    optEnum = OptEnum.Usage;
                    optArg = $"unrecognized option: {argument}.";
                    Usage(optArg);
                    break;
            }

            return optArg;
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

    }

}

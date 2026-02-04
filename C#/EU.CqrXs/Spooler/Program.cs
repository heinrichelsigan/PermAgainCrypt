using EU.CqrXs.Crypt;
using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.Cipher.Symmetric;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using System.Text;
using System.Xml;


namespace EU.CqrXs.Spooler
{

    /// <summary>
    /// OptEnum different option types
    /// </summary>
    public enum OptSpoolEnum
    {
        Usage = 0x0,
        InDir = 0x1,
        OutDir = 0x2,
        Decrypt = 0x3,
        Key = 0x4,
        Symmetric = 0x5,
        Mode = 0x6,
        Verbose = 0xe,
        GetHelp = 0xf
    }



    /// <summary>
    /// Console app for spooling large amount of
    /// 1. plain files and encrypt them into another directory
    /// 2. encrypted files and decrypt them into another directory
    /// 
    /// EU.CqrXs.Console.Program 
    /// -i | --InDir={path to incoming dir} 
    /// -o | --OutDir={path to outcoming dir}
    /// -k | --Key={users key}   
    /// -D | --Decrypt 
    /// -M | --mode={CBC|CFB|ECB}   
    /// -S | --SymmCipher // use symmetric chipher only to encrypt
    /// -V | --verbose
    /// -? | --gethelp
    /// </summary>
    internal class Program
    {
        const string BATCH_FILE_TEST = "Spooler_Test.bat";
        const string README_FILE = "README.MD";
        static readonly string? progName = System.Environment.ProcessPath;
        static readonly string? progDirectory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        static readonly string progFilename = (!string.IsNullOrEmpty(progName)) ? Path.GetFileName(progName) : "EU.CqrXs.SpoolTest.exe";
        static readonly string UsageString = $"Usage:\t {progFilename} " + @"
    /// -i | --InDir={path to incoming dir} 
    /// -o | --OutDir={path to outcoming dir}
    /// -k | --Key=secretKey
    /// -D | --Decrypt 
    /// -M | --mode={CBC|CFB|ECB}
    /// -S | --SymmCipher   // use symmetric chipher only to encrypt 
    /// -V | --verbose      // verbose output
    /// -? | --gethelp\n";
        // generic spooler variables
        static bool useSymmCipher = false, decryptDirection = false, verbose = false;
        static string inDir = "", outDir = "", keyFile = "";
        static string[] keys = new string[0], files = new string[0];

        // specific encrypt/decrypt process variables
        static string? outEnviron = null, key = null;
        static FileInfo? inFile = null, outFile = null;
        static byte[]? inBytes = null, outBytes = null;
        static string passKey = "";
        static readonly ZipType[] ZipTypes = { ZipType.None, ZipType.GZip, ZipType.BZip2, ZipType.Zip };
        static ZipType zipType = ZipType.None;
        static readonly EncodingType[] AsciiEncoders = { EncodingType.Hex16, EncodingType.Base16, EncodingType.Base32, 
                                                EncodingType.Uu, EncodingType.Xx, EncodingType.Hex64, EncodingType.Base64 };
        static EncodingType encodingType = EncodingType.None;
        static readonly KeyHash[] KeyHashes = { KeyHash.BCrypt, KeyHash.Blake2xs, KeyHash.CShake, KeyHash.Dstu7564,
                                                KeyHash.Hex, KeyHash.MD5, KeyHash.Oct, KeyHash.OpenBSDCrypt, 
                                                KeyHash.RipeMD256,KeyHash.Sha1, KeyHash.Sha256, KeyHash.Sha384, KeyHash.Sha512,
                                                KeyHash.SCrypt, KeyHash.TupleHash, KeyHash.Whirlpool };
        static KeyHash keyHash = KeyHash.Hex;
        static CipherMode2 cmode2 = CipherMode2.ECB;

        /// <summary>
        /// Console spooler app for en-/decrypting a huge amount of files
        /// </summary>
        /// <param name="args">command line arguments</param>
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
                Usage();

            CException decryptExc = null, encryptExc = null;
            bool keyFromArg = true;
            long filesCount = 0;
            encodingType = EncodingType.None;
            Constants.DirCreate = false;
            Constants.NOLog = true;

            Dictionary<OptSpoolEnum, string> dict = new Dictionary<OptSpoolEnum, string>();
            string[] algos = new List<string>().ToArray();

            for (int i = 0; i < args.Length; i++)
            {
                // string optStr = GetOption(... => out OptEnum optEnum)
                string optStr = GetOption(args[i], out OptSpoolEnum optEnum);
            }            

            int zc = 0;
            int hc = 0;
            int ec = 0;
            int kc = 0;

            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                System.Console.WriteLine("Reading key from stdin");
                using (Stream stdin = System.Console.OpenStandardInput())
                {
                    byte[] buffer = new byte[1024];
                    int bytes;
                    bytes = stdin.Read(buffer, 0, buffer.Length);                   
                    byte[] outBuffer = EnDeCodeHelper.GetBytesTrimCrLfNulls(buffer);
                    if (outBuffer.Length > 0)
                        key = System.Text.Encoding.UTF8.GetString(outBuffer);
                    else
                        Usage("key string is null or empty.");
                    keyFromArg = false;
                }                
            }
            if (verbose)
                System.Console.WriteLine($"Key read from {(keyFromArg ?"argument":"stdin")}: '{key}'");

            files = Directory.GetFiles(inDir);
            byte[] outBytes = new byte[0];
            foreach (string file in files)
            {
                passKey = key; // keys[((kc++) % KeyHashes.Length)];
                keyHash = KeyHashes[((++hc) % KeyHashes.Length)];
                encodingType = AsciiEncoders[((++ec) % AsciiEncoders.Length)];
                zipType = ZipTypes[((++zc) % ZipTypes.Length)];

                byte[] inByte = File.ReadAllBytes(file);
                string ofName = Path.GetFileName(file);
                if (verbose)
                    Console.WriteLine(DateTime.Now.Area23DateTimeWithSeconds().ToString() + 
                        " reading " + inByte.Length + " bytes from file " + ofName);

                CipherPipe cPipe;
                SymmCipherPipe symmPipe;
                if (!decryptDirection) // encrypting
                {
                    if (useSymmCipher) // SymmCipherPipe and SymmCipherEnum only
                    {
                        symmPipe = new SymmCipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, cmode2);
                        PrintSymmCipherPipe(symmPipe, decryptDirection);
                        outBytes = symmPipe.EncryptEncodeBytes(inByte, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, cmode2);
                        ofName += symmPipe.PipeFullExtension;
                    }
                    else // CipherPipe and all CipherEnum's
                    {
                        cPipe = new CipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, cmode2);
                        PrintCipherPipe(cPipe, decryptDirection);
                        outBytes = cPipe.EncryptEncodeBytes(inByte, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, cmode2);
                        ofName += cPipe.PipeFullExtension;
                    }
                }
                else if (file.IsPermAgainCryptFile()) // decrypting
                {
                    keyHash = KeyHash.Hex;
                    zipType = ZipType.None;
                    if (useSymmCipher)
                    {
                        ofName = ofName.StripSymmCipherPipeFromFileName(out symmPipe);
                        if (symmPipe != null)
                        {
                            keyHash = symmPipe.KHash;
                            encodingType = symmPipe.EncodeType;
                            zipType = symmPipe.ZType;
                        }
                        PrintSymmCipherPipe(symmPipe, decryptDirection);
                        try
                        {
                            outBytes = symmPipe.DecodeDecrpytBytes(inByte, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, cmode2);
                        } 
                        catch (Exception exSymmDecrypt)
                        {
                            outBytes = new byte[0];
                            decryptExc = new CException($"{exSymmDecrypt.GetType()} was thrown decrypting {ofName} with {symmPipe.PipeFullExtension}", exSymmDecrypt);
                            if (verbose)
                                System.Console.Error.WriteLine($"{exSymmDecrypt.GetType()}: {exSymmDecrypt.Message}\n\t{exSymmDecrypt.ToString()}");                            
                        }
                    }
                    else
                    {
                        ofName = ofName.StripCipherPipeFromFileName(out cPipe);
                        if (cPipe != null)
                        {
                            keyHash = cPipe.KHash;
                            encodingType = cPipe.EncodeType;
                            zipType = cPipe.ZType;
                        }
                        PrintCipherPipe(cPipe, decryptDirection);
                        try
                        {
                            outBytes = cPipe.DecodeDecrpytBytes(inByte, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, cmode2);
                        } 
                        catch (Exception exDecrypt)
                        {
                            decryptExc = new CException($"{exDecrypt.GetType()} was thrown decrypting {ofName} with {cPipe.PipeFullExtension}", exDecrypt);
                            outBytes = new byte[0];
                            if (verbose)
                                System.Console.Error.WriteLine($"{exDecrypt.GetType()}: {exDecrypt.Message}\n\t{exDecrypt.ToString()}");
                        }
                    }

                }
                
                string outFile = Path.Combine(outDir, ofName);
                if (outBytes != null && outBytes.Length > 0)
                {
                    if (verbose)
                        Console.WriteLine(DateTime.Now.Area23DateTimeWithSeconds() + " writing " + outBytes.Length + " outbytes to file " + outFile);
                    File.WriteAllBytes(outFile, outBytes);
                }
                filesCount++;
            } 

            if (verbose)
                System.Console.Out.WriteLine($"{Path.GetFileName(progName)}: {filesCount} documents processed.");
            
            return;
        }

        /// <summary>
        /// Gets an option by argument
        /// </summary>
        /// <param name="argument">cmd line argument</param>
        /// <param name="optEnum"><see cref="OptEnum">OptEnum cmd arg option enum</see></param>
        /// <returns></returns>
        public static string GetOption(string argument, out OptSpoolEnum optEnum)
        {
            string optArg = "";
            if (string.IsNullOrEmpty(argument) || argument.Length < 2 || argument[0] != '-')
            {
                optEnum = OptSpoolEnum.Usage;
                return optArg;
            }
            optArg = argument;
            string arg = argument.TrimStart("-/".ToCharArray());

            if (arg.Contains("="))
                optArg = arg.Substring(arg.IndexOf("=") + 1);

            switch (arg[0])
            {
                case 'I':
                case 'i':
                    optEnum = OptSpoolEnum.InDir;
                    inDir = optArg;
                    if (string.IsNullOrEmpty(inDir))
                        Usage($"--InDir needs not null or empty parameter for incoming directory.");
                    if (!Directory.Exists(inDir))
                    {
                        if (Directory.Exists(Path.Combine(progDirectory, inDir)))
                            inDir = Path.Combine(progDirectory, inDir);
                        else
                        {
                            if (Constants.DirCreate)
                            {
                                if (!inDir.Contains(Path.DirectorySeparatorChar))
                                {
                                    inDir = Path.Combine(progDirectory, inDir);
                                    Directory.CreateDirectory(inDir);
                                }
                                else
                                    Directory.CreateDirectory(inDir);
                            }
                            else
                            {
                                inDir = Path.Combine(LibPaths.TempDir, string.IsNullOrEmpty(inDir) ? "spool_in" : inDir);
                                try
                                {
                                    if (!Directory.Exists(inDir))
                                        Directory.CreateDirectory(inDir);
                                }
                                catch (Exception exInDir)
                                {
                                    Usage($"{exInDir.GetType()}: {exInDir.Message}\n{exInDir.StackTrace}\n");
                                }
                            }
                        }
                    }
                    return optArg;

                case 'O':
                case 'o':
                    optEnum = OptSpoolEnum.OutDir;
                    outDir = optArg;
                    if (string.IsNullOrEmpty(outDir))
                        Usage($"--OutDir needs not null or empty parameter for outgoing directory.");                    
                    if (!Directory.Exists(outDir))
                    {
                        if (Directory.Exists(Path.Combine(progDirectory, outDir)))
                            outDir = Path.Combine(progDirectory, outDir);
                        else
                        {
                            if (Constants.DirCreate)
                            {
                                if (!outDir.Contains(Path.DirectorySeparatorChar))
                                {
                                    outDir = Path.Combine(progDirectory, outDir);
                                    Directory.CreateDirectory(outDir);
                                }
                                else
                                    Directory.CreateDirectory(outDir);
                            }
                            else if (!string.IsNullOrEmpty(outDir) && !outDir.Contains(Path.DirectorySeparatorChar))
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
                        }
                    }                    
                    return optArg;

                case 'D':
                case 'd':
                    optEnum = OptSpoolEnum.Decrypt;
                    Program.decryptDirection = true;
                    return optArg;

                case 'k':
                case 'K':
                    optEnum = OptSpoolEnum.Key;
                    key = optArg;
                    if (string.IsNullOrEmpty(key))
                        Usage("Key={NULL or \"\"})");
                    return optArg;

                case 'S':
                    optEnum = OptSpoolEnum.Symmetric;
                    useSymmCipher = true;
                    return optArg;

                case 'm':
                case 'M':
                    if (!Enum.TryParse<CipherMode2>(optArg, true, out cmode2))
                        cmode2 = CipherMode2.ECB;
                    optEnum = OptSpoolEnum.Mode;
                    return optArg;

                case 'v':
                case 'V':
                    optEnum = OptSpoolEnum.Verbose;
                    Constants.NOLog = false;
                    Program.verbose = true;
                    return optArg;

                    case 'g':
                    case 'G':
                    case '?':
                    default:
                        optEnum = OptSpoolEnum.Usage;
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

            System.Console.Out.WriteLine(UsageString);
            if (verbose)
            {
                System.Console.Out.WriteLine($"\nExamples: \n{progFilename} -V -S -k=bar@ba.area23.at\n" +
                        "-i=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\In   \n" +
                        "-o=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\Encrypt  \n");

                System.Console.Out.WriteLine($"\nExamples: \n{progFilename} -V -D -S -k=bar@ba.area23.at\n" +
                            "-i=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\Encrypt  \n" +
                            "-o=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\Out  \n");
            }
            System.Environment.Exit(0);
        }


        #region print only debug info
        public static void PrintSymmCipherPipe(SymmCipherPipe symmPipe, bool outPipe = false)
        {
            if (verbose)
            {
                SymmCipherEnum[] symmCiphers = (outPipe) ? symmPipe.OutSymmPipe : symmPipe.InSymmPipe;
                System.Console.Write((string)((outPipe) ? "Out:\t" : " In:\t"));
                foreach (var symmCipher in symmCiphers)
                    System.Console.Write($"{symmCipher}=>");
                System.Console.WriteLine($"\r\nSymmCipherPipe: KeyHash={symmPipe.KHash} ZipType={symmPipe.ZType} " +
                    $"EncodeType={symmPipe.EncodeType} PipeString={symmPipe.PipeString}");
            }
        }

        public static void PrintCipherPipe(CipherPipe cipherPipe, bool outPipe = false)
        {
            if (verbose)
            {
                CipherEnum[] ciphers = (outPipe) ? cipherPipe.OutPipe : cipherPipe.InPipe;
                System.Console.Write((string)((outPipe) ? "Out:\t" : " In:\t"));
                foreach (CipherEnum cipher in ciphers)
                    System.Console.Write($"{cipher}=>");
                System.Console.WriteLine($"\r\nCipherPipe: KeyHash={cipherPipe.KHash} ZipType={cipherPipe.ZType} " +
                    $"EncodeType={cipherPipe.EncodeType} PipeString={cipherPipe.PipeString}");
            }
        }
        #endregion print only debug info

    }

}

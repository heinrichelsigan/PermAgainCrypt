using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.Cipher.Symmetric;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using System.Text;
using System.Xml;


namespace EU.CqrXs.SpoolTest
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
        KeyFile = 0x4,
        Symmetric = 0x5,
        Verbose = 0x6,
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
        static readonly string progFilename = (!string.IsNullOrEmpty(progName)) ? Path.GetFileName(progName) : "EU.CqrXs.SpoolTest.exe";
        static readonly string UsageString = $"Usage:\t {progFilename} " + @"
    /// -i | --InDir={path to incoming dir} 
    /// -o | --OutDir={path to outcoming dir}
    /// -k | --KeyFile={file with 10000 of keys}     
    /// -D | --Decrypt 
    /// -S | --SymmCipher   // use symmetric chipher only to encrypt 
    /// -V | --verbose      // verbose output
    /// -? | --gethelp\n";
        // generic spooler variables
        static bool useSymmCipher = false, decryptDirection = false, verbose = false;
        static string inDir = "", outDir = "", keyFile = "";
        static string[] keys = new string[0], files = new string[0];

        // specific encrypt/decrypt process variables
        static string? inName = null, outName = null, outEnviron = null, key = null;
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

        /// <summary>
        /// Console spooler app for en-/decrypting a huge amount of files
        /// </summary>
        /// <param name="args">command line arguments</param>
        static void Main(string[] args)
        {
            if (args.Length < 1)
                Usage();

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

            int zc = 0;
            int hc = 0;
            int ec = 0;
            int kc = 0;


            if (!Directory.Exists(inDir))
            {
                inDir = Path.Combine(progDirectory, inDir.Contains(Path.DirectorySeparatorChar) ? "spool_in" : inDir);
                if (!Directory.Exists(inDir))
                    Directory.CreateDirectory(inDir);
                File.Copy(Path.Combine(progDirectory, README_FILE), Path.Combine(inDir, README_FILE), true);
            }
                    
            files = Directory.GetFiles(inDir);
            byte[] outBytes = new byte[0];
            foreach (string file in files)
            {
                passKey = keys[((kc++) % KeyHashes.Length)];
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
                        symmPipe = new SymmCipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
                        PrintSymmCipherPipe(symmPipe, decryptDirection);
                        outBytes = symmPipe.EncryptEncodeBytes(inByte, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
                        ofName += symmPipe.PipeFullExtension;
                    }
                    else // CipherPipe and all CipherEnum's
                    {
                        cPipe = new CipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
                        PrintCipherPipe(cPipe, decryptDirection);
                        outBytes = cPipe.EncryptEncodeBytes(inByte, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
                        ofName += cPipe.PipeFullExtension;
                    }
                }
                else if (file.IsPermAgainCryptFile()) // decrypting
                {
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
                        outBytes = symmPipe.DecodeDecrpytBytes(inByte, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
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
                        outBytes = cPipe.DecodeDecrpytBytes(inByte, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
                    }

                }
                
                string outFile = Path.Combine(outDir, ofName);
                if (verbose)
                    Console.WriteLine(DateTime.Now.Area23DateTimeWithSeconds() + " writing " + outBytes.Length + " outbytes to file " + outFile);
                File.WriteAllBytes(outFile, outBytes);
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
                    optEnum = OptSpoolEnum.KeyFile;
                    keyFile = optArg;
                    if (string.IsNullOrEmpty(keyFile) || !File.Exists(keyFile)) 
                    {
                        if (File.Exists(Path.Combine(progDirectory, keyFile)))
                            keyFile = Path.Combine(progDirectory, keyFile);
                        else
                        {
                            string warn = string.IsNullOrEmpty(keyFile) ? "(NULL)" : keyFile;
                            Usage($"KeyFile={warn} doesn't exist.");
                        }
                    }
                    keys = File.ReadAllLines(keyFile, Encoding.UTF8);
                    return optArg;      
                    
                case 'S':
                    optEnum = OptSpoolEnum.Symmetric;
                    useSymmCipher = true;
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
                System.Console.Out.WriteLine($"\nExamples: \n{progFilename} -V " +
                        "-i=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\In   \n" +
                        "-o=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\Out  \n" +
                        "-k=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\In\\keys.txt \n" +
                        "-S\n");

                System.Console.Out.WriteLine($"\nExamples: \n{progFilename} -V " +
                            "-i=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\Out   \n" +
                            "-o=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\ReIn  \n" +
                            "-k=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\In\\keys.txt \n" +
                            "-D -S\n");
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

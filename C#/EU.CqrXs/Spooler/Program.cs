using EU.CqrXs.Crypt;
using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using System.Runtime.CompilerServices;

namespace EU.CqrXs.Spooler
{

    /// <summary>
    /// OptEnum different option types
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>code changes</listheader>
    /// <item>
    /// 2026-02-11 alert-fix-13 changed mode from "ECB" to "CFB"     
    /// Reason: Git security scans
    /// consequences: no more fully deterministic math bijective proper symmertric cipher en-/decryption in pipe
    /// fixed attacks: not so easy REPLY attacks with binary format header and heuristic key collection
    /// </item>
    /// <item>
    /// 2026-mm-dd [enter pull request name here] [enter what you did here]
    /// Reason: [enter a senseful reason]
    /// consequences: [describe most impactful consequences of bugfix or code change request]
    /// fixed [vulnerability, code smell]: [Describe understandable precise in 1-2 setences]
    /// </item>
    /// </list>
    /// </remarks>
    public enum OptSpoolEnum
    {
        Usage = 0x0,
        InDir = 0x1,
        OutDir = 0x2,
        Decrypt = 0x3,
        Key = 0x4,
        Secure = 0x5,
        Mode = 0x6,
        Verbose = 0xe,
        Help = 0xf
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
    /// -M | --mode={CBC|CFB|ECB} default: CFB
    /// -S | --secure
    /// -V | --verbose
    /// -? | --help
    /// </summary>
    internal class Program
    {        
        static readonly string? progName = System.Environment.ProcessPath;
        static readonly string? progDirectory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        static readonly string progFilename = (!string.IsNullOrEmpty(progName)) ? Path.GetFileName(progName) : "EU.CqrXs.SpoolTest.exe";
        static readonly string UsageString = $"Usage:\t {progFilename} " + @"
    /// -i | --InDir={path to incoming dir} 
    /// -o | --OutDir={path to outcoming dir}
    /// -k | --Key=secretKey
    /// -D | --Decrypt 
    /// -M | --mode={CBC|CFB|ECB} default: CFB
    /// -S | --secure
    /// -V | --verbose      // verbose output
    /// -? | --help\n";
        // generic spooler variables
        static bool decryptDirection = false, verbose = false, secureCipher = false;
        static string inDir = "", outDir = "", keyFile = "";
        static string[] keys = new string[0], files = new string[0];

        // specific encrypt/decrypt process variables
        static string? outEnviron = null, key = null;
        static FileInfo? inFile = null, outFile = null;
        static byte[] inBytes = new byte[0], outBytes = new byte[0];
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
        static CipherMode2 cmode2 = CipherMode2.CFB;

        /// <summary>
        /// Console spooler app for en-/decrypting a huge amount of files
        /// </summary>
        /// <param name="args">command line arguments</param>
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
                Usage();

            DateTime startDate = DateTime.Now;
            TimeSpan duration = startDate - DateTime.Now;
            CException decryptExc = null, encryptExc = null;
            bool keyFromArg = true;
            long filesCount = 0;
            encodingType = EncodingType.None;
            Constants.DirCreate = false;
            Constants.NOLog = true;

            string[] algos = new List<string>().ToArray();

            for (int i = 0; i < args.Length; i++)
            {
                string[] optArgs = GetOption(args[i]);
                OptSpoolEnum optSpoolEnum = Enum.Parse<OptSpoolEnum>(optArgs[0]);
                string optArg = optArgs[1];
            }

            int zc = 0, hc = 0, ec = 0;

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
            Verbose($"Key read from {(keyFromArg ? "argument" : "stdin")}: '{key}'");

            files = Directory.GetFiles(inDir);
            outBytes = new byte[0];
            foreach (string file in files)
            {
                passKey = key; // keys[((kc++) % KeyHashes.Length)];
                startDate = DateTime.Now;
                inBytes = File.ReadAllBytes(file);        // read all bytes from file
                string ofName = Path.GetFileName(file);         // gets full filename without directory path
                Verbose($"reading {inBytes.Length} bytes from file {ofName}");

                keyHash = KeyHashes[((++hc) % KeyHashes.Length)];

                if (!secureCipher)
                {                    
                    encodingType = AsciiEncoders[((++ec) % AsciiEncoders.Length)];
                    zipType = ZipTypes[((++zc) % ZipTypes.Length)];

                    CipherPipe cPipe;
                    if (!decryptDirection) // encrypting
                    {
                        // CipherPipe and all CipherEnum's
                        cPipe = new CipherPipe(keyHash.Hash(passKey), passKey, encodingType, zipType, keyHash, cmode2);
                        PrintCipherPipe(cPipe, decryptDirection);
                        outBytes = cPipe.EncryptEncodeBytes(inBytes, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, cmode2);
                        ofName += cPipe.PipeFullExtension;
                    }
                    else if (file.IsPermAgainCryptFile()) // decrypting
                    {
                        keyHash = KeyHash.Hex;
                        zipType = ZipType.None;

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
                            outBytes = cPipe.DecodeDecrpytBytes(inBytes, passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, cmode2);
                        }
                        catch (Exception exDecrypt)
                        {
                            decryptExc = new CException($"{exDecrypt.GetType()} was thrown decrypting {ofName} with {cPipe.PipeFullExtension}", exDecrypt);
                            outBytes = new byte[0];
                            Verbose($"{exDecrypt.GetType()}: {exDecrypt.Message}\n\t{exDecrypt.ToString()}", true);
                        }
                    }
                }
                else
                {
                    encodingType = EncodingType.Base64;
                    zipType = ZipType.GZip;

                    SecureCipherPipe sPipe;
                    if (!decryptDirection) // encrypting
                    {
                        // CipherPipe and all CipherEnum's
                        sPipe = new SecureCipherPipe(keyHash.Hash(passKey), cmode2, false);
                        sPipe.ZType = zipType;
                        sPipe.EncodeType = encodingType;
                        

                        PrintSecureCipherPipe(sPipe, decryptDirection);
                        outBytes = sPipe.EncryptEncodeBytes(inBytes, passKey, cmode2);
                        ofName += sPipe.PipeFullExtension;
                    }
                    else if (file.IsPermAgainCryptFile()) // decrypting
                    {
                        keyHash = KeyHash.Hex;
                        zipType = ZipType.None;

                        ofName = ofName.StripSecureCipherPipeFromFileName(out sPipe);
                        if (sPipe != null)
                        {
                            encodingType = sPipe.EncodeType;
                            zipType = sPipe.ZType;
                            cmode2 = sPipe.CMode2;
                        }
                        PrintSecureCipherPipe(sPipe, decryptDirection);
                        try
                        {
                            outBytes = sPipe.DecodeDecrpytBytes(inBytes, passKey, cmode2);
                        }
                        catch (Exception exDecrypt)
                        {
                            decryptExc = new CException($"{exDecrypt.GetType()} was thrown decrypting {ofName} with {sPipe.PipeFullExtension}", exDecrypt);
                            outBytes = new byte[0];
                            Verbose($"{exDecrypt.GetType()}: {exDecrypt.Message}\n\t{exDecrypt.ToString()}", true);
                        }
                    }
                }                
                
                string outFile = Path.Combine(outDir, ofName);
                if (outBytes != null && outBytes.Length > 0)
                {
                    Verbose($"writing {outBytes.Length} outbytes to file {outFile}");
                    File.WriteAllBytes(outFile, outBytes);
                }
                filesCount++;
                duration = DateTime.Now.Subtract(startDate);

                Verbose($"perf in: {BytesPerSecond(inBytes.LongLength, duration.TotalSeconds)}, " +
                    $"out: {BytesPerSecond(outBytes.LongLength, duration.TotalSeconds)}");
            }

            Verbose($"{Path.GetFileName(progName)}: {filesCount} documents processed.");
            
            return;
        }

        /// <summary>
        /// Gets an option by argument
        /// </summary>
        /// <param name="argument">cmd line argument</param>
        /// <returns><see cref="T:string[2]">optArgs</see> where optArgs[0] contains OptSpoolEnum, optArgs[1] contains option value</returns>
        public static string[] GetOption(string argument)
        {
            OptSpoolEnum optEnum = OptSpoolEnum.Usage;
            if (string.IsNullOrEmpty(argument) || argument.Length < 2 || argument[0] != '-')
            {
                optEnum = OptSpoolEnum.Usage;
                Usage($"unrecognized option: {argument}.");
                System.Environment.Exit(1);
            }
            
            string arg = argument.TrimStart("-/".ToCharArray());
            string[] optArgs = (arg.Contains("=")) ? arg.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries) : new string[] { arg, "" };

            switch (arg[0])
            {
                case 'I':
                case 'i':
                    optEnum = OptSpoolEnum.InDir;
                    optArgs[0] = optEnum.ToString();
                    inDir = optArgs[1];
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
                                inDir = Path.Combine(Area23Log.TempDir, string.IsNullOrEmpty(inDir) ? "spool_in" : inDir);
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
                    
                    return optArgs;

                case 'O':
                case 'o':
                    optEnum = OptSpoolEnum.OutDir;
                    optArgs[0] = optEnum.ToString();
                    outDir = optArgs[1];
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
                                outDir = Path.Combine(Area23Log.TempDir, "spool_out");
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
                    return optArgs;

                case 'D':
                case 'd':
                    optEnum = OptSpoolEnum.Decrypt;
                    optArgs[0] = optEnum.ToString();
                    Program.decryptDirection = true;
                    return optArgs;

                case 'k':
                case 'K':
                    optEnum = OptSpoolEnum.Key;
                    optArgs[0] = optEnum.ToString();
                    key = optArgs[1];
                    if (string.IsNullOrEmpty(key))
                        Usage("Key={NULL or \"\"})");
                    return optArgs;
              
                case 'm':
                case 'M':
                    if (!Enum.TryParse<CipherMode2>(optArgs[1], true, out cmode2))
                        cmode2 = CipherMode2.CFB;
                    optEnum = OptSpoolEnum.Mode;                    
                    optArgs[0] = optEnum.ToString();
                    return optArgs;

                case 's':
                case 'S':
                    secureCipher = true;
                    optEnum = OptSpoolEnum.Secure;                   
                    optArgs[0] = optEnum.ToString();
                    return optArgs;

                case 'v':
                case 'V':
                    optEnum = OptSpoolEnum.Verbose;
                    optArgs[0] = optEnum.ToString();
                    Constants.NOLog = false;
                    Program.verbose = true;
                    return optArgs;

                case 'H':
                case 'h':
                case '?':
                default:
                    optEnum = OptSpoolEnum.Usage;
                    optArgs[0] = optEnum.ToString();
                    optArgs[1] = $"unrecognized option: {argument}.";
                    Usage(optArgs[1]);
                    break;
            }

            return optArgs;
        }

        /// <summary>
        /// Usage shows the usage of console application
        /// </summary>
        internal static void Usage(string errMsg = "")
        {
            if (!string.IsNullOrEmpty(errMsg))
                System.Console.Error.WriteLine(errMsg);

            System.Console.Out.WriteLine(UsageString);
            Verbose($"\nExamples: \n{progFilename} -V -S -k=bar@ba.area23.at\n" +
                        "-i=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\In   \n" +
                        "-o=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\Encrypt  \n");

            Verbose($"\nExamples: \n{progFilename} -V -D -S -k=bar@ba.area23.at\n" +
                            "-i=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\Encrypt  \n" +
                            "-o=S:\\PermAgainCrypt\\Deploy\\SpoolTest\\Out  \n");
            
            System.Environment.Exit(0);
        }

        internal static string BytesPerSecond(long byteLen, double seconds)
        {
            long bps = (long)(byteLen / seconds);
            if (bps < 2048)
                return $"{bps} bytes/s";
            if (bps < 1024 * 1024)
            {
                bps = (long)(bps / 1024);
                return $"{bps} KB/s";
            }
            bps = (long)(bps / (1024 * 1024));
            return $"{bps} MB/s";
        }

        #region print verbose debug info

        /// <summary>
        /// Verbose prints message to stdout or stderr
        /// </summary>
        /// <param name="s"><see cref="string">string s</see></param>
        /// <param name="stdErr"><see cref="bool">stdErr</see> if true, prints to stderr, otherwise to stdout, default false</param>
        public static void Verbose(string s, bool stdErr = false) // default to stdout
        {
            if (verbose)
            {
                if (stdErr)
                    Console.Error.WriteLine(DateTime.Now.Area23DateTimeWithSeconds().ToString() + " " + s);
                else
                    Console.Out.WriteLine(DateTime.Now.Area23DateTimeWithSeconds().ToString() + " " + s);
            }
        }

        /// <summary>
        /// Prints the properties of <see cref="CipherPipe"/>
        /// </summary>
        /// <param name="cipherPipe"><see cref="CipherPipe"/></param>
        /// <param name="outPipe">direction decrypt</param>
        public static void PrintCipherPipe(CipherPipe cipherPipe, bool outPipe = false)
        {
            if (verbose)
            {
                CipherEnum[] ciphers = (outPipe) ? cipherPipe.OutPipe : cipherPipe.InPipe;
                System.Console.Write((string)((outPipe) ? "Out:\t" : " In:\t"));
                foreach (CipherEnum cipher in ciphers)
                    System.Console.Write($"{cipher}=>");
                System.Console.WriteLine($"\r\nCipherPipe: KeyHash={cipherPipe.KHash} ZipType={cipherPipe.ZType} " +
                    $"EncodeType={cipherPipe.EncodeType} CipherMode={cipherPipe.CMode2} PipeString={cipherPipe.PipeString}");
            }
        }


        /// <summary>
        /// Prints the properties of <see cref="SecureCipherPipe"/>
        /// </summary>
        /// <param name="secCipherPipe"><see cref="SecureCipherPipe"/></param>
        /// <param name="outPipe">direction decrypt</param>
        public static void PrintSecureCipherPipe(SecureCipherPipe secCipherPipe, bool outPipe = false)
        {
            if (verbose)
            {
                CipherEnum[] ciphers = (outPipe) ? secCipherPipe.OutPipe : secCipherPipe.InPipe;
                System.Console.Write((string)((outPipe) ? "Out:\t" : " In:\t"));
                foreach (CipherEnum cipher in ciphers)
                    System.Console.Write($"{cipher}=>");
                System.Console.WriteLine($"\r\nSecureCipherPipe: ZipType={secCipherPipe.ZType} " +
                    $"EncodeType={secCipherPipe.EncodeType} CipherMode={secCipherPipe.CMode2} PipeString={secCipherPipe.PipeString}");
            }
        }


        #endregion print verbose debug info

    }

}

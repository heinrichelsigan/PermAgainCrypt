using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.Cipher.Symmetric;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using EU.Net.WebHttp;
using System.Text;


namespace EU.CqrXs.Console
{

    /// <summary>
    /// OptEnum different option types
    /// </summary>
    public enum OptEnum
    {
        Usage = 0x0,
        InParam = 0x1,
        OutP = 0x2,
        Zip = 0x3,
        Unzip = 0x4,
        Encode = 0x5,
        Decode = 0x6,
        Crypt = 0x7,
        Key = 0x8,
        Decrypt = 0x9,
        HashSum = 0xa,
        Help = 0xb,
        Qey = 0xc,
        Pass = 0xd,
        Hash = 0xe,
        SymmCipher = 0xf,

        AutoTests = 0x10,
        AutoTestSingle = 0x11,
        AutoTestsTiny = 0x1a,
        AutoTestsSmall = 0x1b,
        AutoTestsLarge = 0x1c,
        AutoTestsHuge = 0x1d,

        DownloadRandomData = 0x20,
        DownloadLatestPicture = 0x21,
        DownloadLastPictures = 0x22
    }

    /// <summary>
    /// struct OptionArgument
    /// </summary>
    struct OptionArgument
    {
        static readonly char[] charSeperators = new char[] { '=', ':' };
        System.Buffers.SearchValues<char> searchValues = System.Buffers.SearchValues.Create(new ReadOnlySpan<char>(charSeperators));
        public OptEnum Option { get; set; }
        public string Argument { get; set; }
        internal string[] OptArgs
        {
            get => (Argument.ContainsAny(System.Buffers.SearchValues.Create(new ReadOnlySpan<char>(charSeperators))) ?
                        Argument.Split(charSeperators, StringSplitOptions.RemoveEmptyEntries) :
                        new string[] { Argument });
        }
        internal string argOptKey;
        internal string argOptValue;

        public OptionArgument() { Option = OptEnum.Usage; Argument = ""; argOptKey = ""; argOptValue = ""; }
        public OptionArgument(OptEnum opt, string arg)
        {
            Option = opt;
            Argument = arg ?? "";
            argOptKey = (OptArgs.Length > 0) ? OptArgs[0] : Argument;
            argOptValue = (OptArgs.Length > 1) ? OptArgs[1] : Argument;
        }
    }

    /// <summary>
    /// Console app pipe for crypt/decrypt zip/unzip encode/decode md5sum/shaSum
    /// 
    /// EU.CqrXs.Console.Program 
    /// -0 | --AutoTest={Single,tiny,small,default,Large,HUGE} |  --Download={Url}
    /// -i | --inFile= | --inText={string|EnviromentVariable} | --inStd    
    /// -o | --outFile= | --outText=EnviromentVariable | --outStd
    /// -u | --unzip={gzip | bzip2 | zip}
    /// -z | --zip={gzip|bzip2|zip}
    /// -d | --decode={raw|hex16|hex32|base32|base64|uu}
    /// -e | --encode={raw|hex16|hex32|base32|base64|uu}
    /// -C | --crypt={[aes,des3,blowfish,fish2,fish3]|key}
    ///    |  -p --pass=Passphrase
    /// -D | --decrypt={[aes,des3,blowfish,fish2,fish3]|key}
    ///    |  -p --pass=Passphrase
    /// -k | --key=mykey
    /// -q | --qey=myqey    
    /// -H | --hash={Oct|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|TupleHash}
    /// -S | --SymmCipher 
    /// -? | --gethelp
    /// </summary>
    internal class Program
    {
        public const string AUTO_TEST = "AutoTest";
        public const string DOWNLOAD = "Download";
        public const string BATCH_FILE_TEST = "Console_Test.bat";
        public static readonly string[] BatchTestModes =
            new string[] { "single", "tiny", "small", "default", "Large", "HUGE" };
        
        public const string README_FILE = "README.MD";
        static bool useSymmCipher = false;
        static readonly string? progName = System.Environment.ProcessPath;
        static readonly string? progDirectory = Path.GetFullPath(System.Environment.ProcessPath);
        static string? inName = null, outName = null, outEnviron = null, key = null;
        static bool reverseDirection = false;
        static FileInfo? inFile = null, outFile = null;
        static byte[]? inBytes = null, outBytes = null;
        static string passKey = "";
        static ZipType zipType = ZipType.None;
        static EncodingType encodingType = EncodingType.None;
        static KeyHash keyHash = KeyHash.Hex;
        static string[] algos = new string[] { };

        /// <summary>
        /// Console app pipe for crypt/decrypt zip/unzip encode/decode md5sum/shaSum
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (ProcessCmdLineArgs(args) < 0)
                Usage();

            Constants.DirCreate = false;
            Constants.NOLog = true;
            encodingType = EncodingType.None;

            for (int i = 0; i < args.Length; i++)
            {
                string modeArg = args[i];
                if (!string.IsNullOrEmpty(modeArg) &&
                    (modeArg.StartsWith("-0") ||
                    modeArg.Contains($"--{AUTO_TEST}", StringComparison.CurrentCultureIgnoreCase) ||
                    modeArg.Contains(DOWNLOAD, StringComparison.CurrentCultureIgnoreCase)))
                {
                    if (modeArg.Contains(DOWNLOAD, StringComparison.InvariantCultureIgnoreCase))
                    {
                        DownloadImage(modeArg);
                    }

                    if (modeArg.Contains(AUTO_TEST, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ProcessCmd.Execute("start", $" {BATCH_FILE_TEST}", false);
                        return;
                    }
                    break;
                }
            }
            
            ExecutePipe();

            return;
        }

        /// <summary>
        /// Downloads an absolute referemces image from toplevel domain via 4 known search engines
        /// </summary>
        /// <param name="modeArg">argument of args, that probably cointains <see cref=">DOWNLOAD" /></param>
        public static void DownloadImage(string modeArg)
        {
            int imgUrlIdx = -1;
            string urlImage = "";
            List<string> imgs = WebClientRequest.LatestAtImages(".at");
            string fName = Path.Combine(progDirectory, DateTime.Now.Area23DateTimeWithMillis() + ".img");

            foreach (string anImg in imgs)
            {
                try
                {
                    if (anImg.Contains("<img", StringComparison.InvariantCultureIgnoreCase) &&
                        anImg.Contains("src", StringComparison.InvariantCultureIgnoreCase) &&
                        anImg.Contains("=") &&
                        (imgUrlIdx = anImg.IndexOf("src")) > -1)
                    {
                        urlImage = anImg.Substring(imgUrlIdx + 1);
                        if ((imgUrlIdx = urlImage.IndexOf("=")) > -1)
                            urlImage = urlImage.Substring(imgUrlIdx + 1);
                        urlImage = urlImage.Trim("\"'".ToCharArray());

                        if (urlImage.Contains('>'))
                        {
                            urlImage = (urlImage.Contains("/>") ?
                                        urlImage.Substring(0, urlImage.IndexOf("/") - 1) :
                                        urlImage.Substring(0, urlImage.IndexOf(">") - 1));
                        }

                        Uri uri = new Uri(urlImage);
                        if (uri.IsWellFormedOriginalString() || uri.ToString().Contains("://"))
                        {
                            FileInfo fi = WebClientRequest.DownloadBytes(uri.ToString(), fName, System.Text.Encoding.UTF8);
                            if (fi.Exists && uri.ToString().Contains('/') && uri.ToString().Contains("."))
                            {
                                string fileRest = "", localPath = uri.LocalPath.ToString();
                                if ((imgUrlIdx = uri.ToString().LastIndexOf("/")) > -1)
                                {
                                    fileRest = uri.ToString().Substring(imgUrlIdx + 1);
                                    if (localPath.Contains('.') && localPath.Length > 3)
                                        fileRest = localPath;
                                    if (fileRest.Contains('.') && fileRest.Length > 3)
                                    {
                                        fi.CopyTo(fileRest);
                                        Thread.Sleep(40);
                                        fi.Delete();
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception urlExc)
                {
                    Area23Log.LogOriginMsgEx(progName, $"{urlExc.GetType()}:", urlExc);
                }
            }
        }

        /// <summary>
        /// generates additional needed files for Crypt Console
        /// </summary>
        public void GenConsoleAddítionalFiles()
        {
            if (!File.Exists(Path.Combine(progDirectory, README_FILE)))
                File.WriteAllText(Path.Combine(progDirectory, README_FILE), readmeMD);
            if (!File.Exists(Path.Combine(progDirectory, BATCH_FILE_TEST)))
                File.WriteAllText(Path.Combine(progDirectory, BATCH_FILE_TEST), console_test);

        }

        /// <summary>
        /// parses command line arguments
        /// </summary>
        /// <param name="args"><see cref="T:string[]"/></param>
        /// <returns>number of arguments totally processed</returns>
        public static int ProcessCmdLineArgs(string[] args)
        {
            if (args == null || args.Length <= 1)
                return -1;

            OptionArgument optArg = new OptionArgument(OptEnum.Usage, "");
            OptEnum optEnum = optArg.Option;
            string optStr = "";
            Dictionary<OptEnum, string> dict = new Dictionary<OptEnum, string>();
            int readChars = -1;

            for (int i = 0; i < args.Length; i++)
            {
                // string optStr = GetOption(... => out OptEnum optEnum)
                optArg = GetOptArg(args[i]);
                optEnum = optArg.Option;
                optStr = optArg.argOptValue;

                // Nothing todo on io params
                if (optEnum == OptEnum.OutP || optEnum == OptEnum.InParam) ;
                else // Help => Usage("") Usage => Usage(optStr)
                    if (optEnum == OptEnum.Help || optEnum == OptEnum.Usage)
                    Usage((optEnum == OptEnum.Help) ? "" : optStr);
                else // fetch  or Key or Qey (decrypt key) from optEnum and optStr
                    if (optEnum == OptEnum.Pass || optEnum == OptEnum.Key || optEnum == OptEnum.Qey)
                    passKey = optStr;
                else // prefetch SymmCipherMode
                    if (optEnum == OptEnum.SymmCipher)
                    useSymmCipher = true;
                else // otherwise add optEnum and optStr to Dictionary<OptEnum, string>();  
                    dict.Add(optEnum, optStr);
            }

            if (string.IsNullOrEmpty(inName))
                readChars = ReadFromStdin(ref inBytes, ref outBytes);

            // iterate all option keys
            foreach (OptEnum optVar in dict.Keys)
            {
                optStr = dict[optVar];
                switch (optVar)
                {
                    case OptEnum.Zip:
                    case OptEnum.Unzip:
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
                    case OptEnum.Encode:
                    case OptEnum.Decode:
                        encodingType = EncodingTypesExtensions.GetEnum(optStr);
                        break;
                    case OptEnum.Hash:
                        keyHash = KeyHash_Extensions.GetKeyHashFromString(optStr);
                        break;
                    case OptEnum.Crypt:
                    case OptEnum.Decrypt: // Usage on not existing or empty passphrase / key
                        if (string.IsNullOrEmpty(passKey) || string.IsNullOrWhiteSpace(passKey))
                            Usage($"urecognized crypt option \"{optStr}\" without --pass=passPhrase ");

                        // when string / array is not null, fetch array for crypt pipe
                        if (!string.IsNullOrEmpty(optStr))
                        {
                            optStr = optStr.Replace("(", "").Replace("{", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace(")", "");
                            algos = optStr.Split(",;:".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        }
                        break;
                    default: break;
                }
            }

            return args.Length;
        }

        /// <summary>
        /// Execute encrypt:
        ///     1st optional zipping, 2nd - 9th cipher transform operation, 10th ascii encoding
        /// Execute decrypt: 
        ///     1st ascii decoding, 2nd - 9th reverse cipher detransform operation, 10th optional unzipping
        /// </summary>
        public static void ExecutePipe()
        {
            // Create cipher pipe for en-/decryption
            CipherPipe pipe = (algos.Length > 0) ?
                            new CipherPipe(algos, Constants.MAX_PIPE_LEN, encodingType, zipType, keyHash) :
                            new CipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
            SymmCipherPipe symmPipe;
            if (useSymmCipher)
            {

            }
            // System.Console.WriteLine($"CipherPipe: KeyHash={pipe.KHash} ZipTyoe={pipe.ZType} EncodeType={pipe.EncodeType} PipeString={pipe.PipeString}");

            string outString = "";
            if (!reverseDirection)
            {
                System.Console.Write($" InPipe: ");
                if (useSymmCipher)
                {
                    symmPipe = (algos.Length > 0) ?
                        new SymmCipherPipe(algos, 8, encodingType, zipType, keyHash) :
                        new SymmCipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
                    foreach (var symmCipher in symmPipe.InPipe)
                        System.Console.Write($"{symmCipher}=>");
                    System.Console.WriteLine($"\r\nSymmCipherPipe: KeyHash={symmPipe.KHash} ZipType={symmPipe.ZType} " +
                        $"EncodeType={symmPipe.EncodeType} PipeString={symmPipe.PipeString}");
                    outString = symmPipe.EncrpytEncode(inBytes, passKey, encodingType, zipType, keyHash);
                }
                else
                {
                    foreach (CipherEnum cipher in pipe.InPipe)
                        System.Console.Write($"{cipher}=>");
                    System.Console.WriteLine($"\r\nCipherPipe: KeyHash={pipe.KHash} ZipType={pipe.ZType} " +
                        $"EncodeType={pipe.EncodeType} PipeString={pipe.PipeString}");
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
                    symmPipe = (algos.Length > 0) ?
                        new SymmCipherPipe(algos, 8, encodingType, zipType, keyHash) :
                        new SymmCipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);
                    foreach (var symmCipher in symmPipe.OutPipe)
                        System.Console.Write($"{symmCipher}=>");
                    System.Console.WriteLine($"\r\nSymmCipherPipe: KeyHash={symmPipe.KHash} ZipType={symmPipe.ZType} " +
                        $"EncodeType={symmPipe.EncodeType} PipeString={symmPipe.PipeString}");
                    outBytes = symmPipe.DecodeDecrpyt(inString, passKey, encodingType, zipType, keyHash);
                }
                else
                {
                    foreach (CipherEnum cipher in pipe.OutPipe)
                        System.Console.Write($"{cipher}=>");
                    System.Console.WriteLine($"\r\nCipherPipe: KeyHash={pipe.KHash} ZipType={pipe.ZType} " +
                        $"EncodeType={pipe.EncodeType} PipeString={pipe.PipeString}");
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
        /// ReadFromStdin ready <see cref="inBytes"/> as
        /// </summary>
        /// <param name="inbytes">reference to global variable <see cref="inBytes"/></param>
        /// <param name="outbytes">reference to global variable <see cref="outBytes"/></param>
        /// <returns>-1 on fail or number of bytes read</returns>
        public static int ReadFromStdin(ref byte[] inbytes, ref byte[] outbytes)
        {
            int bytesRead = -1;
            inbytes = inbytes ?? new byte[0];
            List<byte> listBytes = null;
            // read from stdin, when no inName specified            
            System.Console.WriteLine("Reading from stdin, enter \r\n^Z (Enter Strg - z Enter) to stop reading from stdin");
            using (Stream stdin = System.Console.OpenStandardInput())
            {
                listBytes = listBytes ?? new List<byte>();
                byte[] buffer = new byte[2048];

                while ((bytesRead = stdin.Read(buffer, 0, buffer.Length)) > 0)
                    listBytes.AddRange(buffer);

                outBytes = EnDeCodeHelper.GetBytesTrimCrLfNulls(listBytes.ToArray());
                inBytes = new byte[outBytes.Length];
                Array.Copy(outbytes, 0, inbytes, 0, outbytes.Length);
            }

            return (listBytes == null) ? -1 : listBytes.Count;
        }

        /// <summary>
        /// Gets an option by argument
        /// </summary>
        /// <param name="argument">cmd line argument</param>
        /// <param name="optEnum"><see cref="OptEnum">OptEnum cmd arg option enum</see></param>
        /// <returns>
        /// <see cref="T:OptionArgument" /> with 
        ///     string OptionArgument.Argument 
        ///     OptEnum OptionArgument.Option
        /// </returns>
        public static OptionArgument GetOptArg(string argument)
        {
            int optIndex = -1;
            OptionArgument optArg = new OptionArgument(OptEnum.Usage, argument);

            if (string.IsNullOrEmpty(argument) || argument.Length < 2 || argument[0] != '-' || argument[0] != '/')
                return optArg;

            string arg = argument.TrimStart("-/".ToCharArray());
            if ((optIndex = arg.IndexOf('=')) > -1)
            {
                optArg.argOptKey = arg.Substring(0, optIndex);
                optArg.argOptValue = arg.Substring(optIndex + 1);
            }
            else if ((optIndex = arg.IndexOf(':')) > -1)
            {
                optArg.argOptKey = arg.Substring(0, optIndex);
                optArg.argOptValue = arg.Substring(optIndex + 1);
            }
            switch (arg[0])
            {
                case 'I':
                case 'i':
                    optArg.Option = OptEnum.InParam;
                    inName = optArg.argOptValue;
                    if (string.IsNullOrEmpty(inName))
                        ; // Else
                    else
                        if (arg.ToLower().Contains("file") || File.Exists(inName) || File.Exists(Path.Combine(progDirectory, inName)))
                    {
                        if (File.Exists(Path.Combine(progDirectory, inName)))
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
                    optArg.Option = OptEnum.OutP;
                    outName = optArg.argOptValue;
                    if (string.IsNullOrEmpty(outName))
                        ; // to stdout                    
                    else
                        if (arg.ToLower().Contains("file") || optArg.argOptValue.Contains(LibPaths.SepChar) || optArg.argOptValue.Contains('.') || !string.IsNullOrEmpty(outName))
                        outFile = new FileInfo(outName);
                    else
                        if (!string.IsNullOrEmpty(outName) || arg.ToLower().Contains("text") || optArg.argOptValue.StartsWith("$"))
                        outEnviron = optArg.argOptValue;

                    return optArg;
                case 'Z':
                case 'z':
                    optArg.Option = OptEnum.Zip;
                    return optArg;
                case 'U':
                case 'u':
                    reverseDirection = true;
                    optArg.Option = OptEnum.Unzip;
                    return optArg;
                case 'E':
                case 'e':
                    optArg.Option = OptEnum.Encode;
                    return optArg;
                case 'd':
                    reverseDirection = true;
                    optArg.Option = OptEnum.Decode;
                    return optArg;
                case 'C':
                case 'c':
                    optArg.Option = OptEnum.Crypt;
                    return optArg;
                case 'D':
                    reverseDirection = true;
                    optArg.Option = OptEnum.Decrypt;
                    return optArg;
                case 'k':
                case 'K':
                    optArg.Option = OptEnum.Key;
                    return optArg;
                case 'p':
                case 'P':
                    optArg.Option = OptEnum.Pass;
                    return optArg;
                case 'q':
                case 'Q':
                    reverseDirection = true;
                    optArg.Option = OptEnum.Qey;
                    return optArg;
                case 'h':
                case 'H':
                    optArg.Option = OptEnum.Hash;
                    return optArg;
                case 'S':
                    optArg.Option = OptEnum.SymmCipher;
                    return optArg;
                case 'g':
                case 'G':
                case '?':
                default:
                    optArg.Option = OptEnum.Usage;
                    optArg.Argument = $"unrecognized option: {argument}.";
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

            System.Console.Out.WriteLine($"Usage:\t{Path.GetFileName(progName)} \n" + @"
    -i | --inFile:  FileName 
       | --inText:  {String | EnviromentVariable} 
       | --inStd
    -o | --outFile: FileName 
       | --outText: EnviromentVariable 
       | --outStd
    -u | --unzip =  {gzip | bzip2}
    -z | --zip =    {gzip | bzip2}
    -d | --decode = {raw | hex16 | hex32 | base32 | base64 | uu}
    -e | --encode = {raw | hex16 | hex32 | base32 | base64 | uu}
    -c | --crypt =  {algo1, algo2, ...}
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
    -p | --pass =   Passphrase
    -D | --decrypt: {algo1, algo2, ...}
    -p | --pass =   Passphrase
    -k | --key =    passKey encrypt
    -q | --qey =    passKey decrypt
    -h | --hash =   {Oct | Blake2xs | BCrypt | CShake | Dstu7564 | MD5 | RipeMD256 | SCrypt | Sha1 | Sha256 | Sha384 | Sha512 | Whirlpool | TupleHash}
    -S | --SymmCipher
    -? | --gethelp");

            System.Console.Out.WriteLine($"\nExamples: ");
            System.Console.Out.WriteLine($"\t{Path.GetFileName(progName)} -i=test.jpg -z=bzip2 -e=base32 -o=test.jpg.bz2.base32");
            System.Console.Out.WriteLine($"\t{Path.GetFileName(progName)} -i=test.jpg.bz2.base32 -d=base32 -u=bzip2 -o=test1.jpg");
            System.Console.Out.WriteLine($"\n\t{Path.GetFileName(progName)} --inFile=test.jpg --zip=gzip --crypt=AesLight,Fish3 -k=MySecretKey -e=base64 -o=test.jpg.gz.aeslight.fish3.base64");
            System.Console.Out.WriteLine($"\t{Path.GetFileName(progName)} -i=test.jpg.gz.aeslight.fish3.base64 -d=base64  -D=AesLight,Fish3 -k=MySecretKey -e=base64  --unzip=gzip  -o=test2.jpg");
            System.Console.Out.WriteLine($"\n\t{Path.GetFileName(progName)} -i=README.MD -z=zip -k=io.cqrxs.eu -H=SCrypt -e=uu -o=README.MD.SCrypt.zip.uu");
            System.Console.Out.WriteLine($"\t{Path.GetFileName(progName)} -i=README.MD.SCrypt.zip.uu -d=uu -q=io.cqrxs.eu -H=SCrypt -u=zip -o=README_UNZIP.txt");
            System.Environment.Exit(0);
        }


        #region static readonly strings

        static readonly string console_test = "@echo off\n\n" +
            "echo Staring console tests by executable: " + progName + " \n" + Environment.NewLine +
            "echo deleting README.MD.BlowFish.Fish2.Fish3.base64 README.MD.Whirlpool.bz.Base32 README.MD.SCrypt.zip.uu README_UNZIP.txt README_GUNZIP.txt README_BUNZIP.txt README.MD.BCrypt.zip.xx README_SYM_BCRYPT_UNZIP.txt \n\n" +
            "del /q README.MD.BlowFish.Fish2.Fish3.base64 README.MD.Whirlpool.bz.Base32 README.MD.SCrypt.zip.uu README_UNZIP.txt README_GUNZIP.txt README_BUNZIP.txt README.MD.BCrypt.zip.xx README_SYM_BCRYPT_UNZIP.txt \n\n" +
            "@echo on \n" + Environment.NewLine +
            Path.GetFileName(progName) + "\t -i=.\\README.MD -z=gzip  -c=BlowFish,Fish2,Fish3 -p=Hallo -e=base64 -o=.\\README.MD.BlowFish.Fish2.Fish3.base64 \n" +
            Path.GetFileName(progName) + "\t -i=.\\README.MD.BlowFish.Fish2.Fish3.base64  -d=base64 -D=BlowFish,Fish2,Fish3 -p=Hallo -u=gzip -o=.\\README_GUNZIP.txt \n " +
            Path.GetFileName(progName) + "\t -i=.\\README.MD -z=bz -k=heinrichelsigan.area23.at -H=Whirlpool -e=hex32 -o=.\\README.MD.Whirlpool.bz.Hex32 \n" +
            Path.GetFileName(progName) + "\t -i=.\\README.MD.Whirlpool.bz.Hex32 -d=hex32 -q=heinrichelsigan.area23.at -H=Whirlpool -u=bz -o=.\\README_BUNZIP.txt \n" +
            Path.GetFileName(progName) + "\t -i=.\\README.MD -z=zip -k=io.cqrxs.eu -H=SCrypt -e=uu -o=.\\README.MD.SCrypt.zip.uu \n" +
            Path.GetFileName(progName) + "\t -i=.\\README.MD.SCrypt.zip.uu -d=uu -q=io.cqrxs.eu -H=SCrypt -u=zip -o=.\\README_UNZIP.txt \n" +
            Environment.NewLine +
            Path.GetFileName(progName) + "\t -i=.\\README.MD -S -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\\README.MD.BCrypt.zip.xx \n" +
            Path.GetFileName(progName) + "\t -i=.\\README.MD.BCrypt.zip.xx -S -d=xx -q=io.cqrxs.eu -H=BCrypt -u=zip -o=.\\README_SYM_BCRYPT_UNZIP.txt \n\n" +
            Environment.NewLine +
            "start notepad README_UNZIP.txt\n\nstart notepad README_SYM_BCRYPT_UNZIP.txt\n\necho Finished\n\n\npause\n" +
            Environment.NewLine;

        static readonly string readmeMD = "https://cqrxs.eu/download/EU.CqrXs.Console/README.MD " + Environment.NewLine +
            Path.GetFileName(progName) + Environment.NewLine +
            "Usage: " + Path.GetFileName(progName) + Environment.NewLine + @"

    -i  | --inFile =    FileName 
        | --inText =    {String | EnviromentVariable}
        | --inStd
    -o  | --outFile =   FileName
        | --outText =   EnviromentVariable 
        | --outStd

    -u  | --unzip =     {gzip | bzip2}
    -z  | --zip =       {gzip | bzip2}
    -d  | --decode =    {raw | hex16 | hex32 | base32 | base64 | uu}
    -e  | --encode =    {raw | hex16 | hex32 | base32 | base64 | uu}
    -c  | --crypt =     {algo1, algo2, ...}
        algo:
            Aes,AesLight, Rijndael, Des, Des3, Dstu7624,
            Aria, Camellia, CamelliaLight, Cast5, Cast6,
            BlowFish, Fish2, Fish3,
            Gost28147, Idea, Noekeon,
            RC2, RC532, RC564, RC6,
            Seed, SkipJack, Serpent, SM4,
            Tea, Tnepres, XTea,
            ZenMatrix, ZenMatrix2
        symmAlgo: 
            Aes, BlowFish, Camellia, Cast6, Des3, Fish2, Fish3, Gost28147, Idea, RC532, Seed, SkipJack, Serpent, Tea, XTea, SM4 
    -p  | --pass =      Passphrase
    -D  | --decrypt =   {algo1, algo2,...}
    -p  | --pass =      Passphrase
    -k  | --key =       passKey encrypt
    -q  | --qey =       passKey decrypt
    -h  | --hash =      {Oct | Blake2xs | BCrypt | CShake | Dstu7564 | MD5 | RipeMD256 | SCrypt | Sha1 | Sha256 | Sha384 | Sha512 | Whirlpool | TupleHash}
    -S  | --SymmCipher
    -?  | --gethelp

Examples:" + Environment.NewLine +
    Path.GetFileName(progName) + " -i=test.jpg -z=bzip2 -e=base32 -o=test.jpg.bz2.base32 \n" +
    Path.GetFileName(progName) + " -i=test.jpg.gz.aeslight.fish3.base64 -d=base64  -D=AesLight,Fish3 -k=MySecretKey -e=base64  --unzip=gzip  -o=test2.jpg \n" +
    Path.GetFileName(progName) + " -i=.\\README.MD -z=zip -k=io.cqrxs.eu -H=SCrypt -e=uu -o=.\\README.MD.SCrypt.zip.uu \n" +

    Path.GetFileName(progName) + " -i=.\\README.MD.SCrypt.zip.uu -d=uu -q=io.cqrxs.eu -H=SCrypt -u=zip -o=.\\README_UNZIP.txt \n" +

    Path.GetFileName(progName) + " -i=.\\README.MD -z= zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\\README.MD.BCrypt.zip.xx \n" +
    Path.GetFileName(progName) + " -i=.\\README.MD.BCrypt.zip.xx -d= xx - q = io.cqrxs.eu - H = BCrypt - u = zip - o =.\\README_SYM_BCRYPT_UNZIP.txt \n";
        
        #endregion static readonly strings

    }

}

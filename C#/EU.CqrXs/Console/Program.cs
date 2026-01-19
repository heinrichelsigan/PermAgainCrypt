using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.Cipher.Symmetric;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using EU.CqrXs.Net.WebHttp;
using System.Buffers;
using System.Text;

namespace EU.CqrXs.Console
{

    /// <summary>
    /// OptEnum options that are posible
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

        YankeeBatchTest = 0x10,        
        LoadImage = 0x20        
    }

    /// <summary>
    /// struct OptionArgument
    /// </summary>
    public struct OptionArgument
    {
        static readonly char[] charSeperators = new char[] { '=', ':' };
        static SearchValues<char> searchValues = SearchValues.Create(new ReadOnlySpan<char>(charSeperators));

        public string Argument { get; private set; }
        public OptEnum OptKey { get; private set; }
        public string OptValue { get; private set; }

        internal string[] OptArgs
        {
            get => (Argument.ContainsAny(SearchValues.Create(new ReadOnlySpan<char>(charSeperators))) ?
                        Argument.Split(charSeperators, StringSplitOptions.RemoveEmptyEntries) :
                        new string[] { Argument });
        }

        public static OptionArgument GetOptArg(string argument)
        {
            if (argument != null && argument.Length > 1)
                argument = argument.TrimStart("-/".ToArray());

            string[] args = (argument.ContainsAny(SearchValues.Create(new ReadOnlySpan<char>(charSeperators))) ?
                        argument.Split(charSeperators, StringSplitOptions.RemoveEmptyEntries) :
                        new string[] { argument });
            OptionArgument optArg = new OptionArgument();
            optArg.OptKey = OptEnum.Usage;
            optArg.OptValue = (args.Length > 1 && args[1] != null) ? args[1] : "";
            switch (args[0][0])
            {
                case 'i':
                case 'I': optArg.OptKey = OptEnum.InParam; break;
                case 'o':
                case 'O': optArg.OptKey = OptEnum.OutP; break;
                case 'u': optArg.OptKey = OptEnum.Unzip; break;
                case 'z':
                case 'Z': optArg.OptKey = OptEnum.Zip; break;
                case 'd': optArg.OptKey = OptEnum.Decode; break;
                case 'e': optArg.OptKey = OptEnum.Encode; break;
                case 'c': optArg.OptKey = OptEnum.Crypt; break;
                case 'D': optArg.OptKey = OptEnum.Decrypt; break;
                case 'p': optArg.OptKey = OptEnum.Pass; break;
                case 'k': optArg.OptKey = OptEnum.Key; break;
                case 'q': optArg.OptKey = OptEnum.Qey; break;
                case 'h': optArg.OptKey = OptEnum.Hash; break;
                case 'S': optArg.OptKey = OptEnum.SymmCipher; break;
                case 'y': 
                case 'Y': optArg.OptKey = OptEnum.YankeeBatchTest; break;
                case 'l':
                case 'L': optArg.OptKey = OptEnum.LoadImage; break;
                case '?':
                default: optArg.OptKey = OptEnum.Usage; break;
            }

            return optArg;
        }

        public OptionArgument() { Argument = ""; OptKey = OptEnum.Usage; OptValue = ""; }
        public OptionArgument(string arg) : this()
        {
            Argument = arg ?? "";
            OptionArgument optArg = GetOptArg(arg);            
            OptKey = optArg.OptKey;
            OptValue = optArg.OptValue;
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
        public const string IMG_DOWNLOAD = "ImageDownload";
        public const string BATCH_FILE_TEST = "Console_Test.bat";
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

        static OptEnum optEnum;
        static OptionArgument optArg;

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

            optArg = new OptionArgument();
            optEnum = optArg.OptKey;
            string optStr = "", cryptOption = "";
            int readChars = -1;

            for (int i = 0; i < args.Length; i++)
            {
                // string optStr = GetOption(... => out OptEnum optEnum)
                optArg = OptionArgument.GetOptArg(args[i]);
                optEnum = optArg.OptKey;
                optStr = optArg.OptValue;

                if (optEnum == OptEnum.InParam)
                {
                    inName = optArg.OptValue;
                    if (!string.IsNullOrEmpty(inName))
                    {
                        if (args[i].ToLower().Contains("file") || File.Exists(inName) || File.Exists(Path.Combine(progDirectory, inName)))
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
                        else if (args[i].ToLower().Contains("text") || !string.IsNullOrEmpty(inName))
                        {
                            string? inStr = Environment.GetEnvironmentVariable(inName.TrimStart("$".ToCharArray()));
                            if (inStr == null || inStr.Length == 0)
                                inStr = inName;
                            inBytes = Encoding.UTF8.GetBytes(inStr);
                        }
                        else // if (string.IsNullOrEmpty(inName))
                            readChars = ReadFromStdin(ref inBytes, ref outBytes);
                    }
                }
                else // out parameter to file, to enviroment variable, to stdout
                    if (optEnum == OptEnum.OutP)
                {
                    outName = optArg.OptValue;
                    if (string.IsNullOrEmpty(outName))
                        ; // to stdout                    
                    else if (args[i].ToLower().Contains("file") || optArg.OptValue.Contains(LibPaths.SepChar) || optArg.OptValue.Contains('.') || !string.IsNullOrEmpty(outName))
                        outFile = new FileInfo(outName);
                    else if (!string.IsNullOrEmpty(outName) || args[i].ToLower().Contains("text") || optArg.OptValue.StartsWith("$"))
                        outEnviron = optArg.OptValue;
                }
                else // fetch  or Key or Qey (decrypt key) from optEnum and optStr
                    if (optEnum == OptEnum.Pass || optEnum == OptEnum.Key || optEnum == OptEnum.Qey)
                    passKey = optStr;
                else // prefetch SymmCipherMode
                    if (optEnum == OptEnum.SymmCipher)
                    useSymmCipher = true;
                // otherwise add optEnum and optStr to Dictionary<OptEnum, string>();  
                // else  dict.Add(optEnum, optStr);
                else // Zip Unzip
                    if (optEnum == OptEnum.Zip || optEnum == OptEnum.Unzip)
                {
                    if (optStr.ToLower().Contains("gz") || optStr.ToLower().Contains("gunzip"))
                        zipType = ZipType.GZip;
                    else if (optStr.ToLower().Contains("bz") || optStr.ToLower().Contains("bunzip") || optStr.ToLower().Contains("2"))
                        zipType = ZipType.BZip2;
                    else if (optStr.ToLower().Contains("zip") || optStr.ToLower().Contains("unzip"))
                        zipType = ZipType.Zip;
                    else
                        Usage("urecognized zip option: " + optStr);
                }
                else // Decode encode
                    if (optEnum == OptEnum.Encode || optEnum == OptEnum.Decode)
                    encodingType = EncodingTypesExtensions.GetEnum(optStr);
                else // hash
                    if (optEnum == OptEnum.Hash)
                    keyHash = KeyHash_Extensions.GetKeyHashFromString(optStr);
                else  // Crypt DeCrypt                  
                    if (optEnum == OptEnum.Crypt || optEnum == OptEnum.Decrypt)
                    cryptOption = optStr;
                else // _ batch file test
                    if (optEnum == OptEnum.YankeeBatchTest)
                {
                    ProcessCmd.Execute("start", $" {BATCH_FILE_TEST}", false);
                    System.Environment.Exit(0);
                }
                else // @ download image
                    if (optEnum == OptEnum.LoadImage)
                        DownloadImage(optStr);
        }

            if (!string.IsNullOrEmpty(cryptOption))
            {
                if (string.IsNullOrEmpty(passKey) || string.IsNullOrWhiteSpace(passKey))
                    Usage("option Crypt/DeCryptcryptOption without passkey -p key or -q in opt arg:" + cryptOption);

                // when string / array is not null, fetch array for crypt pipe
                if (!string.IsNullOrEmpty(cryptOption))
                {
                    cryptOption = cryptOption.Replace("(", "").Replace("{", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace(")", "");
                    algos = cryptOption.Split(",;:".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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
    -Y | --Yankee_batch_test
    -L | --LoadImage
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
            Path.GetFileName(progName) + "\t -i=.\\README.MD -z=gzip -c=BlowFish,Fish2,Fish3 -p=Hallo -e=base64 -o=.\\README.MD.BlowFish.Fish2.Fish3.base64 \n" +
            Path.GetFileName(progName) + "\t -i=.\\README.MD.BlowFish.Fish2.Fish3.base64 -d=base64 -D=BlowFish,Fish2,Fish3 -p=Hallo -u=gzip -o=.\\README_GUNZIP.txt \n " +
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

    -u  | --unzip =     {gzip | bzip2 | zip}
    -z  | --zip =       {gzip | bzip2 | zip}
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
    -D  | --decrypt =   {algo1, algo2,...}
    -p  | --pass =      Passphrase
    -k  | --key =       passKey encrypt
    -q  | --qey =       passKey decrypt
    -h  | --hash =      {Oct | Blake2xs | BCrypt | CShake | Dstu7564 | MD5 | RipeMD256 | SCrypt | Sha1 | Sha256 | Sha384 | Sha512 | Whirlpool | TupleHash}
    -S  | --SymmCipher
    -Y  | --Yánkee_batch_test
    -L  | --LoadImage
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

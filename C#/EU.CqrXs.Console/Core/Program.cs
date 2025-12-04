using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using System.Text;


namespace EU.CqrXs.Console.Core
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
        Hash = 0xe
    }


    /// <summary>
    /// Console app pipe for crypt/decrypt zip/unzip encode/decode md5sum/shaSum
    /// 
    /// EU.CqrXs.Console.Program 
    /// -i | --inFile= | --inText={string|EnviromentVariable} | --inStd    
    /// -o | --outFile= | --outText=EnviromentVariable | --outStd
    /// -u | --unzip={gzip|bzip2|zip}
    /// -z | --zip={gzip|bzip2|zip}
    /// -d | --decode={raw|hex16|hex32|base32|base64|uu}
    /// -e | --encode={raw|hex16|hex32|base32|base64|uu}
    /// -C | --crypt={[aes,des3,blowfish,fish2,fish3]|key}
    ///    |  -p --pass=Passphrase
    /// -D | --decrypt={[aes,des3,blowfish,fish2,fish3]|key}
    ///    |  -p --pass=Passphrase
    /// -k | --key=mykey
    /// -q | --qey=myqey    
    /// -H | --hash={Ascon256|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|Xoodyak         
    /// -? | --gethelp
    /// </summary>
    internal class Program
    {
        
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

        /// <summary>
        /// Console app pipe for crypt/decrypt zip/unzip encode/decode md5sum/shaSum
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length <= 1)
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

                // Nothing todo on io params
                if (optEnum == OptEnum.OutP || optEnum == OptEnum.InParam) ;
                else // Help => Usage()
                    if (optEnum == OptEnum.Help)
                    Usage();
                else // Usage with error message
                    if (optEnum == OptEnum.Usage)
                    Usage(optStr);
                else // fetch passphrase or Key or Qey (decrypt key) from optEnum and optStr
                    if (optEnum == OptEnum.Pass || optEnum == OptEnum.Key || optEnum == OptEnum.Qey)
                    passKey = optStr;
                else // otherwise add optEnum and optStr to Dictionary<OptEnum, string>();  
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

            // Create cipher pipe for en-/decryption
            CipherPipe pipe = (algos.Length > 0) ?
                                new CipherPipe(algos, Constants.MAX_PIPE_LEN, encodingType, zipType, keyHash) :
                                new CipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash);

            // System.Console.WriteLine($"CipherPipe: KeyHash={pipe.KHash} ZipTyoe={pipe.ZType} EncodeType={pipe.EncodeType} PipeString={pipe.PipeString}");

            if (!reverseDirection)
            {
                System.Console.Write($" InPipe: ");
                foreach (CipherEnum cipher in pipe.InPipe)
                    System.Console.Write($"{cipher}=>");
                System.Console.WriteLine($"\r\nCipherPipe: KeyHash={pipe.KHash} ZipTyoe={pipe.ZType} EncodeType={pipe.EncodeType} PipeString={pipe.PipeString}");
                // Encrypt process
                string outString = pipe.EncrpytEncode(inBytes, passKey, encodingType, zipType, keyHash);
                outBytes = System.Text.Encoding.UTF8.GetBytes(outString);
            }
            else
            {
                System.Console.Write($"OutPipe: ");
                foreach (CipherEnum cipher in pipe.OutPipe)
                    System.Console.Write($"{cipher}=>");
                System.Console.WriteLine($"\r\nCipherPipe: KeyHash={pipe.KHash} ZipTyoe={pipe.ZType} EncodeType={pipe.EncodeType} PipeString={pipe.PipeString}");
                // Decrypt process
                string inString = System.Text.Encoding.UTF8.GetString(inBytes);
                outBytes = pipe.DecodeDecrpyt(inString, passKey, encodingType, zipType, keyHash);
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
                case 'z': optEnum = OptEnum.Zip; 
                    return optArg;
                case 'U':
                case 'u': reverseDirection = true; optEnum = OptEnum.Unzip; 
                    return optArg;
                case 'E':
                case 'e': optEnum = OptEnum.Encode; 
                    return optArg;
                case 'd': reverseDirection = true; optEnum = OptEnum.Decode; 
                    return optArg;
                case 'C':
                case 'c': optEnum = OptEnum.Crypt; 
                    return optArg;
                case 'D': reverseDirection = true; optEnum = OptEnum.Decrypt; 
                    return optArg;
                case 'k':
                case 'K': optEnum = OptEnum.Key; 
                    return optArg;
                case 'p':
                case 'P': optEnum = OptEnum.Pass; 
                    return optArg;
                case 'q':
                case 'Q': reverseDirection = true; optEnum = OptEnum.Qey; 
                    return optArg;
                case 'h':
                case 'H': optEnum = OptEnum.Hash; 
                    return optArg;
                case 'g':
                case 'G':
                case '?': 
                default:  optEnum = OptEnum.Usage; 
                    optArg = $"unrecognized option: {argument}.";
                    return optArg;
            }
        }

        /// <summary>
        /// Usage shows the usage of console application
        /// </summary>
        static void Usage(string errMsg = "")
        {
            if (!string.IsNullOrEmpty(errMsg))
                System.Console.Error.WriteLine(errMsg);

            System.Console.Out.WriteLine("Usage:\t" + Path.GetFileName(progName) + @"
    -i | --inFile= | --inText={string|EnviromentVariable} | --inStd    
    -o | --outFile= | --outText=EnviromentVariable | --outStd
    -u | --unzip={gzip|bzip2}
    -z | --zip={gzip|bzip2}
    -d | --decode={raw|hex16|hex32|base32|base64|uu}
    -e | --encode={raw|hex16|hex32|base32|base64|uu}
    -c | --crypt={algo1,algo2,...}          
         algo:
            Aes,AesLight,Rijndael,Des,Des3,Dstu7624,
            Aria,Camellia,CamelliaLight,Cast5,Cast6,
            BlowFish,Fish2,Fish3,ThreeFish256,
            Gost28147,Idea,Noekeon,
            RC2,RC532,RC564,RC6,
            Seed,SkipJack,Serpent,SM4,
            Tea,Tnepres,XTea,
            ZenMatrix,ZenMatrix2
       |  -p --pass=Passphrase
    -D | --decrypt={[aes,des3,blowfish,fish2,fish3]|key}
       |  -p --pass=Passphrase    
    -k | --key=passKey encrypt
    -q | --qey=passKey decrypt
    -h | --hash={Ascon256|Blake2xs|BCrypt|CShake|Dstu7564|MD5|RipeMD256|SCrypt|Sha1|Sha256|Sha384|Sha512|Whirlpool|Xoodyak}      
    -? | --gethelp");

            System.Console.Out.WriteLine($"\nExamples:");
            System.Console.Out.WriteLine($"      \t{Path.GetFileName(progName)} -i=test.jpg -z=bzip2 -e=base32 -o=test.jpg.bz2.base32");
            System.Console.Out.WriteLine($"      \t{Path.GetFileName(progName)} -i=test.jpg.bz2.base32 -d=base32 -u=bzip2 -o=test1.jpg");
            System.Console.Out.WriteLine($"      \t{Path.GetFileName(progName)} --inFile=test.jpg --zip=gzip --crypt=AesLight,Fish3 -k=MySecretKey -e=base64 -o=test.jpg.gz.aeslight.fish3.base64");
            System.Console.Out.WriteLine($"      \t{Path.GetFileName(progName)} -i=test.jpg.gz.aeslight.fish3.base64 -d=base64  -D=AesLight,Fish3 -k=MySecretKey -e=base64  --unzip=gzip  -o=test2.jpg");

            System.Environment.Exit(0);
        }

    }

}

using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.Cipher.Symmetric;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using System.Text;

namespace EU.CqrXs.Console
{


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
    /// -D | --Decrypt 
    /// -S | --SymmCipher 
    /// -? | --gethelp
    /// </summary>
    internal class Program
    {
        const string BATCH_FILE_TEST = "Console_Test.bat";
        const string README_FILE = "README.MD";
        static readonly string? progName = System.Environment.ProcessPath;
        static readonly string? progDirectory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        static string? inName = null, outName = null, outEnviron = null, key = null;
        static bool reverseDirection = false, verbose = false, useSymmCipher = false;
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
            string[] algos = new List<string>().ToArray();
            encodingType = EncodingType.None;            
            Constants.DirCreate = false;
            Constants.NOLog = true;
            string encryptOptLater = "";

            if (args.Length <= 0)
                Usage();
                                   
            for (int i = 0; i < args.Length; i++)
            {
                // string optStr = GetOption(... => out OptEnum optEnum)
                string optStr = args[i].GetOption(out OptEnum optEnum);
                switch (optEnum)
                {
                    case OptEnum.InParam:
                        inName = optStr;
                        if (string.IsNullOrEmpty(inName))
                            ; // Else
                        else
                            if (args[i].ToLower().Contains("file") || File.Exists(inName) || File.Exists(Path.Combine(progDirectory, inName)))
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
                            if (args[i].ToLower().Contains("text") || !string.IsNullOrEmpty(inName))
                        {
                            string? inStr = Environment.GetEnvironmentVariable(inName.TrimStart("$".ToCharArray()));
                            if (inStr == null || inStr.Length == 0)
                                inStr = inName;
                            inBytes = Encoding.UTF8.GetBytes(inStr);
                        }
                        else
                            Usage($"unrecognized option: {args[i]}.");
                        break;
                    case OptEnum.OutP:
                        outName = optStr;
                        if (string.IsNullOrEmpty(outName))
                            ; // to stdout                    
                        else
                            if (args[i].ToLower().Contains("file") || optStr.Contains(LibPaths.SepChar) || optStr.Contains('.') || !string.IsNullOrEmpty(outName))
                            outFile = new FileInfo(outName);
                        else
                            if (!string.IsNullOrEmpty(outName) || args[i].ToLower().Contains("text") || optStr.StartsWith("$"))
                            outEnviron = optStr;
                        break;
                    case OptEnum.Zip:
                        if (optStr.ToLower().Contains("gz") || optStr.ToLower().Contains("gunzip"))
                            zipType = ZipType.GZip;
                        else if (optStr.ToLower().Contains("bz") || optStr.ToLower().Contains("bunzip") || optStr.ToLower().Contains("2"))
                            zipType = ZipType.BZip2;
                        else if (optStr.ToLower().Contains("zip") || optStr.ToLower().Contains("unzip"))
                            zipType = ZipType.Zip;
                        else
                            Usage($"urecognized zip option: {optStr}");
                        break;
                    case OptEnum.Encode:
                        encodingType = EncodingTypesExtensions.GetEnum(optStr);
                        break;
                    case OptEnum.Key:
                        passKey = optStr;
                        break;
                    case OptEnum.Hash:
                        keyHash = KeyHash_Extensions.GetKeyHashFromString(optStr);
                        break;
                    case OptEnum.SymmCipher:
                        useSymmCipher = true;
                        break;
                    case OptEnum.CipherAlgos:
                        encryptOptLater = optStr;
                        break;
                    case OptEnum.DeCrypt:
                        reverseDirection = true;
                        break;
                    case OptEnum.Verbose:
                        verbose = true;
                        break;
                    case OptEnum.Help:
                    case OptEnum.Usage:
                    default:
                        Usage(string.IsNullOrEmpty(optStr) ? "" : optStr);
                        break;
                }                               
        }

            // when string / array is not null, fetch array for crypt pipe
            if (!string.IsNullOrEmpty(encryptOptLater))
            {
                if (string.IsNullOrEmpty(passKey) || string.IsNullOrWhiteSpace(passKey))
                    Usage($"urecognized crypt option \"{encryptOptLater}\" without --key=secretKey ");

                encryptOptLater = encryptOptLater.Replace("(", "").Replace("{", "").Replace("[", "").Replace("]", "").Replace("}", "").Replace(")", "");
                algos = encryptOptLater.Split(",;:".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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
                        algos = algoList.ToArray();
                    }
                }
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
            
            if (useSymmCipher) 
            {
                // Create SymmCipherPipe // for reduced symmetric cipher pool only
                SymmCipherPipe symmPipe = (algos.Length > 0 || string.IsNullOrEmpty(passKey)) ?
                            new SymmCipherPipe(algos, 8, encodingType, zipType, keyHash) :
                            new SymmCipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, verbose);

                PrintSymmCipherPipe(symmPipe, reverseDirection);
                outBytes = symmPipe.CryptCodeBytes(inBytes, 
                    passKey ?? "", string.IsNullOrEmpty(passKey) ? "" : keyHash.Hash(passKey),
                    reverseDirection, encodingType, zipType, keyHash);
            }
            else 
            {
                // Create cipher pipe for en-/decryption
                CipherPipe pipe = (algos.Length > 0 || string.IsNullOrEmpty(passKey)) ?
                                new CipherPipe(algos, Constants.MAX_PIPE_LEN, encodingType, zipType, keyHash) :
                                new CipherPipe(passKey, keyHash.Hash(passKey), encodingType, zipType, keyHash, CipherMode2.ECB, verbose);

                PrintCipherPipe(pipe, reverseDirection);
                outBytes = pipe.CryptCodeBytes(inBytes, 
                    passKey ?? "", string.IsNullOrEmpty(passKey) ? "" : keyHash.Hash(passKey),
                    reverseDirection, encodingType, zipType, keyHash);
            }
            
            inBytes = outBytes;

            if (outFile != null)
                File.WriteAllBytes(outFile.FullName, outBytes);
            else if (string.IsNullOrEmpty(outName))
                System.Console.WriteLine(Encoding.UTF8.GetString(outBytes));
            else
                if (!string.IsNullOrEmpty(outEnviron))
                System.Environment.SetEnvironmentVariable(outEnviron, Encoding.UTF8.GetString(outBytes));

            return;
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

    EU.CqrXs.Console.exe -i=.\README.MD -z=zip -k=io.cqrxs.eu -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4 -H=SCrypt -e=uu -o=.\README.MD.SCrypt.zip.uu
    EU.CqrXs.Console.exe -D -i=.\README.MD.SCrypt.zip.uu -e=uu -k=io.cqrxs.eu -C=Aes,Blowfish,Des3,Fish2,Fish3,Seed,Serpent,SM4 -H=SCrypt -z=zip -o=.\READ_UNZIP.txt

    EU.CqrXs.Console.exe -i=.\README.MD -S -z=zip -k=io.cqrxs.eu -H=BCrypt -e=xx -o=.\README.MD.BCrypt.zip.xx
    EU.CqrXs.Console.exe -D -i=.\README.MD.BCrypt.zip.xx -S -e=xx -k=io.cqrxs.eu -H=BCrypt -z=zip -o=.\README_SYM_BCRYPT_UNZIP.txt\n\n");

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

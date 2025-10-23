using Area23.At.Framework.Core;
using Area23.At.Framework;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Zip;
using Area23.At.Framework.Core.Util;

namespace Area23.At.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cipher,ZipType,OpTime,FullName,MB/s,Size KB");
            DateTime startOp = DateTime.Now;
            TimeSpan opTime = TimeSpan.Zero;
            string fileName = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            if (args.Length > 0 && !string.IsNullOrEmpty(args[0]) && File.Exists(args[0]))
                fileName = args[0];

            foreach (CipherEnum cipher in CipherEnumExtensions.GetCipherTypes())
            {
                //if (cipher != CipherEnum.ZenMatrix || cipher != CipherEnum.ZenMatrix2)
                //    continue;

                CipherEnum[] cipherEnums = new CipherEnum[] { cipher };
                ZipType[] zTypes = new ZipType[] { ZipType.None, ZipType.GZip, ZipType.BZip2, ZipType.Zip };
                CipherPipe pipe = new CipherPipe(cipherEnums);
                byte[] plainBytes = File.ReadAllBytes(fileName);
                foreach (ZipType zType in zTypes)
                {
                    try
                    {
                        startOp = DateTime.Now;
                        byte[] outBytes = pipe.EncrpytFileBytesGoRounds(plainBytes,
                            Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
                            EncodingType.Base64, zType, KeyHash.Hex);
                        opTime = DateTime.Now.Subtract(startOp);
                        long mbs = (long)((850) / opTime.TotalSeconds);
                        string fName = fileName + zType.GetZipTypeExtension() + "." + cipher.ToString();
                        File.WriteAllBytes(fName, plainBytes);
                        Thread.Sleep(125);
                        double size = outBytes.Length / (1024);
                        Console.WriteLine(cipher.ToString() + "," + zType.ToString() + "," + opTime.TotalSeconds + "." + opTime.Milliseconds + "." + opTime.Microseconds + "," + Path.GetFileName(fName) + "," + mbs + "," + size);
                    } catch (Exception e)
                    {
                        Console.WriteLine(cipher.ToString() + " \t" + zType.ToString() + " \tException: " + e.Message);
                    }

                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

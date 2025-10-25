using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using Org.BouncyCastle.Crypto;
using System.Configuration;
using System.Xml.Linq;

namespace Area23.At.PermAgainCrypt.Test
{
    /// <summary>
    /// TestEncryptionSingleAlgo tests all encryption / decryption via <see cref="CipherEnum"/> = <see cref="CipherEnumExtensions.GetCipherTypes()" />
    /// Aes, BlowFish, Camellia, Cast6, Des3, Fish2, Fish3, ...        
    /// </summary>
    [TestClass]
    public sealed class TestEncryptionSingleAlgo
    {
        // TODO: Fix ZenMatrix in Test

        [TestMethod]
        public void TestAllEncryptionSingleAlgo()
        {
            Console.WriteLine("TestEncryptionSingleAlgo.TestAllEncryptionSingleAlgo() \t[started]");
            DateTime startOp = DateTime.Now, midOp = DateTime.Now, endOp = DateTime.Now;
            TimeSpan encOpTime = TimeSpan.Zero, decOpTime = TimeSpan.Zero, allOpTime = TimeSpan.Zero;
            string fileBytesTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            string fileTextTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";
            string dirCsvOut = "";
            string fileCsvOut = AppContext.BaseDirectory + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_hh_") + "Stats.csv";
            if (ConfigurationManager.AppSettings != null && ((dirCsvOut = ConfigurationManager.AppSettings["StatDir"]) != null) && Directory.Exists(dirCsvOut)) 
                fileCsvOut = dirCsvOut + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_hh_") + "Stats.csv";            
            File.WriteAllText(fileCsvOut, "FullName,Size[KB],Cipher,EncOpTime,DecOptTime,AllOpTime");

            Assert.IsTrue(File.Exists(fileTextTest));
            CipherEnum[] cipherTypes = CipherEnumExtensions.GetCipherTypes();
            ZipType[] zTypes = new ZipType[] { ZipType.None };
            KeyHash kHash = KeyHash.Hex;
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Hex32, EncodingType.Hex16 };
            EncodingType encType = EncodingType.Base64;            
            string plainText = File.ReadAllText(fileTextTest);
            byte[] plainBytes = File.ReadAllBytes(fileBytesTest);
            foreach (CipherEnum cipherEnum in cipherTypes)
            {
                CipherEnum[] cipherEnums = new CipherEnum[] { cipherEnum };
                CipherPipe pipe = new CipherPipe(cipherEnums); // new CipherPipe(Encoding.UTF8.GetBytes(Constants.AUTHOR_EMAIL), 0);
                try
                {
                    startOp = DateTime.Now;
                    byte[] cipherBytes = pipe.EncrpytFileBytesGoRounds(plainBytes, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
                                                encType, zType, kHash);
                    Assert.IsNotNull(cipherBytes);
                    
                    midOp = DateTime.Now;
                    encOpTime = midOp.Subtract(startOp);
                    byte[] deCodedBytes = pipe.DecryptFileBytesRoundsGo(cipherBytes, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
                                            encType, zType, kHash);
                    Assert.IsTrue(plainBytes != null && deCodedBytes != null &&  deCodedBytes.Length > 0 && plainBytes.Length == deCodedBytes.Length);
                    
                    endOp = DateTime.Now;
                    decOpTime = endOp.Subtract(midOp);
                    allOpTime = endOp.Subtract(startOp);

                    if (deCodedBytes == null || deCodedBytes.Length < 1 || deCodedBytes.Length != plainBytes.Length)
                    {
                        Console.WriteLine($"{cipherEnum} \tencrypt in {encOpTime} \tdecrypt in {decOpTime} \ttotal {allOpTime} [failed]");
                        Console.WriteLine($"          \tdeCodedBytes.Length ({deCodedBytes.Length}) != plainBytes.Length ({plainBytes.Length})");
                        Assert.Fail();
                    }
                    Console.WriteLine($"{cipherEnum} \tencrypt in {encOpTime.ToString("ss'.'fff")} \tdecrypt in {decOpTime.ToString("ss'.'fff")} \ttotal {allOpTime.ToString("mm':'ss'.'fff")} [passed]");
                    double size = deCodedBytes.Length / (1024);
                    File.WriteAllText(fileCsvOut, 
                        $"{Path.GetFileName(fileBytesTest)},{size},{cipherEnum},{encOpTime.ToString("ss'.'fff")},{decOpTime.ToString("ss'.'fff")},{allOpTime.ToString("mm':'ss'.'fff")}");
                    

                }
                catch (Exception e)
                {
                    Console.WriteLine($"{cipherEnum} \tException: {e.GetType()} \t{e.Message}\r\n      \t{e.StackTrace}");
                }                

            }
            return;
        }

    }
}

using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using Org.BouncyCastle.Crypto;
using System.Configuration;
using System.Reflection;
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
        internal static string Email = Constants.AUTHOR_EMAIL;

        [TestMethod]
        public void TestAllEncryptionSingleAlgo()
        {
            string className = "TestEncryptionSingleAlgo";
            string methodBase = "TestAllEncryptionSingleAlgo";
            try
            {
                className = MethodBase.GetCurrentMethod().DeclaringType.Name;
                methodBase = MethodBase.GetCurrentMethod().Name;
                Email = RegistryAccessor.GetEmailFromRegistry();
            }
            catch
            {
                className = this.GetType().BaseType.Name;
                methodBase = "TestAllEncryptionSingleAlgo";
                Email = Constants.AUTHOR_EMAIL;
            }
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[started]");

            DateTime startOp = DateTime.Now, midOp = DateTime.Now, endOp = DateTime.Now;
            TimeSpan encOpTime = TimeSpan.Zero, decOpTime = TimeSpan.Zero, allOpTime = TimeSpan.Zero;
            string fileBytesTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            string fileTextTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";
            string dirCsvOut = "";
            string fileCsvOut = AppContext.BaseDirectory + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_hh_") + $"{className}_{methodBase}.csv";
            if (ConfigurationManager.AppSettings != null && ((dirCsvOut = ConfigurationManager.AppSettings["StatDir"]) != null) && Directory.Exists(dirCsvOut)) 
                fileCsvOut = dirCsvOut + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_hh_") + $"{className}_{methodBase}.csv";
            File.WriteAllText(fileCsvOut, "FullName,Size[KB],Email,Cipher,EncOpTime,DecOptTime,AllOpTime" + Environment.NewLine);

            Assert.IsTrue(File.Exists(fileTextTest));            
            List<CipherEnum> cipherENumList = CipherEnumExtensions.GetCipherTypes().ToList();
            cipherENumList.RemoveAt(cipherENumList.Count - 1);
            // CipherEnum[] cipherEnums = cipherENumList.ToArray(); // CipherEnumExtensions.GetCipherTypes();
            CipherEnum[] cipherTypes = cipherENumList.ToArray(); // CipherEnumExtensions.GetCipherTypes();
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
                    byte[] cipherBytes = pipe.EncrpytFileBytesGoRounds(plainBytes, Email, KeyHash.Hex.Hash(Email),
                                                encType, zType, kHash);
                   Assert.IsNotNull(cipherBytes);
                    
                    midOp = DateTime.Now;
                    encOpTime = midOp.Subtract(startOp);
                    byte[] deCodedBytes = pipe.DecryptFileBytesRoundsGo(cipherBytes, Email, KeyHash.Hex.Hash(Email),
                                            encType, zType, kHash);
                    Assert.IsTrue(plainBytes != null && deCodedBytes != null &&  deCodedBytes.Length > 0 && 
                        ((plainBytes.Length == deCodedBytes.Length) || Math.Abs(deCodedBytes.Length - plainBytes.Length) <= 16));

                    endOp = DateTime.Now;
                    decOpTime = endOp.Subtract(midOp);
                    allOpTime = endOp.Subtract(startOp);

                    if (deCodedBytes != null && deCodedBytes.Length > 0 &&
                        (Math.Abs(deCodedBytes.LongLength - plainBytes.LongLength) < 16))
                    {
                        long difference = deCodedBytes.CompareBytes(plainBytes, true);
                        if (difference > 0)
                        {
                            Console.WriteLine($"{cipherEnum} for {Email}\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                            Console.WriteLine($"          \tdeCodedBytes.Length ({deCodedBytes.Length}) != plainBytes.Length ({plainBytes.Length})");
                            Assert.Fail();
                        }
                    }
                    Console.WriteLine($"{cipherEnum} for {Email}\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");
                    double size = deCodedBytes.Length / (1024);
                    File.AppendAllText(fileCsvOut, 
                        $"{Path.GetFileName(fileBytesTest)},{size},{Email},{cipherEnum},{encOpTime.ToString("ss'.'ffff")},{decOpTime.ToString("ss'.'ffff")},{allOpTime.ToString("ss'.'ffff")}" +
                        Environment.NewLine);


                }
                catch (Exception e)
                {
                    Console.WriteLine($"{cipherEnum} for {Email} \tException: {e.GetType()} \t{e.Message}\r\n      \t{e.StackTrace}");
                }                

            }
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[finished]");
            return;
        }

    }
}

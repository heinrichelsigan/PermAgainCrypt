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
    public sealed class TestEncryptionHashingAlgo
    {
        internal static string Email = Constants.AUTHOR_EMAIL;

        [TestMethod]
        public void TestAllEncryptionHashingAlgo()
        {
            string className = "TestEncryptionHashingAlgo";
            string methodBase = "TestAllEncryptionHashingAlgo";
            try
            {
                className = MethodBase.GetCurrentMethod().DeclaringType.Name;
                methodBase = MethodBase.GetCurrentMethod().Name;
                Email = RegistryAccessor.GetEmailFromRegistry();
            }
            catch
            {
                className = this.GetType().BaseType.Name;
                methodBase = "TestAllEncryptionHashingAlgo";
                Email = Constants.AUTHOR_EMAIL;
            }
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[started]");

            DateTime startOp = DateTime.Now, midOp = DateTime.Now, endOp = DateTime.Now;
            TimeSpan encOpTime = TimeSpan.Zero, decOpTime = TimeSpan.Zero, allOpTime = TimeSpan.Zero;
            string fileByesTest = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            string fileTextTest = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";
            string dirCsvOut = "";
            string fileCsvOut = AppContext.BaseDirectory + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_hh_") + $"{className}_{methodBase}.csv";            

            Assert.IsTrue(File.Exists(fileTextTest));
            CipherEnum[] cipherTypes = CipherEnumExtensions.GetCipherTypes();
            ZipType[] zTypes = new ZipType[] { ZipType.None };
            KeyHash kHash = KeyHash.Hex;
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Hex32, EncodingType.Hex16 };
            EncodingType encType = EncodingType.Base64;            
            string plainText = File.ReadAllText(fileTextTest);
            byte[] plainBytes = File.ReadAllBytes(fileByesTest);
            KeyHash[] khs = KeyHash_Extensions.GetHashTypes();
            int khcnt = 0;
            foreach (CipherEnum cipherEnum in cipherTypes)
            {
                if (cipherEnum == CipherEnum.Rsa)
                    continue;
                CipherEnum[] cipherEnums = new CipherEnum[] { cipherEnum };
                CipherPipe pipe = new CipherPipe(cipherEnums); // new CipherPipe(Encoding.UTF8.GetBytes(Constants.AUTHOR_EMAIL), 0);
                KeyHash khane = khs[(khcnt)%(khs.Length)];
                encType = encodingTypes[(khcnt) % (encodingTypes.Length)];
                khcnt++;
                try
                {
                    startOp = DateTime.Now;
                    byte[] cipherBytes = pipe.EncrpytFileBytesGoRounds(plainBytes, Email, KeyHash.Hex.Hash(Email),
                                                encType, zType, khane);
                   Assert.IsNotNull(cipherBytes);
                    
                    midOp = DateTime.Now;
                    encOpTime = midOp.Subtract(startOp);
                    byte[] deCodedBytes = pipe.DecryptFileBytesRoundsGo(cipherBytes, Email, KeyHash.Hex.Hash(Email),
                                            encType, zType, khane);
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
                            Console.WriteLine($"{khane}/{cipherEnum}/{encType} for {Email}\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                            Console.WriteLine($"                              \tdeCodedBytes.Length ({deCodedBytes.Length}) != plainBytes.Length ({plainBytes.Length})");
                            Assert.Fail();
                        }
                    }
                    double size = deCodedBytes.Length / (1024);
                    Console.WriteLine($"{khane}/{cipherEnum}/{encType} for {Email} {size}KB\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{cipherEnum}/{encType}/{khane} for {Email} \tException: {e.GetType()} \t{e.Message}\r\n      \t{e.StackTrace}");
                }                

            }
            
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[finished]");
            return;
        }

    }
}

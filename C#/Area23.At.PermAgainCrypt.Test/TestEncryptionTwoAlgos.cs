using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using Org.BouncyCastle.Tls;
using System.Configuration;
using System.Drawing;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Area23.At.PermAgainCrypt.Test
{
    /// <summary>
    /// TestEncryptionTwoAlgos tests all en- / decryption with 2 <see cref="CipherEnum"/>  algos in <see cref="CipherPipe"/>
    /// Aes => Aes, Aes => BlowFish, Aes => Camellia, 
    /// BlowFish => Aes, BlowFish => BlowFish, BlowFish => Camellia, 
    /// Camellia => Aes, Camellia => BlowFish, Camellia => Camellia, ...
    /// </summary>
    [TestClass]
    public sealed class TestEncryptionTwoAlgos
    {
        internal static string Email = Constants.AUTHOR_EMAIL;

        [TestMethod]
        public void TestAllEncryptionTwoAlgosBytes()
        {
            string className = "TestEncryptionTwoAlgos";
            string methodBase = "TestAllEncryptionTwoAlgosBytes";
            try
            {
                className = MethodBase.GetCurrentMethod().DeclaringType.Name;
                methodBase = MethodBase.GetCurrentMethod().Name;
                Email = RegistryAccessor.GetEmailFromRegistry();
            }
            catch
            {
                className = this.GetType().BaseType.Name;
                methodBase = "TestAllEncryptionTwoAlgosBytes";
                Email = Constants.AUTHOR_EMAIL;
            }
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[started]");
            
            DateTime startOp = DateTime.Now, midOp = DateTime.Now, endOp = DateTime.Now;
            TimeSpan encOpTime = TimeSpan.Zero, decOpTime = TimeSpan.Zero, allOpTime = TimeSpan.Zero;
            string fileBytesTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            string fileTextTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";
            string dirCsvOut = "";
            string fileCsvOut = AppContext.BaseDirectory + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_hh_") + $"{className}_{methodBase}.csv";
            
            Assert.IsTrue(File.Exists(fileTextTest));
            CipherEnum[] cipherEnums = CipherEnumExtensions.GetCipherTypes();
            ZipType[] zTypes = new ZipType[] { ZipType.None, ZipType.Zip, ZipType.GZip, ZipType.BZip2 };
            KeyHash kHash = KeyHash.Hex;
            KeyHash[] kHashes = KeyHash_Extensions.GetHashTypes();
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = EncodingTypesExtensions.GetEncodingTypes();
            EncodingType encType = EncodingType.Base64;
            string plainText = File.ReadAllText(fileTextTest);
            int j = 0;
            for (int i = 0; i < cipherEnums.Length; i += 2)
            {
                CipherEnum cipherType = cipherEnums[i];
                CipherEnum cipherEnum = cipherEnums[((i + 1) % cipherEnums.Length)];
                if (cipherType == CipherEnum.Rsa) cipherType = CipherEnum.Des;
                if (cipherEnum == CipherEnum.Rsa) cipherEnum = CipherEnum.BlowFish;

                byte[] plainBytes = File.ReadAllBytes(fileBytesTest);
                CipherEnum[] cipherPair = new CipherEnum[] { cipherType, cipherEnum };
                zType = zTypes[j % zTypes.Length];
                kHash = kHashes[j % kHashes.Length];
                if ((encType = encodingTypes[++j % encodingTypes.Length]) == EncodingType.None)
                    encType = EncodingType.Base64;
                
                CipherPipe pipe = new CipherPipe(cipherPair, 8, encType, zType, kHash);                

                try
                {
                    startOp = DateTime.Now;
                    byte[] cipherBytes = pipe.EncrpytFileBytesGoRounds(plainBytes, Email, kHash.Hash(Email),
                                                encType, zType, kHash);
                    Assert.IsNotNull(cipherBytes);

                    midOp = DateTime.Now;
                    encOpTime = midOp.Subtract(startOp);
                    byte[] deCodedBytes = pipe.DecryptFileBytesRoundsGo(cipherBytes, Email, kHash.Hash(Email),
                                            encType, zType, kHash);
                    Assert.IsTrue(plainBytes != null && deCodedBytes != null && deCodedBytes.Length > 0 && deCodedBytes.Length > 0 &&
                        plainBytes.LongLength == deCodedBytes.LongLength && plainBytes[i] == deCodedBytes[i]);

                    endOp = DateTime.Now;
                    decOpTime = endOp.Subtract(midOp);
                    allOpTime = endOp.Subtract(startOp);

                    if (deCodedBytes != null && deCodedBytes.Length > 0 && plainBytes != null && plainBytes.LongLength > 0 &&
                        (Math.Abs(deCodedBytes.LongLength - plainBytes.LongLength) < 16))
                    {
                        long difference = deCodedBytes.CompareBytes(plainBytes, true);
                        if (difference > 0)
                        {
                            Console.WriteLine($"{kHash} {cipherType}=>{cipherEnum} {zType} {encType} for {Email}\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                            Console.WriteLine($"          \tdeCodedBytes.Length ({deCodedBytes.Length}) != plainBytes.Length ({plainBytes.Length})");
                            Assert.Fail();
                        }
                    }

                    double size = deCodedBytes.Length / (1024);
                    Console.WriteLine($"{kHash} {cipherType}=>{cipherEnum} {zType} {encType} {size}KB for {Email} \tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{kHash} {cipherType}=>{cipherEnum} {zType} {encType} for {Email} \tException: {e.GetType()} \t{e.Message}\r\n      \t{e.StackTrace}");
                }
            }
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[finished]");
            return;
        }


        [TestMethod]
        public void TestAllEncryptionTwoAlgosAsciiText()
        {
            string className = "TestEncryptionTwoAlgos";
            string methodBase = "TestAllEncryptionTwoAlgosAsciiText";
            try
            {
                className = MethodBase.GetCurrentMethod().DeclaringType.Name;
                methodBase = MethodBase.GetCurrentMethod().Name;
                Email = RegistryAccessor.GetEmailFromRegistry();
            }
            catch
            {
                className = this.GetType().BaseType.Name;
                methodBase = "TestAllEncryptionTwoAlgosAsciiText";
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
            CipherEnum[] cipherEnums = CipherEnumExtensions.GetCipherTypes();
            ZipType[] zTypes = new ZipType[] { ZipType.None, ZipType.Zip, ZipType.GZip, ZipType.BZip2 };
            KeyHash kHash = KeyHash.Hex;
            KeyHash[] kHashes = KeyHash_Extensions.GetHashTypes();
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Hex32, EncodingType.Hex16 };
            EncodingType encType = EncodingType.Base64;           
            string plainText = File.ReadAllText(fileTextTest);
            int j = 0;
            for (int i = 0; i < cipherEnums.Length; i++)
            {
                CipherEnum cipherType = cipherEnums[i];
                CipherEnum cipherEnum = cipherEnums[((i + 1) % cipherEnums.Length)];
                if (cipherType == CipherEnum.Rsa) cipherType = CipherEnum.Des;
                if (cipherEnum == CipherEnum.Rsa) cipherEnum = CipherEnum.BlowFish;

                CipherEnum[] cipherPair = new CipherEnum[] { cipherType, cipherEnum };
                zType = zTypes[j % zTypes.Length];
                kHash = kHashes[j % kHashes.Length];
                if ((encType = encodingTypes[++j % encodingTypes.Length]) == EncodingType.None)
                    encType = EncodingType.Base64;                
                
                CipherPipe pipe = new CipherPipe(cipherPair, 8, encType, zType, kHash);
                
                try
                {
                    startOp = DateTime.Now;
                    byte[] plainBytes = File.ReadAllBytes(fileByesTest);
                    string cryptText = pipe.EncrpytTextGoRounds(plainText, Email, kHash.Hash(Email), encType, zType, kHash);
                    Assert.IsTrue(!string.IsNullOrEmpty(cryptText));

                    midOp = DateTime.Now;
                    encOpTime = midOp.Subtract(startOp);
                    string deCodedText = pipe.DecryptTextRoundsGo(cryptText, Email, kHash.Hash(Email), encType, zType, kHash);
                    if (!string.IsNullOrEmpty(plainText) && !string.IsNullOrEmpty(deCodedText))
                        Assert.IsTrue(plainText.Length >= 0 && deCodedText.Length >= 0 && deCodedText.Length == plainText.Length);

                    endOp = DateTime.Now;
                    decOpTime = endOp.Subtract(midOp);
                    allOpTime = endOp.Subtract(startOp);

                    Assert.AreEqual<string>(plainText, deCodedText);

                    if (deCodedText == null || deCodedText.Length < 1 || (deCodedText.Length != plainText.Length) || !plainText.Equals(deCodedText))
                    {
                        Console.WriteLine($"{kHash} {zType} {cipherType}=>{cipherEnum} {encType} for {Email}\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                        Console.WriteLine($"          \tdeCodedBytes.Length ({deCodedText.Length}) != plainBytes.Length ({plainText.Length})");
                        Assert.Fail();
                    }
                    double size = deCodedText.Length / (1024);
                    Console.WriteLine($"{kHash} {zType} {cipherType}=>{cipherEnum} {encType} for {Email} {size}KB\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");
                                    }
                catch (Exception e)
                {
                    Console.WriteLine($"{kHash} {zType} {cipherType}=>{cipherEnum} {encType} for {Email}\tException: {e.GetType()} \t{e.Message}\r\n      \t{e.StackTrace}");
                }
            }
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[finished]");
            return;
        }

    }
}

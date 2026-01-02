using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using System.Reflection;

namespace Area23.At.PermAgainCrypt.Test
{
    /// <summary>
    /// TestEncryptionTwoAlgos tests all en- / decryption with 2 <see cref="CipherEnum"/>  algos in <see cref="CipherPipe"/>
    /// Aes => Aes, Aes => BlowFish, Aes => Camellia, 
    /// BlowFish => Aes, BlowFish => BlowFish, BlowFish => Camellia, 
    /// Camellia => Aes, Camellia => BlowFish, Camellia => Camellia, ...
    /// </summary>
    [TestClass]
    public sealed class TestEncryptionMaxAlgos
    {
        public static readonly string[] TestEmails = { "he@area23.at", "zen@area23.at", "helsigan@area23.at", "heinrich.elsigan@area23.at", "he23@area23.at",
            "postmaster@cqrxs.eu", "zen@smtp.area23.at", "nobody@io.cqrxs.eu",
            "heinrich.elsigan@gmail.com", "office.area23@gmail.com", "heinrich.elsigan@live.at", "heinrich.elsigan@proton.me" };

        [TestMethod]
        public void TestPartiallyEncryptionMaxAlgorithmsBytes()
        {           
            string className = "TestEncryptionMaxAlgos";
            string methodBase = "TestPartiallyEncryptionMaxAlgosBytes";
            try
            {
                className = MethodBase.GetCurrentMethod().DeclaringType.Name;
                methodBase = MethodBase.GetCurrentMethod().Name;
            }            
            catch 
            {
                className = this.GetType().BaseType.Name;
                methodBase = "TestPartiallyEncryptionMaxAlgosBytes";
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
            ZipType[] zTypes = new ZipType[] { ZipType.None, ZipType.GZip, ZipType.BZip2, ZipType.Zip };
            KeyHash[] kHashes = KeyHash_Extensions.GetHashTypes();
            KeyHash kHash = KeyHash.Hex;
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Hex32, EncodingType.Hex16 };
            EncodingType encType = EncodingType.Base64;
            string plainText = File.ReadAllText(fileTextTest);

            int i = 0, j = 0;
            foreach (string email in TestEmails) 
            {
                
                kHash = kHashes[j % kHashes.Length];
                encType = encodingTypes[j % encodingTypes.Length];
                zType = zTypes[(++j) % zTypes.Length];
                i++;

                string hashIv = kHash.Hash(email);
                CipherPipe pipe = new CipherPipe(email, hashIv, encType, zType, kHash);
                string pipeText = pipe.PipeString;

                byte[] plainBytes = File.ReadAllBytes(fileByesTest);

                try
                {
                    startOp = DateTime.Now;
                    byte[] cipherBytes = pipe.EncryptEncodeBytes(plainBytes, email, hashIv, encType, zType, kHash);
                    Assert.IsNotNull(cipherBytes);

                    midOp = DateTime.Now;
                    encOpTime = midOp.Subtract(startOp);
                    byte[] deCodedBytes = pipe.DecodeDecrpytBytes(cipherBytes, email, hashIv, encType, zType, kHash);
                    Assert.IsTrue(plainBytes != null && deCodedBytes != null && deCodedBytes.Length > 0 &&
                        (Math.Abs(deCodedBytes.Length - plainBytes.Length) <= 16));

                    Assert.IsTrue(plainBytes != null && deCodedBytes != null && deCodedBytes.Length > 0 && deCodedBytes.Length > 0 &&
                        (Math.Abs(deCodedBytes.LongLength - plainBytes.LongLength) < 16) &&
                        (plainBytes[0] == deCodedBytes[0] && plainBytes[1] == deCodedBytes[1] &&
                            plainBytes[i + 16] == deCodedBytes[i + 16] && plainBytes[i + 8] == deCodedBytes[i + 8]));
            
                    endOp = DateTime.Now;
                    decOpTime = endOp.Subtract(midOp);
                    allOpTime = endOp.Subtract(startOp);

                    if (deCodedBytes != null && deCodedBytes.Length > 0 &&
                        (Math.Abs(deCodedBytes.LongLength - plainBytes.LongLength) < 16)) 
                    {
                        long difference = deCodedBytes.CompareBytes(plainBytes, true);
                        if (difference > 0)
                        {
                            Console.WriteLine($"{pipeText} \tencrypt for {email} {zType} {encType} {kHash} in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                            Console.WriteLine($"           \tdeCodedBytes.Length ({deCodedBytes.Length}) != plainBytes.Length ({plainBytes.Length})");
                            Assert.Fail();
                        }
                    }
                    double size = deCodedBytes.Length / (1024);
                    Console.WriteLine($"{pipeText} {size}KB \tencrypt for {email} {zType} {encType} {kHash} in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");                   
               
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{pipeText} \tException: {e.GetType()} for {email} \t{e.Message}\r\n      \t{e.StackTrace}");
                }

            }

            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[finished]");
            return;
        }


    }
}

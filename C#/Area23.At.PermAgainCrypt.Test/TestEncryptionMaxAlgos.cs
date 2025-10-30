using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        public static readonly string[] TestEmails = { "he@area23.at", "zen@area23.at", "helsigan@area23.at", "heinrich.elsigan@area23.at",
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
            string fileBytesTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            string fileTextTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";
            string dirCsvOut = "";
            string fileCsvOut = AppContext.BaseDirectory + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_hh_") + $"{className}_{methodBase}.csv";
            if (ConfigurationManager.AppSettings != null && ((dirCsvOut = ConfigurationManager.AppSettings["StatDir"]) != null) && Directory.Exists(dirCsvOut))
                fileCsvOut = dirCsvOut + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_hh_") + $"{className}_{methodBase}.csv";
            File.WriteAllText(fileCsvOut, "FullName,Size[KB],CipherPipe,EncOpTime,DecOptTime,AllOpTime" + Environment.NewLine);

            Assert.IsTrue(File.Exists(fileTextTest));
            CipherEnum[] cipherEnums = CipherEnumExtensions.GetCipherTypes();
            ZipType[] zTypes = new ZipType[] { ZipType.None };
            KeyHash kHash = KeyHash.Hex;
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Hex32, EncodingType.Hex16 };
            EncodingType encType = EncodingType.Base64;
            string plainText = File.ReadAllText(fileTextTest);
            
            int i = 0;
            foreach (string email in TestEmails) 
            {
                string pipeText = "";
                string hashIv = KeyHash.Hex.Hash(email);
                i++;
                CipherPipe pipe = new CipherPipe(email, hashIv);
                foreach (CipherEnum cipher in pipe.InPipe)            
                    pipeText += cipher.ToString() + "→";
                
                
                byte[] plainBytes = File.ReadAllBytes(fileBytesTest);

                try
                {
                    startOp = DateTime.Now;
                    byte[] cipherBytes = pipe.EncrpytFileBytesGoRounds(plainBytes, email, hashIv,
                                                encType, zType, kHash);
                    Assert.IsNotNull(cipherBytes);

                    midOp = DateTime.Now;
                    encOpTime = midOp.Subtract(startOp);
                    byte[] deCodedBytes = pipe.DecryptFileBytesRoundsGo(cipherBytes, email, hashIv,
                                            encType, zType, kHash);
                    Assert.IsTrue(plainBytes != null && deCodedBytes != null && deCodedBytes.Length > 0 &&
                        (Math.Abs(deCodedBytes.Length - plainBytes.Length) <= 16));

                    Assert.IsTrue(plainBytes != null && deCodedBytes != null && deCodedBytes.Length > 0 && deCodedBytes.Length > 0 &&
                        plainBytes.LongLength == deCodedBytes.LongLength && plainBytes[i] == deCodedBytes[i]);
            
                    endOp = DateTime.Now;
                    decOpTime = endOp.Subtract(midOp);
                    allOpTime = endOp.Subtract(startOp);

                    if (deCodedBytes == null || deCodedBytes.Length < 1 || plainBytes.LongLength != deCodedBytes.LongLength || plainBytes[i] != deCodedBytes[i])
                    {
                        Console.WriteLine($"{pipeText} \tencrypt for {email} in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                        Console.WriteLine($"          \tdeCodedBytes.Length ({deCodedBytes.Length}) != plainBytes.Length ({plainBytes.Length})");
                        Assert.Fail();
                    }
                    Console.WriteLine($"{pipeText} \tencrypt for {email} in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");
                    double size = deCodedBytes.Length / (1024);
                    File.AppendAllText(fileCsvOut,
                        $"{Path.GetFileName(fileBytesTest)},{size},{pipeText},{encOpTime.ToString("ss'.'ffff")},{decOpTime.ToString("ss'.'ffff")},{allOpTime.ToString("ss'.'ffff")}" +
                        Environment.NewLine);
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

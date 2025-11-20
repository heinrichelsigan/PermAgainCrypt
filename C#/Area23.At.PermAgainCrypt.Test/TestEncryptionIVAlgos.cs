using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using System.Configuration;
using System.Diagnostics;
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
    public sealed class TestEncryptionIVAlgos
    {
        internal static string Email = Constants.AUTHOR_EMAIL;

        [TestMethod]
        public void TestEncryptionIVAlgorithmsBytes()
        {           
            string className = "TestEncryptionIVAlgos";
            string methodBase = "TestEncryptionIVAlgosBytes";
            try
            {
                className = MethodBase.GetCurrentMethod().DeclaringType.Name;
                methodBase = MethodBase.GetCurrentMethod().Name;
                Email = RegistryAccessor.GetEmailFromRegistry();
            }            
            catch 
            {
                className = this.GetType().BaseType.Name;
                methodBase = "TestEncryptionIVAlgoBytes";
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
            File.WriteAllText(fileCsvOut, "FullName,Size[KB],Email,CipherPipe,EncOpTime,DecOptTime,AllOpTime" + Environment.NewLine);

            Assert.IsTrue(File.Exists(fileTextTest));
            CipherEnum[] cipherEnums = CipherEnumExtensions.GetCipherTypes();
            ZipType[] zTypes = new ZipType[] { ZipType.None };
            KeyHash kHash = KeyHash.Hex;
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Hex32, EncodingType.Hex16 };
            EncodingType encType = EncodingType.Base64;            
            string plainText = File.ReadAllText(fileTextTest);            
            for (int i = 0; i < cipherEnums.Length - 3; i++)
            {

                CipherEnum[] cipherPair = new CipherEnum[] { cipherEnums[i], cipherEnums[i + 1], cipherEnums[i + 2], cipherEnums[i + 3] };
                CipherPipe pipe = new CipherPipe(cipherPair); // new CipherPipe(Encoding.UTF8.GetBytes(Constants.AUTHOR_EMAIL), 0);
                byte[] plainBytes = File.ReadAllBytes(fileBytesTest);

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
                    Assert.IsTrue(plainBytes != null && deCodedBytes != null && deCodedBytes.Length > 0 &&
                        (Math.Abs(deCodedBytes.Length - plainBytes.Length) <= 16));

                    Assert.IsTrue(plainBytes != null && deCodedBytes != null && deCodedBytes.Length > 0 && deCodedBytes.Length > 0 &&
                        plainBytes.LongLength == deCodedBytes.LongLength && plainBytes[i] == deCodedBytes[i]);
            
                    endOp = DateTime.Now;
                    decOpTime = endOp.Subtract(midOp);
                    allOpTime = endOp.Subtract(startOp);

                    if (deCodedBytes == null || deCodedBytes.Length < 1 || plainBytes.LongLength != deCodedBytes.LongLength || plainBytes[i] != deCodedBytes[i])
                    {
                        Console.WriteLine($"{cipherEnums[i]}=>{cipherEnums[i+1]}=>{cipherEnums[i+2]}=>{cipherEnums[i+3]} for {Email}\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                        Console.WriteLine($"          \tdeCodedBytes.Length ({deCodedBytes.Length}) != plainBytes.Length ({plainBytes.Length})");
                        Assert.Fail();
                    }
                    Console.WriteLine($"{cipherEnums[i]}=>{cipherEnums[i+1]}=>{cipherEnums[i+2]}=>{cipherEnums[i+3]} for {Email}\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");
                    double size = deCodedBytes.Length / (1024);
                    File.AppendAllText(fileCsvOut,
                        $"{Path.GetFileName(fileBytesTest)},{size},{Email}{cipherEnums[i]}=>{cipherEnums[i+1]}=>{cipherEnums[i+2]}=>{cipherEnums[i+3]},{encOpTime.ToString("ss'.'ffff")},{decOpTime.ToString("ss'.'ffff")},{allOpTime.ToString("ss'.'ffff")}" +
                        Environment.NewLine);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{cipherEnums[i]}=>{cipherEnums[i+1]}=>{cipherEnums[i+2]}=>{cipherEnums[i+3]} for {Email}\tException: {e.GetType()} \t{e.Message}\r\n      \t{e.StackTrace}");
                }

            }
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[finished]");
            return;
        }


    }
}
